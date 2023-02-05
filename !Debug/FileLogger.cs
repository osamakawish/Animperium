using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathAnim._Debug;

public class FileLogger
{
    private string Path() => $"{Directory.GetCurrentDirectory()}/{_fileName}.txt";
    private readonly string _fileName;
    private string _log = "";

    /// <summary>
    /// Do NOT add .txt at end of filename.
    /// </summary>
    /// <param name="fileName"></param>
    public FileLogger(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName)) fileName = "log";
        _fileName = fileName;
        var path = Path();
        if (!File.Exists(path)) File.Create(path);
    }

    public string LogLine(string append)
    {
        _log += append;

        using StreamWriter file = new(Path(),
            new FileStreamOptions {
                Access = FileAccess.Write,
                Mode = FileMode.Append
            });
        file.WriteLine(append);

        return _log;
    }

    public static string operator +(FileLogger fileLogger, string append) => fileLogger.LogLine(append);
}