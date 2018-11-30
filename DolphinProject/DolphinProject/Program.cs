﻿using DolphinProject.DataAccess;
using DolphinProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinProject
{
    class Program
    {
        const string BASEURL = "https://dolphin.jump-technology.com:3472/api/v1/";

        static void Main(string[] args)
        {
            APIAccess api = new APIAccess(BASEURL);

            string res = api.Post("ratio/invoke", "{ \"ratio\":[20], \"asset\":[1029, 599] }");

            Console.Write("Terminated...");
            Console.ReadKey();
        }
    }
}
