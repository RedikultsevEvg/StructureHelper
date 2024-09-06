using System.IO;
using System.Threading.Tasks;

namespace DataAccess.FileDialogs
{
    public interface IFileRepository
    {
        Task SaveFileAsync(Stream fileStream, string fileName);
        Task<Stream> GetFileAsync(string fileName);
        Task DeleteFileAsync(string fileName);
        Task UpdateFileAsync(Stream fileStream, string fileName);
    }

}
