using DolphinProject.Model;
using DolphinProject.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinProject.Business
{
    class CheckValidity
    {
        public double ComputePortfolioNav(Portfolio portfolio)
        {
            double totalNav = 0;
            foreach (Actif a in portfolio.Actifs)
            {
                XMLAccess xml = new XMLAccess();
                Asset asset = xml.GetAsset(a.Asset);
                totalNav += Convert.ToDouble(asset.nav) * a.Quantity;
            }
            return totalNav;
        }

        public bool CheckPortfolioNav(Portfolio portfolio)
        {
            double totalNav = ComputePortfolioNav(portfolio);
            foreach (Actif a in portfolio.Actifs)
            {
                XMLAccess xml = new XMLAccess();
                Asset asset = xml.GetAsset(a.Asset);
                if (Convert.ToDouble(asset.nav) * a.Quantity / totalNav * 100 < 1 || Convert.ToDouble(asset.nav) * a.Quantity / totalNav * 100 > 10)
                    return false;
            }
            return true;
        }
    }
}
