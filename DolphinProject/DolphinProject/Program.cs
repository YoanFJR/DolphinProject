using DolphinProject.DataAccess;
using DolphinProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinProject
{
    class Program
    {
        const string BASEURL = "https://dolphin.jump-technology.com:3472/api/v1/";

        static void Main(string[] args)
        {
            APIAccess api = new APIAccess(BASEURL);

            //Portfolio p = api.GetPortfolio();
            //p.Actifs.Clear();
            //p.Actifs.Add(new Actif()
            //{
            //    Asset = 814,
            //    Quantity = 1
            //});
            //p.Actifs.Add(new Actif()
            //{
            //    Asset = 609,
            //    Quantity = 1
            //});

            //api.PutPortfolio(p);

            Console.Write("Terminated...");
            Console.ReadKey();
        }
    }
}
