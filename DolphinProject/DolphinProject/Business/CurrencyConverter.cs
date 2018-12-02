using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolphinProject.Business
{
    public class CurrencyConverter
    {
        //Convert the value related to old currency to new value in EUR.
        public static double value_exchange (string old_curr, double old_val)
        {
            double new_val = 0;

            switch (old_curr)
            {
                case "USD":
                    new_val = old_val * 0.773096;
                    break;
                case "GBP":
                    new_val = old_val * 1.197404;
                    break;
                case "JPY":
                    new_val = old_val * 0.010048;
                    break;
                case "NOK":
                    new_val = old_val * 0.128966;
                    break;
                case "SEK":
                    new_val = old_val * 0.112013;
                    break;
                case "CHF":
                    new_val = old_val * 0.822707;
                    break;
                default:
                    new_val = old_val;
                    break;
            }
            return new_val;
        }
    }
}
