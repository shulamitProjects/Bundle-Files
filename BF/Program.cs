using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.CommandLine.Help.HelpBuilder;

namespace BF
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            var outputOption = new Option<FileInfo>("--output", "Filepath and name") { IsRequired= true};
            var languageOption = new Option<List<string>>("--language", result => result.Tokens.Select(t => t.Value).ToList(), false, "List of programming languages") { IsRequired = true };
            var noteOption = new Option<bool>("--note","Include source code path as a comment");
            var sortOption = new Option<string>("--sort",() => "filename", "Sorting method (filename/type)");
            var removeEmptyLinesOption = new Option<bool>("--remove-empty-lines", "Remove empty lines");
            var authorOption = new Option<string>("--author","Author's name");

            var bundleCommand = new Command("bundle", "Bundle code files to a single file")
            {
                outputOption,
                languageOption,
                noteOption,
                sortOption,
                removeEmptyLinesOption,
                authorOption
            };

            bundleCommand.SetHandler((FileInfo output, List<string> language, bool note, string sort, bool removeEmptyLines, string author) =>
            {
                if (output == null || language == null || language.Count == 0)
                {
                    Console.WriteLine("Output file path and languages are required.");
                    return;
                }
                BundleFiles.Bundle(output, language, note, sort, removeEmptyLines, author);
            }, outputOption, languageOption, noteOption, sortOption, removeEmptyLinesOption, authorOption);

            var RspCommand = new Command("create-rsp", "Create a response file for the bundle command");
            RspCommand.SetHandler(() =>
            {
                CreateRspFile.Create();
            });

            var rootCommand = new RootCommand("Root command for file bundler CLI")
            {
                bundleCommand,
                RspCommand
            };

            return await rootCommand.InvokeAsync(args);
        }

    }
}
