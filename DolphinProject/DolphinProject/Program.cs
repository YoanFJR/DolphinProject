using DolphinProject.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinProject
{
    class Program
    {
        const string BASEURL = "https://dolphin.jump-technology.com:3472/";

        static void Main(string[] args)
        {
            APIAccess api = new APIAccess(BASEURL);
            System.Net.Http.HttpResponseMessage test = api.GetTest();

            Console.ReadKey();
        }
    }
}
