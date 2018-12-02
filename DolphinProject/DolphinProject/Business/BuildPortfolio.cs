using DolphinProject.DataAccess;
using DolphinProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DolphinProject.Business
{
    class BuildPortfolio
    {
        public List<Asset> GetBestSharpe(int quantity)
        {
            List<Asset> result = new List<Asset>();
            List<Value> id_list = new List<Value>();

            XMLAccess XMLAccess = new XMLAccess();
            List<Asset> assets = XMLAccess.GetAssets();
            foreach (Asset elt in assets) {
                if (elt == null)
                    Console.WriteLine("Id: " + elt.Id);
                id_list.Add(elt.Id);
            }
            foreach (Value id in id_list)
            {
                Asset asset = XMLAccess.GetAsset(Convert.ToInt32(id.value));
                assets.Remove(asset);
            }
            
            assets = assets.Where(a => a.Nav > 0).OrderByDescending(a => a.Sharpe).Take(50).ToList();
            //assets.RemoveRange(assets.Count - 50, 50);
            return result;
        }

        public List<Portfolio> GetPortfolios(List<Asset> assets)
        {
            List<Portfolio> result = new List<Portfolio>();
            foreach (Asset a in assets)
            {
                Portfolio portfolio = new Portfolio();
                portfolio.Actifs.Add(new Actif()
                {
                    Asset = Convert.ToInt32(a.Id.value),
                    Quantity = 1
                });

                List<Correlation> correlations = a.Correlations.Where(elt => assets.FirstOrDefault(elt2 => elt2.Id.value == elt.AssetIdDest) != null).OrderByDescending(elt => elt.Value).Take(20).ToList();

                foreach (Correlation c in correlations)
                {
                    portfolio.Actifs.Add(new Actif()
                    {
                        Asset = Convert.ToInt32(c.AssetIdDest),
                        Quantity = 1
                    });
                }

                result.Add(portfolio);
            }
            return result;
        }
    }
}
