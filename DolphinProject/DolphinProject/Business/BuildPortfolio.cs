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
    }
}
