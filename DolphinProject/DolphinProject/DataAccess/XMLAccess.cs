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

            //on l'attache à un noeud
            XmlNode rootNode = doc.AppendChild(root);
            //on crée l'élément golfer
            XmlElement elt = doc.CreateElement("golfer");
            XmlNode cmdNode = rootNode.AppendChild(elt);

            XElement xml = new XElement("golfers",
                new XElement("golfer",
                new XAttribute("id", "1"),
                new XElement("name",
                new XElement("firstname", "Dan"))));
        }

        public Asset GetAsset(int id)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("AssetDb.xml");

            Asset asset = new Asset();
            asset.Id = id;
            asset.Label = doc.SelectSingleNode("/dolphin/asset[" + id + "]/@label").InnerText;
            asset.Type = doc.SelectSingleNode("/dolphin/asset[" + id + "]/@type").InnerText;
            asset.Nav = doc.SelectSingleNode("/dolphin/asset[" + id + "]/@nav").InnerText;
            asset.Sharpe = doc.SelectSingleNode("/dolphin/asset[" + id + "]/@sharpe").InnerText;
            asset.Currency = doc.SelectSingleNode("/dolphin/asset[" + id + "]/@currency").InnerText;

            return asset;
        }
    }
}
