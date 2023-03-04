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
}