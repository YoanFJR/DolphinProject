using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DolphinProject.Model
{
    public class Portfolio
    {
        public string Label { get; set; }
        public string Currency { get; set; }
        public string Type { get; set; }
        public List<Actif> Actifs { get; set; }

        public Portfolio()
        {
            Actifs = new List<Actif>();
            Label = "epita_ptf_2";
            Currency = "EUR";
            Type = "front";

        }

        public string Serialize()
        {
            if (Label == "" || Currency == "" || Type == "" || Actifs == null)
                return "";

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            {
                'label': '" + Label + @"',
                'currency': {
                    'code': '" + Currency + @"'
                },
                'type': 'front',
                'values': {
                    '2012-01-02' : [");
            foreach (Actif actif in Actifs)
            {
                sb.Append(
                    @"{
                        'asset': {
                            'asset':" + actif.Asset + @",
                            'quantity':" + actif.Quantity + @"
                        }
                    },"
                );
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(@"]
                }
            }");
            
            
            return sb.ToString().Replace("'", "\"");
        }

        public void Deserialize(string json)
        {
            Label = Regex.Match(json, "\"label\":\"[a-zA-Z0-9_]*\"").Value.Replace("\"","").Replace("label:", "");
            Currency = Regex.Match(json, "\"currency\":{\"code\":\"[A-Za-z]*\"}").Value.Replace("\"", "").Replace("currency:{code:", "").Replace("}","");
            Type = Regex.Match(json, "\"type\":\"[a-zA-Z]*\"").Value.Replace("\"", "").Replace("type:", "");
            foreach (Match match in Regex.Matches(json, "{\"asset\":{\"asset\":[0-9]*,\"quantity\":[0-9.]*}}"))
            {
                Actif actif = new Actif();
                actif.Asset = Convert.ToInt32(Regex.Match(match.Value, "\"asset\":[0-9]+").Value.Replace("\"asset\":", ""));
                actif.Quantity = (int)Convert.ToDouble(Regex.Match(match.Value, "\"quantity\":[0-9.]*").Value.Replace("\"quantity\":", "").Replace(".", ","));
                Actifs.Add(actif);
            }
        }
    }
}
