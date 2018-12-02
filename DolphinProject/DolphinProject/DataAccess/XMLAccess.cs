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
        public void AddAssets(List<Asset> assets)
        {
            XDocument doc = XDocument.Load("AssetDb.xml");
            foreach (Asset asset in assets)
            {
                XElement Assets = new XElement("asset", new XAttribute("id", asset.Id.value), new XElement("label", asset.Label?.value),
                                                                                          new XElement("type", asset.Type?.value ?? "null"),
                                                                                          new XElement("nav", asset.Nav),
                                                                                          new XElement("sharpe", asset.Sharpe),
                                                                                          new XElement("currency", asset.Currency?.value ?? "null"));
                doc.Element("dolphin").Add(Assets);
            }
            doc.Save("AssetDb.xml");
        }

        public void UpdateCorrelation(List<Asset> assets)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("AssetDb.xml");

            foreach (Asset asset in assets)
            {
                XmlElement el = (XmlElement)doc.SelectSingleNode("//asset[@id=" + asset.Id.value + "]");
                if (el != null)
                {
                    XmlElement correlations = doc.CreateElement("correlations");

                    foreach (Correlation cor in asset.Correlations)
                    {
                        XmlElement correlation = doc.CreateElement("correlation");
                        XmlElement dest = doc.CreateElement("dest");
                        XmlElement value = doc.CreateElement("value");
                        dest.InnerText = cor.AssetIdDest;
                        value.InnerText = cor.Value;
                        correlation.AppendChild(dest);
                        correlation.AppendChild(value);
                        correlations.AppendChild(correlation);
                    }
                    el.AppendChild(correlations);
                }
            }
            
            doc.Save("AssetDb.xml");
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
            asset.Nav = Convert.ToDouble(node.SelectSingleNode("//nav").InnerText);
            asset.Sharpe = Convert.ToDouble(node.SelectSingleNode("//sharpe").InnerText);
            asset.Currency = new Value() { value = node.SelectSingleNode("//currency").InnerText };

            return asset;
        }

        public List<Asset> GetAssetsId()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("AssetDb.xml");

            List<Asset> assets = new List<Asset>();

            foreach (XmlNode assetNode in doc.SelectNodes("//asset"))
            {
                assets.Add(new Asset()
                {
                    Id = new Value() { value = assetNode.Attributes.GetNamedItem("id").InnerText }
                });
            }

            return assets;
        }
    }
}
