using DolphinProject.DataAccess;
using DolphinProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DolphinProject.Business
{
    class BuildPortfolio
    {
        public List<Asset> GetBestSharpe(int quantity)
        {
            return new XMLAccess().GetAssets().Where(a => a.Nav > 0 && a.Type.value == "STOCK").OrderByDescending(a => a.Sharpe).Take(quantity).ToList();
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

                List<Correlation> correlations = a.Correlations.Where(elt => assets.FirstOrDefault(elt2 => elt2.Id.value == elt.AssetIdDest) != null).OrderBy(elt => Math.Abs(Convert.ToDouble(elt.Value))).Take(19).ToList();

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

        public Portfolio GetBestSharpPortofolio(APIAccess api, List<Portfolio> portfolios)
        {
            List<double> sharpes = new List<double>();
            int count = 1;


            foreach (Portfolio p in portfolios)
            {
                Portfolio elt = new CheckValidity().UpgradePortfolio(p);
                api.PutPortfolio(elt);

                string request = "{\"ratio\":[20],\"asset\":[1029]}";
                string sharpe = api.Post("ratio/invoke", request);

                Match value = Regex.Match(sharpe, "\"[0-9]*\":{[ \\-\n\"a-zA-Z:,0-9{]*");
                string idAsset = value.Value.Substring(1, 4).Replace("\"", "");

                string val = Regex.Match(value.Value, "\"value\":\"[\\-0-9,]*\"").Value;
                val = val.Replace("\"", "").Replace("value:", "");

                sharpes.Add(Convert.ToDouble(val));
                Console.WriteLine("[LOG] Compute Sharpe of portfolio " + count + "/" + portfolios.Count);
                count++;
            }

            return portfolios[sharpes.IndexOf(sharpes.Max())];
        }

        public Portfolio ComputeProtfolioWithCorrelations()
        {
            List<Portfolio> portfolios = GetPortfolios(GetBestSharpe(50));
            Dictionary<int, int> resultCorrelations = new Dictionary<int, int>();
            foreach(Portfolio p in portfolios)
            {
                foreach(Actif a in p.Actifs)
                {
                    if (resultCorrelations.ContainsKey(a.Asset))
                        resultCorrelations[a.Asset] = resultCorrelations[a.Asset] + 1;
                    else
                        resultCorrelations[a.Asset] = 1;
                }
            }

            Portfolio res = new Portfolio();
            foreach (KeyValuePair<int,int> pair in  resultCorrelations.ToList().OrderByDescending(elt => elt.Value).Take(20))
            {
                res.Actifs.Add(new Actif() { Asset = pair.Key, Quantity = 1 });
            }

            return res;
            //Portfolio p = GetBestSharpPortofolio(api, portfolios);
            //api.PutPortfolio(res);
        }

        public void SubmitBestPortolio(APIAccess api)
        {
            List<Portfolio> portfolios = GetPortfolios(GetBestSharpe(50));
            Portfolio p = GetBestSharpPortofolio(api, portfolios);
            api.PutPortfolio(p);
        }
    }
}
