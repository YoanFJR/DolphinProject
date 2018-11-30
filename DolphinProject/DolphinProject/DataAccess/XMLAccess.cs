using DolphinProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DolphinProject.DataAccess
{
    public class XMLAccess
    {
        public void LoadDocument()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("AssetDb.xml");

            XmlNode root = doc.FirstChild;

            XElement xml = new XElement("asset",
                new XElement("golfer",
                new XAttribute("id", "1"),
                new XElement("name",
                new XElement("firstname", "Dan"))));
        }

        public void AddAssets(List<Asset> assets)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("AssetDb.xml");
            XmlNode root = doc.FirstChild;

            foreach (Asset asset in assets)
            {
                //XElement label = new XElement("label", asset.Label);
                //XElement type = new XElement("type", asset.Type);
                //XElement Nav = new XElement("nav", asset.Nav);
                //XElement Sharpe = new XElement("sharpe", asset.Sharpe);
                //XElement Currency = new XElement("currency", asset.Currency);

                XNode Asset = new XElement("asset", new XElement("label", asset.Label),
                                                      new XElement("type", asset.Type),
                                                      new XElement("nav", asset.Nav),
                                                      new XElement("sharpe", asset.Sharpe),
                                                      new XElement("currency", asset.Currency));
                //root.AppendChild(Asset);
            }
        }

        public Asset GetAsset(int id)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("AssetDb.xml");

            Asset asset = new Asset();
            asset.Id = new Value() { value = "" + id };
            XmlNode node =  doc.SelectSingleNode("//asset[@id=" + id + "]");
            asset.Label = new Value() { value = node.SelectSingleNode("//label").InnerText };
            asset.Type = new Value() { value = node.SelectSingleNode("//type").InnerText };
            asset.Nav = new Value() { value = node.SelectSingleNode("//nav").InnerText };
            asset.Sharpe = new Value() { value = node.SelectSingleNode("//sharpe").InnerText };
            asset.Currency = new Value() { value = node.SelectSingleNode("//currency").InnerText };

            return asset;
        }
    }
}
