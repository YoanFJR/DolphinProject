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

        [JsonProperty(PropertyName = "nav")]
        public Value Nav { get; set; }

        [JsonProperty(PropertyName = "20")]
        public Value Sharpe { get; set; }

    }
}
