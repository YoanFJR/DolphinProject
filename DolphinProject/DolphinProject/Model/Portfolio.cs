using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinProject.Model
{
    public class Portfolio
    {
        public string Label { get; set; }
        public string Currency { get; set; }
        public string Type { get; set; }
        public List<Actif> Actifs { get; set; }

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
    }
}
