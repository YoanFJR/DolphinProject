﻿using DolphinProject.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DolphinProject.DataAccess
{
    public class APIAccess
    {
        #region Constantes
        private const string USERNAME = "epita_user_2";
        private const string PASSWORD = "xeLLV8HzuV6urNNr";
        private const string ID_PORTFOLIO = "1029";
        private readonly string _baseUrl;
        private readonly HttpClient _client;
        #endregion

        #region Constructor
        public APIAccess(string baseUrl)
        {
            _baseUrl = baseUrl;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_baseUrl);
            var byteArray = Encoding.ASCII.GetBytes(USERNAME+':'+PASSWORD);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }
        #endregion

        public string Get(string link)
        {
            return _client.GetStringAsync(link).Result;
        }
        public async Task<string> GetAsync(string link)
        {
            return await _client.GetStringAsync(link);
        }
        public string Post(string link, string body)
        {
            HttpContent content = new StringContent(body, Encoding.UTF8, "application/json");

            var res = _client.PostAsync(link, content).Result;
            return res.IsSuccessStatusCode ? res.Content.ReadAsStringAsync().Result : "";
        }
        public async Task<string> PostAsync(string link, string body)
        {
            HttpContent content = new StringContent(body, Encoding.UTF8, "application/json");

            var res = await _client.PostAsync(link, content);
            return res.IsSuccessStatusCode ? await res.Content.ReadAsStringAsync() : "";
        }

        public bool PutPortfolio(Portfolio portfolio)
        {
            string p = portfolio.Serialize();
            HttpContent content = new StringContent(p, Encoding.UTF8, "application/json");

            var res = _client.PutAsync("portfolio/" + ID_PORTFOLIO + "/dyn_amount_compo", content).Result;
            return res.IsSuccessStatusCode;
        }
        public Portfolio GetPortfolio()
        {
            string res = _client.GetStringAsync("portfolio/" + ID_PORTFOLIO + "/dyn_amount_compo").Result;
            Portfolio portfolio = new Portfolio();
            portfolio.Deserialize(res);

            return portfolio;
        }

        public bool GetAssets()
        {

            for (int i = 597; i <= 1017; i++)
            {
                string res = _client.GetStringAsync("asset/" + i + "?columns=ASSET_DATABASE_ID&columns=TYPE&columns=LAST_CLOSE_VALUE_IN_CURR&columns=ASSET_DATABASE_ID&date=2012-01-01").Result;
                Asset asset = JsonConvert.DeserializeObject<Asset>(res);
            }
            return true;
            
        }
    }
}
