using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF
{
    internal class CreateRspFile
    {
        public static void Create()
        {
            Console.WriteLine("Enter output file path:");
            var output = Console.ReadLine();

            Console.WriteLine("Enter languages (comma-separated, use 'all' for all files):");
            var languages = Console.ReadLine();

            Console.WriteLine("Include source code path as a comment? (true/false):");
            var note = Console.ReadLine();

            Console.WriteLine("Sorting method (filename/type):");
            var sort = Console.ReadLine();

            Console.WriteLine("Remove empty lines? (true/false):");
            var removeEmptyLines = Console.ReadLine();

            Console.WriteLine("Enter author's name (optional):");
            var author = Console.ReadLine();

            var rspContent = $"BF bundle --output \"{output}\" --language \"{languages}\" --note {note} --sort {sort} --remove-empty-lines {removeEmptyLines} --author \"{author}\"";
            var rspFileName = "response.rsp";

            File.WriteAllText(rspFileName, rspContent);
            Console.WriteLine($"Response file '{rspFileName}' created. You can run the command with 'BF @response.rsp'.");
        }
    }
}
