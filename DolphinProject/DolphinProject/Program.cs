﻿using DolphinProject.DataAccess;
using DolphinProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DolphinProject.Business;

namespace DolphinProject
{
    class Program
    {
        const string BASEURL = "https://dolphin.jump-technology.com:3472/api/v1/";

        static void Main(string[] args)
        {
            APIAccess api = new APIAccess(BASEURL);

            BuildPortfolio buildPortfolio = new BuildPortfolio();
            List<Portfolio> portfolios = buildPortfolio.GetPortfolios(buildPortfolio.GetBestSharpe(50));
            Portfolio portfolio = buildPortfolio.GetBestSharpPortofolio(api, portfolios);
            //Portfolio portfolio = buildPortfolio.ComputeProtfolioWithCorrelations();
            //CheckValidity checkValidity = new CheckValidity();
            //Portfolio res = checkValidity.UpgradePortfolio(portfolio);

            api.PutPortfolio(portfolio);
            //List<Asset> test = buildPortfolio.GetBestSharpe(50);

            Console.Write("Terminated...");
            Console.ReadKey();
        }
    }
}
