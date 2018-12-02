﻿using DolphinProject.Model;
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
                totalNav += Convert.ToDouble(asset.Nav) * a.Quantity;
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
                //if (Convert.ToDouble(asset.Nav) * a.Quantity / totalNav * 100 < 1 || Convert.ToDouble(asset.Nav) * a.Quantity / totalNav * 100 > 10)
                //return false;
                if (CurrencyConverter.value_exchange(asset.Type.ToString(), Convert.ToDouble(asset.Nav)) * a.Quantity / totalNav * 100 < 1 || Convert.ToDouble(asset.Nav) * a.Quantity / totalNav * 100 > 10)
                    return false;
            }
            return true;
        }
    }
}
