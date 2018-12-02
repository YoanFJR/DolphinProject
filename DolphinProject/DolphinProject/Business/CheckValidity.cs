using DolphinProject.Model;
using DolphinProject.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DolphinProject.Business;

namespace DolphinProject.Business
{
    class CheckValidity
    {
        public double ComputePortfolioNav(Portfolio portfolio, List<Asset> assets)
        {
            double totalNav = 0;
            foreach (Actif a in portfolio.Actifs)
            {
                Asset asset = assets.FirstOrDefault(elt => elt.Id.value == a.Asset.ToString());
                totalNav += CurrencyConverter.value_exchange(asset.Currency.ToString(), Convert.ToDouble(asset.Nav) * a.Quantity);
            }
            return totalNav;
        }

        public bool CheckPortfolioNav(Portfolio portfolio)
        {
            double totalNav = ComputePortfolioNav(portfolio, new XMLAccess().GetAssets());
            foreach (Actif a in portfolio.Actifs)
            {
                XMLAccess xml = new XMLAccess();
                Asset asset = xml.GetAsset(a.Asset);

                if (CurrencyConverter.value_exchange(asset.Currency.ToString(), Convert.ToDouble(asset.Nav)) * a.Quantity / totalNav * 100 < 1 || CurrencyConverter.value_exchange(asset.Currency.ToString(), Convert.ToDouble(asset.Nav) * a.Quantity / totalNav * 100) > 10)
                    return false;
            }
            return true;
        }

        public bool CheckPortfolioCompo(List<Asset> assets)
        {
            int stock = 0;
            foreach (Asset a in assets)
                if (a.Type.value == "STOCK")
                    stock++;
            return stock >= 10;
        }

        public Portfolio UpgradePortfolio (Portfolio p)
        {
            XMLAccess xMLAccess = new XMLAccess();
            List<Asset> assets = new List<Asset>();
            foreach(Actif a in p.Actifs)
                assets.Add(xMLAccess.GetAsset(a.Asset));

            if (true)//!CheckPortfolioNav(p))
            {
                double totalnav = ComputePortfolioNav(p, assets) * 100;
                double target_nav = totalnav * 0.05;

                foreach(Actif actif in p.Actifs)
                {
                    Asset asset = assets.FirstOrDefault(a => a.Id.value == actif.Asset.ToString());
                    double nav_eur = CurrencyConverter.value_exchange(asset.Type.value, asset.Nav);
                    actif.Quantity = (int)(target_nav / nav_eur);
                }
            }

            return p;
        }
    }
}
