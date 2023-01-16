namespace MvcMovie.Services
{
    public class FindPath
    {
        public static string MapPath(string path)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(baseDirectory, path);
        }
    }
}
