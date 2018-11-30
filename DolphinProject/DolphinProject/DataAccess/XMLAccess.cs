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
    }
}
