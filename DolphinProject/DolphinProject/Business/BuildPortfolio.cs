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
            return new XMLAccess().GetAssets().Where(a => a.Nav > 0).OrderByDescending(a => a.Sharpe).Take(quantity).ToList();
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
