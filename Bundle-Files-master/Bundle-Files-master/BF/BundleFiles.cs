using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF
{
    internal class BundleFiles
    {
         public static void Bundle(FileInfo output, List<string> language, bool note, string sort, bool removeEmptyLines, string author)
        {
            try
            {
                var directoryPath = output.DirectoryName;
                if (directoryPath == null || !Directory.Exists(directoryPath))
                {
                    Console.WriteLine($"Directory '{directoryPath}' does not exist.");
                    return;
                }

                var filesToBundle = new List<string>();
                if (language.Contains("all", StringComparer.OrdinalIgnoreCase))
                {
                    filesToBundle = Directory.GetFiles(directoryPath).ToList();
                }
                else
                {
                    var allowedExtensions = language.Select(lang => $".{lang.ToLower()}").ToList();
                    filesToBundle = Directory.GetFiles(directoryPath)
                        .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLower()))
                        .ToList();
                }

                if (filesToBundle.Count == 0)
                {
                    Console.WriteLine("No files found for the specified languages.");
                    return;
                }

                if (sort == "type")
                {
                    filesToBundle.Sort((a, b) => Path.GetExtension(a).CompareTo(Path.GetExtension(b)));
                }
                else
                {
                    filesToBundle.Sort((a, b) => Path.GetFileName(a).CompareTo(Path.GetFileName(b)));
                }

                using (var fs = new StreamWriter(output.FullName, false, new System.Text.UTF8Encoding(false)))
                {
                    if (!string.IsNullOrEmpty(author))
                    {
                        fs.WriteLine($"// Author: {author}");
                    }

                    foreach (var file in filesToBundle)
                    {
                        if (note)
                        {
                            fs.WriteLine($"// Source: {Path.GetFileName(file)}");
                        }

                        var lines = File.ReadAllLines(file);
                        foreach (var line in lines)
                        {
                            if (!removeEmptyLines || !string.IsNullOrWhiteSpace(line))
                            {
                                fs.WriteLine(line);
                            }
                        }
                    }

                    Console.WriteLine($"File was created at {output.FullName}");
                }
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("File path was not found");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("File path is Unauthorized");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
