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
        public double ComputePortfolioNav(Portfolio portfolio)
        {
            double totalNav = 0;
            foreach (Actif a in portfolio.Actifs)
            {
                XMLAccess xml = new XMLAccess();
                Asset asset = xml.GetAssets().FirstOrDefault(elt => elt.Id.value == a.Asset.ToString());
                totalNav += CurrencyConverter.value_exchange(asset.Currency.ToString(), Convert.ToDouble(asset.Nav) * a.Quantity);
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
                Console.WriteLine("%: " + CurrencyConverter.value_exchange(asset.Type.ToString(), Convert.ToDouble(asset.Nav)) * a.Quantity / totalNav * 100);
                if (CurrencyConverter.value_exchange(asset.Currency.ToString(), Convert.ToDouble(asset.Nav)) * a.Quantity / totalNav * 100 < 1 || CurrencyConverter.value_exchange(asset.Currency.ToString(), Convert.ToDouble(asset.Nav) * a.Quantity / totalNav * 100) > 10)
                {
                    Console.WriteLine("%: " + CurrencyConverter.value_exchange(asset.Type.ToString(), Convert.ToDouble(asset.Nav)) * a.Quantity / totalNav * 100);    
                    return false;
                }
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

        public Value GetFirstWrongAsset(Portfolio p)
        {
            Value id = new Value();
            XMLAccess xml = new XMLAccess();
            double totalNav = ComputePortfolioNav(p);

            foreach (Actif actif in p.Actifs)
            {
                Asset asset = xml.GetAsset(actif.Asset);
                if (CurrencyConverter.value_exchange(asset.Currency.ToString(), Convert.ToDouble(asset.Nav)) * a.Quantity / totalNav * 100 < 1 || CurrencyConverter.value_exchange(asset.Currency.ToString(), Convert.ToDouble(asset.Nav) * a.Quantity / totalNav * 100) > 10)
                {
                    id = asset.Id;
                    break;
                }
            }
            return id;
        }

        public Portfolio FixNavAsset(Portfolio p, Value id)
        {
            double targeted_nav = ComputePortfolioNav(p) * (5/100);
            XMLAccess xml = new XMLAccess();
            Asset asset = xml.GetAsset(Convert.ToInt32(id.value));

            double quantity = targeted_nav / asset.Nav;
            foreach (Actif actif in p)
            {
                if (actif.Asset == Convert.ToInt32(id.value))
                {
                    p.Actifs.Remove(actif);
                    p.Actifs.Add(new Actif()
                    {
                        Asset = actif.Asset,
                        Quantity = Convert.ToInt32(quantity)
                    });
                    break;
                }
            }

            return p;
        }

        public void upgradePortfolio (Portfolio p)
        {
            while (!CheckPortfolioNav(p))
            {
                Value wrong_id = GetFirstWrongAsset(p);
                p = FixNavAsset(p, wrong_id);
            }
            const string BASEURL = "https://dolphin.jump-technology.com:3472/api/v1/";
            APIAccess api = new APIAccess(BASEURL);

            api.PutPortfolio(p);
        }
    }
}
