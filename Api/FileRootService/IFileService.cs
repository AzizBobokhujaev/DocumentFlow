namespace Api.FileRootService;

public interface IFileService
{
    Task<string> AddFileAsync(string fileName, string folder, IFormFile file);
    Task<bool> DeleteFileAsync(string fileName, string folder);
}