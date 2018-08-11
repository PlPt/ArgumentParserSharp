using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArgumentParser;

namespace ArgumentParserTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ArgumentParser.ArgumentParser argument = new ArgumentParser.ArgumentParser(new Program());


            argument.Parse<string>(@"LUMI1/STATUS","LUMI_1_STATUSPACKAGE",new List<int>());
            Console.ReadLine();
            argument.Parse<string>(@"LUMI2/STATUS", "LUMI_2_STATUSPACKAGE", new List<int>());
            Console.ReadLine();
            argument.Parse<string>(@"MULTI/U", "2014,56");
            Console.ReadLine();
            argument.Parse<string>(@"MULTI/I", "1,234");


            Console.ReadLine();
        }

        [Command(Command = @"LUMI(\d)/STATUS", Description ="test method")]
        public void LumiStatus(int lumiId,string message,List<int> ll)
        {


            Console.WriteLine("Lumi: {0} -- {1}",lumiId,message);
        }

        [Command(Command = @"MULTI/U", Description = "test method")]
        public void MultiU(float message)
        {
            Console.WriteLine( message);
        }

        [Command(Command = @"MULTI/I", Description = "test method")]
        public void MultiI(float message)
        {
            Console.WriteLine(message);
        }
    }
}
