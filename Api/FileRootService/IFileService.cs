namespace Api.FileRootService;

public interface IFileService
{
    Task<string> AddFileAsync(string fileName, string folder, IFormFile file);
}