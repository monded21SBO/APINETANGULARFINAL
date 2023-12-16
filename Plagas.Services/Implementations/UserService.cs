using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using plagas.Dto;
using Plagas.Dto;
using Plagas.Dto.Request;
using Plagas.Dto.Response;
using Plagas.Entities;
using Plagas.Persistence;
using Plagas.Repositories;
using Plagas.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Text;

namespace Plagas.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<PlagasUserIdentity> _userManager;
        private readonly ILogger<UserService> _logger;
        private readonly IOptions<AppSettings> _options;
        private readonly ITecnicoRepository _TecnicoRepository;
        private readonly IEmailService _emailService;

        public UserService(UserManager<PlagasUserIdentity> userManager, ILogger<UserService> logger, IOptions<AppSettings> options, ITecnicoRepository TecnicoRepository, IEmailService emailService)
        {
            _userManager = userManager;
            _logger = logger;
            _options = options;
            _TecnicoRepository = TecnicoRepository;
            _emailService = emailService;
        }

        public async Task<LoginDtoResponse> LoginAsync(LoginDtoRequest request)
        {
            var response = new LoginDtoResponse();

            try
            {
                // Codigo
                var identity = await _userManager.FindByEmailAsync(request.UserName);
                if (identity == null)
                {
                    throw new SecurityException("Usuario no existe");
                }

                if (await _userManager.IsLockedOutAsync(identity))
                {
                    throw new SecurityException($"Demasiados intentos fallidos para el usuario {identity.UserName}");
                }

                var result = await _userManager.CheckPasswordAsync(identity, request.Password);
                if (!result)
                {
                    response.Success = false;
                    response.ErrorMessage = "Clave incorrecta";

                    _logger.LogWarning("Error de autenticacion para el usuario {UserName}", request.UserName);
                    await _userManager.AccessFailedAsync(identity);

                    return response;
                }

                var roles = await _userManager.GetRolesAsync(identity);

                var expiredDate = DateTime.Now.AddSeconds(5000);

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, $"{identity.FirstName} {identity.LastName}"),
                new Claim(ClaimTypes.Email, request.UserName),
                new Claim(ClaimTypes.Expiration, expiredDate.ToString("yyyy-MM-dd HH:mm:ss")),
            };

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                response.Roles = new List<string>();
                response.Roles.AddRange(roles);

                // Creacion del JWT
                var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Jwt.SecretKey));

                var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
                var header = new JwtHeader(credentials);

                var payload = new JwtPayload(issuer: _options.Value.Jwt.Issuer,
                    audience: _options.Value.Jwt.Audience,
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: expiredDate);

                var token = new JwtSecurityToken(header, payload);
                response.Token = new JwtSecurityTokenHandler().WriteToken(token);
                response.FullName = $"{identity.FirstName} {identity.LastName}";
                response.Success = true;
            }
            catch (SecurityException ex)
            {
                response.ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al autenticar al usuario";
                _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }

            return response;

        }

        public async Task<BaseResponseGeneric<string>> RegisterAsync(RegisterDtoRequest request)
        {
            var response = new BaseResponseGeneric<string>();

            try
            {
                // Codigo
                var user = new PlagasUserIdentity()
                {
                    UserName = request.Email,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Age = request.Age,
                    DocumentNumber = request.DocumentNumber,
                    DocumentType = (DocumentTypeEnum)request.DocumentType,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, request.ConfirmPassword);
                if (result.Succeeded)
                {
                    user = await _userManager.FindByEmailAsync(request.Email);
                    if (user is not null)
                    {
                        await _userManager.AddToRoleAsync(user, Constantes.RolCustomer);

                        var customer = new Tecnicos()
                        {
                            Email = request.Email,
                            FullName = $"{request.FirstName} {request.LastName}"
                        };

                        await _TecnicoRepository.AddAsync(customer);

                        // TODO: Enviar un email

                        response.Data = user.Id;
                        response.Success = true;
                    }
                }
                else
                {
                    response.Success = false;
                    var sb = new StringBuilder();
                    foreach (var error in result.Errors)
                    {
                        sb.AppendLine(error.Description);
                    }

                    response.ErrorMessage = sb.ToString();
                    sb.Clear(); // liberamos memoria
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al registar al usuario";
                _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }

            return response;

        }

        public async Task<BaseResponse> RequestTokenToResetPasswordAsync(DtoResetPasswordRequest request)
        {
            var response = new BaseResponse();
            try
            {
                var userIdentity = await _userManager.FindByEmailAsync(request.Email);
                if (userIdentity is null)
                {
                    throw new SecurityException("Usuario no existe");
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(userIdentity);

                // Enviar un email con el token para reestablecer la contraseña
                await _emailService.SendEmailAsync(request.Email, "Reestablecer clave",
                                   @$"
                    <p> Estimado {userIdentity.FirstName} {userIdentity.LastName}</p>
                    <p> Para reestablecer su clave, por favor copie el siguiente codigo</p>
                    <p> <strong> {token} </strong> </p>
                    <hr />
                    Atte. <br />
                    Control de Plagas Microenvases © 2023
                ");

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al solicitar el token para resetear la clave";
                _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponse> ResetPasswordAsync(DtoConfirmPasswordRequest request)
        {
            var response = new BaseResponse();
            try
            {
                var userIdentity = await _userManager.FindByEmailAsync(request.Email);
                if (userIdentity is null)
                {
                    throw new SecurityException("Usuario no existe");
                }

                var result = await _userManager.ResetPasswordAsync(userIdentity, request.Token, request.ConfirmPassword);
                response.Success = result.Succeeded;

                if (!result.Succeeded)
                {
                    var sb = new StringBuilder();
                    foreach (var error in result.Errors)
                    {
                        sb.AppendLine(error.Description);
                    }

                    response.ErrorMessage = sb.ToString();
                    sb.Clear(); // limpiamos la memoria 
                }
                else
                {
                    // Enviar un email de confirmacion de clave cambiada
                    await _emailService.SendEmailAsync(request.Email, "Confirmacion de cambio de clave",
                        @$"
                    <p> Estimado {userIdentity.FirstName} {userIdentity.LastName}</p>
                    <p> Se ha cambiado su clave correctamente</p>
                    <hr />
                    Atte. <br />
                    Control de Plagas Microenvases © 2023");
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al resetear la clave";
                _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse> ChangePasswordAsync(string email, ChangePasswordRequest request)
        {
            var response = new BaseResponse();

            try
            {
                var userIdentity = await _userManager.FindByEmailAsync(email);
                if (userIdentity is null)
                {
                    throw new SecurityException("Usuario no existe");
                }

                var result =
                    await _userManager.ChangePasswordAsync(userIdentity, request.OldPassword, request.NewPassword);

                response.Success = result.Succeeded;

                if (!result.Succeeded)
                {
                    var sb = new StringBuilder();
                    foreach (var error in result.Errors)
                    {
                        sb.AppendLine(error.Description);
                    }

                    response.ErrorMessage = sb.ToString();
                    sb.Clear(); // limpiamos la memoria 
                }
                else
                {
                    _logger.LogInformation("Se cambio la clave para {email}", userIdentity.Email);

                    // Enviar un email de confirmacion de clave cambiada
                    await _emailService.SendEmailAsync(email, "Confirmacion de cambio de clave",
                        @$"
                    <p> Estimado {userIdentity.FirstName} {userIdentity.LastName}</p>
                    <p> Se ha cambiado su clave correctamente</p>
                    <hr />
                    Atte. <br />
                    Control de Plagas Microenvases © 2023");
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al cambiar la clave";
                _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }

            return response;
        }
    }
}
