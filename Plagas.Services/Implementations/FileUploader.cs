using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Plagas.Entities;
using Plagas.Services.Interfaces;
using System.IO;

namespace Plagas.Services.Implementations
{
    public class FileUploader : IFileUploader
    {
        private readonly ILogger<FileUploader> _logger;
        private readonly AppSettings _appsettings;

        public FileUploader(IOptions<AppSettings> options, ILogger<FileUploader> logger)
        {
            _logger = logger;
            _appsettings = options.Value;
        }

        public async Task<string> UploadFileAsync(string? base64Image, string? fileName)
        {
            if (string.IsNullOrEmpty(base64Image) || string.IsNullOrEmpty(fileName))
            {
                return string.Empty;
            }
            _logger.LogInformation("Se subió correctamente la imagen con el nombre {base64Image}", base64Image);
            try
            {
                var bytes = Convert.FromBase64String(base64Image);



                var path = Path.Combine(_appsettings.StorageConfiguration.Path, fileName);
                

                await using var fileStream = new FileStream(path, FileMode.Create);
                await fileStream.WriteAsync(bytes, 0, bytes.Length);

                _logger.LogInformation("Se subió correctamente la imagen con el nombre {fileName}", fileName);

                return $"{_appsettings.StorageConfiguration.PublicUrl}{fileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al subir el archivo {fileName} {Message}", fileName, ex.Message);
                return string.Empty;
            }
        }
    }
}
