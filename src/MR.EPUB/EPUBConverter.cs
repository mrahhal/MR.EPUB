using System.Text;

namespace MR.EPUB;

public static class EPUBConverter
{
	public static string ConvertToPlainText(string epubFilePath)
	{
		using var epubFile = new EPUBFile(epubFilePath);
		var reader = new EPUBReader(epubFile);

		var sb = new StringBuilder();

		foreach (var doc in reader.EnumerateOrderedDocuments())
		{
			var writtenAnything = false;

			void Append(string text)
			{
				if (string.IsNullOrWhiteSpace(text))
				{
					return;
				}
				writtenAnything = true;
				sb.Append(text);
			}

			void AppendLine()
			{
				sb.AppendLine();
			}

			foreach (var pNode in doc.DocumentNode.SelectNodes("//p"))
			{
				foreach(var pChild in pNode.ChildNodes)
				{
					if (pChild.Name == "ruby")
					{
						foreach (var rubyChild in pChild.ChildNodes)
						{
							if (rubyChild.Name == "rt")
							{
								// ignore
							}
							else
							{
								Append(rubyChild.InnerText);
							}
						}
					}
					else if (pChild.Name == "br")
					{
						AppendLine();
					}
					else
					{
						Append(pChild.InnerText);
					}
				}

				AppendLine();
			}

			if (writtenAnything)
			{
				sb.AppendLine().AppendLine().AppendLine().AppendLine().AppendLine().AppendLine();
			}
		}

		return sb.ToString();
	}
}
