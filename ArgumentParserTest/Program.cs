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
            Console.WriteLine("PS: " +  argument.Parse<string>("test Hallo Welt;13"));

            Console.ReadLine();
        }

        [Command(Command ="test (.*);(\\d+)",Description ="test method")]
        public string Test(string input,int i)
        {
            return input + ": " + i;
        }
    }
}
