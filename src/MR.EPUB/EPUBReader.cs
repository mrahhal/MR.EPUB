using HtmlAgilityPack;

namespace MR.EPUB;

public class EPUBReader
{
	private const string EPUB_CONTAINER_FILE_PATH = "META-INF/container.xml";

	public EPUBReader(EPUBFile file)
	{
		File = file;
	}

	public EPUBFile File { get; }

	public IEnumerable<HtmlDocument> EnumerateOrderedDocuments()
	{
		var containerDoc = XmlHelper.Create(File.Read(EPUB_CONTAINER_FILE_PATH));

		var rootFiles = containerDoc.DocumentNode.SelectNodes("//rootfiles/rootfile");
		foreach (var rootFile in rootFiles)
		{
			var rootFileFullPath = rootFile.GetAttributes("full-path").FirstOrDefault()?.Value;
			if (rootFileFullPath == null)
			{
				continue;
			}
			var rootFileDirectoryPath = Path.GetDirectoryName(rootFileFullPath)!;

			var rootFileDoc = XmlHelper.Create(File.Read(rootFileFullPath));

			foreach (var itemDoc in EnumerateItemsInOpfDocument(rootFileDoc, rootFileDirectoryPath))
			{
				yield return itemDoc;
			}
		}
	}

	private IEnumerable<HtmlDocument> EnumerateItemsInOpfDocument(HtmlDocument doc, string directory)
	{
		foreach (var itemNode in doc.DocumentNode.SelectNodes("//package/manifest/item"))
		{
			var href = itemNode.GetAttributes("href").FirstOrDefault()?.Value;
			var acceptedExts = new[] { ".xhtml", ".xml" };
			if (href == null || acceptedExts.All(ext => !href.EndsWith(ext)))
			{
				continue;
			}

			var itemPath = Path.Combine(directory, href);
			var itemDoc = XmlHelper.Create(File.Read(itemPath));

			yield return itemDoc;
		}
	}
}
