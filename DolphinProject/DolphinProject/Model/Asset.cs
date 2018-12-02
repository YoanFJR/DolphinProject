using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinProject.Model
{
    public class Asset
    {
        [JsonProperty(PropertyName = "ASSET_DATABASE_ID")]
        public Value Id { get; set; }

        [JsonProperty(PropertyName = "LABEL")]
        public Value Label { get; set; }

        [JsonProperty(PropertyName = "TYPE")]
        public Value Type { get; set; }

        [JsonProperty(PropertyName = "CURRENCY")]
        public Value Currency { get; set; }

        public double Nav { get; set; }

        public double Sharpe { get; set; }

        public List<Correlation> Correlations { get; set; }

        public Asset()
        {
            Correlations = new List<Correlation>();
        }
    }
}
