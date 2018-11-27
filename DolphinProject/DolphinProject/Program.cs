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

            Portfolio portfolio = new Portfolio()
            {
                Label = "epita_ptf_2",
                Type = "front",
                Currency = "EUR",
                Actifs = new List<Actif>()
                {
                    new Actif() { Asset = 599, Quantity = 1 }
                }
            };

            bool res = api.PutPortfolio(portfolio);
            string test = api.GetPortfolio();

            Console.ReadKey();
        }
    }
}
