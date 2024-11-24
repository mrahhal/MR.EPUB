using HtmlAgilityPack;

namespace MR.EPUB;

public static class XmlHelper
{
	public static HtmlDocument Create(string content)
	{
		var doc = new HtmlDocument() { OptionEmptyCollection = true };
		doc.LoadHtml(content);
		return doc;
	}
}
