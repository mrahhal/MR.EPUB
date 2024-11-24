using MR.EPUB;

var target = args[0];

var files = new List<string>();
if (Directory.Exists(target))
{
	files.AddRange(Directory.GetFiles(target, "*.epub"));
}
else if (!File.Exists(target))
{
	files.Add(target);
}

foreach (var file in files)
{
	Console.WriteLine($"Processing '{file}'.");

	var outputFilePath = Path.Combine(Path.GetDirectoryName(file)!, Path.GetFileNameWithoutExtension(file) + ".txt");
	var result = EPUBConverter.ConvertToPlainText(file);
	File.WriteAllText(outputFilePath, result);

	Console.WriteLine($"Done... Written to '{outputFilePath}'.");
}
