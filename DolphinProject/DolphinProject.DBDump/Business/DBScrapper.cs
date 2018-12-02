using DolphinProject.DataAccess;
using DolphinProject.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace DolphinProject.DBDump.Business
{
    public class DBScrapper
    {
        const string BASEURL = "https://dolphin.jump-technology.com:3472/api/v1/";
        private APIAccess _api;
        private BackgroundWorker _navWorker;
        private BackgroundWorker _sharpeWorker;
        private List<Asset> _navAssets;
        private List<Asset> _sharpeAssets;

        public DBScrapper()
        {
            _api = new APIAccess(BASEURL);
            _navAssets = new List<Asset>();
            _sharpeAssets = new List<Asset>();
            _navWorker = new BackgroundWorker();
            _sharpeWorker = new BackgroundWorker();
            _navWorker.WorkerReportsProgress = true;
            _navWorker.DoWork += _navWorker_DoWork;
            _navWorker.RunWorkerCompleted += _navWorker_RunWorkerCompleted;
            _navWorker.ProgressChanged += _navWorker_ProgressChanged;
            _sharpeWorker.DoWork += _sharpeWorker_DoWork;
            _sharpeWorker.RunWorkerCompleted += _sharpeWorker_RunWorkerCompleted;
        }

        #region Workers functions
        private void _sharpeWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _sharpeAssets = e.Result as List<Asset>;
            Console.WriteLine("[LOG] Getting sharpes of all assets");
        }
        private void _sharpeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<Asset> assets = e.Argument as List<Asset>;

            StringBuilder sbContent = new StringBuilder("{\"ratio\":[20],\"asset\":[");
            foreach (Asset asset in assets)
                sbContent.Append(asset.Id.value + ",");
            sbContent.Remove(sbContent.Length - 1, 1);
            sbContent.Append("]}");
            string sharpe = _api.Post("ratio/invoke", sbContent.ToString());
            var values = Regex.Matches(sharpe, "\"[0-9]*\":{[ \\-\n\"a-zA-Z:,0-9{]*");

            foreach (Match value in values)
            {
                string idAsset = value.Value.Substring(1, 4).Replace("\"", "");

                string val = Regex.Match(value.Value, "\"value\":\"[\\-0-9,]*\"").Value;
                val = val.Replace("\"", "").Replace("value:", "");

                Asset asset = assets.FirstOrDefault(a => a.Id.value == idAsset);
                asset.Sharpe = Convert.ToDouble(val);
            }

            e.Result = assets;
        }

        private void _navWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _navAssets = e.Result as List<Asset>;
            Console.WriteLine("[LOG] Getting nav of all assets");
        }
        private void _navWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<Asset> assets = e.Argument as List<Asset>;
            int count = 0;

            foreach (Asset asset in assets)
            {
                count++;
                string nav = _api.Get("asset/" + asset.Id.value + "/quote?start_date=2012-01-02&end_date=2012-01-02");

                if (nav != "[]")
                {
                    string value = Regex.Match(nav, "\"nav\":{[ \n\"a-zA-Z:,0-9]*}").Value;

                    value = Regex.Match(value, "\"value\":\"[0-9,]*\"").Value;
                    value = value.Replace("\"", "").Replace("value:", "");
                    asset.Nav = Convert.ToDouble(value);
                }
                _navWorker.ReportProgress(count * 100 / assets.Count);
            }

            e.Result = assets;
        }
        private void _navWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Console.WriteLine("[LOG] nav computed " + e.ProgressPercentage + "%");
        }
        #endregion

        private void GetNAVnDiscardNoNav(List<Asset> assets)
        {
            Console.WriteLine("[LOG] Getting nav...");
            int count = 1;
            foreach (Asset asset in assets)
            {
                string nav = _api.Get("asset/" + asset.Id.value + "/quote?start_date=2012-01-02&end_date=2012-01-02");

                if (nav != "[]")
                {
                    string value = Regex.Match(nav, "\"nav\":{[ \n\"a-zA-Z:,0-9]*}").Value;

                    value = Regex.Match(value, "\"value\":\"[0-9,]*\"").Value;
                    value = value.Replace("\"", "").Replace("value:", "");
                    asset.Nav = Convert.ToDouble(value);
                    Console.WriteLine("[LOG] Getting nav " + count + "/" + assets.Count);
                }
                else
                {
                    Console.WriteLine("[LOG] Skip nav " + count + "/" + assets.Count);
                }
                count++;
            }
            Console.WriteLine("[LOG] Getting nav done");
        }

        private List<Asset> GetResult()
        {
            List<Asset> res = new List<Asset>();
            int count = _navAssets.Count < _sharpeAssets.Count ? _navAssets.Count : _sharpeAssets.Count;
            for (int i = 0 ; i < count ; i++)
            {
                Asset asset = _navAssets.ElementAt(i);
                asset.Sharpe = _sharpeAssets.ElementAt(i).Sharpe;
                res.Add(asset);
            }
            return res;
        }

        public void Scrapp()
        {
            Console.WriteLine("[LOG] Scrapping db started...");
            string AssetsFromAPI = _api.Get("asset?columns=TYPE&columns=LABEL&columns=CURRENCY&columns=ASSET_DATABASE_ID&date=2012-01-02");
            List<Asset> assets = JsonConvert.DeserializeObject<List<Asset>>(AssetsFromAPI);
            Console.WriteLine("[LOG] Getting all assets done");

            Console.WriteLine("[LOG] Getting nav and sharpe...");
            _navWorker.RunWorkerAsync(assets);
            _sharpeWorker.RunWorkerAsync(assets);

            while (_navWorker.IsBusy || _sharpeWorker.IsBusy)
                Thread.Sleep(1000);

            Console.WriteLine("[LOG] Fusionning results...");
            List<Asset> result = GetResult();

            Console.WriteLine("[LOG] Adding asset in database...");
            new XMLAccess().AddAssets(result);
            Console.WriteLine("[LOG] Scrapping done");
        }
    }
}
