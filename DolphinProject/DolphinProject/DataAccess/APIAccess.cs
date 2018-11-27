using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DolphinProject.DataAccess
{
    public class APIAccess
    {
        #region Constantes
        private const string USERNAME = "epita_user_2";
        private const string PASSWORD = "xeLLV8HzuV6urNNr";
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

        public HttpResponseMessage Get(string link)
        {
            return _client.GetAsync(link).Result;
        }
    }
}
