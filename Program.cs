using System;
using System.IO;
using System.Xml;

namespace BetterTrxMerger
{
    /// <summary>
    /// Merges two trx files into an output file.
    /// Known issues/limitations: 
    /// - The totals, failed and passed tests in the generated file
    /// are not updated, hence incorrect.
    /// - No support for test lists
    /// </summary>
    class Program
    {
        static int Main(string[] args)
        {
            const string message = @"
Usage: BetterTrxMerger <receiving trx file> <donor trx file> [output file]
When output file is not defined, the data is merged into the receiving
trx file.

The trx file that is created does contain all information for generating
report files with the specflow.exe tool. Can result in undefined behaviour
when used with other tooling because totals, start/end times etc. are not
recalculated.
";
            if (args.Length < 2)
            {
                Console.WriteLine("Not enough arguments.\n{0}", message);
                return 1;
            }    
            if (!File.Exists(args[0]))
            {
                Console.WriteLine("File {0} does not exist.\n{1}", args[0], message);
                return 1;
            }
            if (!File.Exists(args[1]))
            {
                Console.WriteLine("File {0} does not exist.\n{1}", args[1], message);
                return 1;
            }

            var resultFile = (args.Length > 2) ? args[2] : args[0];
            
            var oDocReceiver = new XmlDocument();
            oDocReceiver.Load(args[0]);
            var oDocDonor = new XmlDocument();
            oDocDonor.Load(args[1]);
            
            oDocReceiver.MergeTrxFile(oDocDonor);
            oDocReceiver.Save(resultFile);
            return 0;
        }
    }
}
