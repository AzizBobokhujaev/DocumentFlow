namespace Api.Helpers;

public static class TextHelper
{
    public static string ReplaceInvalidPathChars(string path)
    {
        var invalidChars = new string(Path.GetInvalidPathChars()) + new string(Path.GetInvalidFileNameChars());
        path = invalidChars.Aggregate(path, (current, c) => current.Replace(c.ToString(), "_"));
        switch (Path.DirectorySeparatorChar)
        {
            case '/':
                path = path.Replace("\\", "_");
                break;
            case '\\':
                path = path.Replace("/", "_");
                break;
        }
        return path;
    }
}