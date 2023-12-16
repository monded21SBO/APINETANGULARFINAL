using Plagas.Dto.Request;
using Plagas.Dto.Response;
using plagas.Dto;


namespace Plagas.Services.Interfaces
{
    public interface IUserService
    {
        Task<LoginDtoResponse> LoginAsync(LoginDtoRequest request);

        Task<BaseResponseGeneric<string>> RegisterAsync(RegisterDtoRequest request);

        Task<BaseResponse> RequestTokenToResetPasswordAsync(DtoResetPasswordRequest request);

        Task<BaseResponse> ResetPasswordAsync(DtoConfirmPasswordRequest request);

        Task<BaseResponse> ChangePasswordAsync(string email, ChangePasswordRequest request);
    }
}
