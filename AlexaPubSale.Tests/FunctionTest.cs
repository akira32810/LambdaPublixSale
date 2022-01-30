using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using AlexaPubSale;
using Alexa.NET;
using Alexa.NET.Response;

using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Newtonsoft.Json;


using System.Text.RegularExpressions;
using System.Net.Http;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.


namespace AlexaPubSale.Tests
{
    public class FunctionTest
    {
        // [Fact]
        public static void Main()
        {
            Console.WriteLine(SubInfo());
        }


        private static string SubInfo()
        {
            string subdata = string.Empty;


            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                string html = client.DownloadString("https://www.publix.com/pd/BMO-DSB-100369");


                string regexstr = "\"products\":[^}]*}";
                Regex regex = new Regex(regexstr, RegexOptions.Singleline);

                Match match = regex.Match(html);


                //int first = html.IndexOf("\"products\":") + "\"products\"".Length;

                //int last = html.LastIndexOf("}");
                //string str2 = html.Substring(first, last - first+1);
                string rawstr = match.Value;

               subdata = System.Text.RegularExpressions.Regex.Unescape("{"+rawstr+"}").Replace("&#39;","'").Replace("&amp;","&").Trim();

              

            }

            // Console.Write("hello world");

           // dynamic data = JsonConvert.DeserializeObject(blob,typeof(object));



            return subdata;
        }

    }
}