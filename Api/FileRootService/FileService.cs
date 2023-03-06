namespace Api.FileRootService;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }
    
    public async Task<string> AddFileAsync(string fileName, string folder, IFormFile file)
    {
        try
        {
            var path = Path.Combine(_webHostEnvironment.WebRootPath, folder);
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);
            path = Path.Combine(_webHostEnvironment.WebRootPath, folder, $"{fileName}.pdf");
            using (var stream = File.Create(path))
            {
                await file.CopyToAsync(stream);
            }
            return path;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    
    public async Task<bool> DeleteFileAsync(string fileName, string folder)
    {
        var path = Path.Combine(_webHostEnvironment.WebRootPath, folder, $"{fileName}.pdf");
        if (File.Exists(path) == true)
        {
            await Task.Run(() => File.Delete(path)); 
            return true;
        }
        return false;
    }
}