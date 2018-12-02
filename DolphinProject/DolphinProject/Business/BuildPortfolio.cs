using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DolphinProject.Model;
using DolphinProject.DataAccess;

namespace DolphinProject.Business
{
    class BuildPortfolio
    {
        public List<Asset> GetBestSharp(int quantity)
        {
            List<Asset> result = new List<Asset>();

            List<Asset> assets = new List<Asset>();

            assets.Sort((a, b) => a.Sharpe < b.Sharpe ? 1 : -1);
            return result;
        }
    }
}
