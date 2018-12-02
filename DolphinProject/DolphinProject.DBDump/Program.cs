using DolphinProject.DBDump.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinProject.DBDump
{
    class Program
    {
        static void Main(string[] args)
        {
            DBScrapper dBScrapper = new DBScrapper();
            dBScrapper.Scrapp();

            Console.Write("Program terminated...");
            Console.ReadKey();
        }
    }
}
