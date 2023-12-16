

using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Plagas.Entities;
using Plagas.Services.Interfaces;

namespace Plagas.Services.Implementations
{
    public class AzureBlobStorageUploader : IFileUploader
    {
        private readonly AppSettings _settings;
        private readonly ILogger<AzureBlobStorageUploader> _logger;
        
        public AzureBlobStorageUploader(IOptions<AppSettings> options, ILogger<AzureBlobStorageUploader>logger) 
        {
        
        _settings = options.Value;
        _logger=logger;

        }

        public async Task<string> UploadFileAsync(string? base64Image, string? fileName)
        {
            if (string.IsNullOrWhiteSpace(base64Image) || string.IsNullOrWhiteSpace(fileName))
                return string.Empty;

            try
            {
                var client = new BlobServiceClient(_settings.StorageConfiguration.Path);
                var container = client.GetBlobContainerClient("imagenes");

                var blob = container.GetBlobClient(fileName);
                await using var stream = new MemoryStream(Convert.FromBase64String(base64Image));
                await blob.UploadAsync(stream, overwrite: true);

                _logger.LogInformation("Se subió correctamente el archivo {fileName} a Azure", fileName);

                return $"{_settings.StorageConfiguration.PublicUrl}/{fileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al subir el archivo {fileName} a Azure :{Message}", fileName, ex.Message);
                return string.Empty;
            }
        }
    }
}
