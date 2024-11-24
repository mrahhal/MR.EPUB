using System.IO.Compression;

namespace MR.EPUB;

public class EPUBFile : IDisposable
{
	private readonly ZipArchive _zip;
	private readonly string _extractedFolder;
	private bool _disposed;

	public EPUBFile(string filePath)
	{
		FilePath = filePath;

		_zip = ZipFile.OpenRead(filePath);
		_extractedFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
		// TODO:
		Console.WriteLine($"Extracted to: {_extractedFolder}");
		_zip.ExtractToDirectory(_extractedFolder);
	}

	public string FilePath { get; }

	public string Read(string innerPath)
	{
		var path = Path.Combine(_extractedFolder, innerPath);
		return File.ReadAllText(path);
	}

	public void Dispose()
	{
		if (_disposed)
		{
			return;
		}

		_disposed = true;
		GC.SuppressFinalize(this);
		_zip.Dispose();
		Directory.Delete(_extractedFolder, true);
	}
}
