using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagas.Services.Interfaces
{
    public interface IFileUploader
    {
        Task<string> UploadFileAsync(string? base64Image, string? fileName);
    }
}
