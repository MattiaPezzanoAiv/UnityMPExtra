using System.IO;

public static class IOUtils
{
    private static string PreprocessPath(string path)
    {
        string result = path;
        if (!Path.HasExtension(path))
        {
            result += ".txt";
        }

        // if file doesn't exist we create the whole directory path then use a stream writer to be safe
        if (!File.Exists(path))
        {
            var dir = Path.GetDirectoryName(path);
            Directory.CreateDirectory(dir);
        }

        return result;
    }

    public static void SafeWriteFile(string path, string content)
    {
        var filePath = PreprocessPath(path);

        using (var sw = new StreamWriter(filePath))
        {
            sw.Write(content);
        }
    }

    public static void SafeWriteFileAsync(string path, string content, System.Action onComplete)
    {
        var filePath = PreprocessPath(path);
        MP.Coroutines.GlobalCoroutineRunner.Instance.StartCoroutine(WriteFileAsyncCoroutine(filePath, content, onComplete));
    }

    private static System.Collections.IEnumerator WriteFileAsyncCoroutine(string path, string content, System.Action onComplete)
    {
        using (var sw = new StreamWriter(path))
        {
            var task = sw.WriteAsync(content);

            while(!task.IsCompleted)
            {
                yield return null;
            }

            if(task.IsFaulted)
            {
                throw new UnityEngine.UnityException(task.Exception.Message);
            }

            onComplete?.Invoke();
        }
    }
}
