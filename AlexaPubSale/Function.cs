using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Alexa.NET.Response;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Newtonsoft.Json;
using Alexa.NET;
using System.Text.RegularExpressions;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializerAttribute(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AlexaPubSale
{
    public class Function
    {

        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
        {

            ILambdaLogger log = context.Logger;
            log.LogLine($"Skill Request Object:" + JsonConvert.SerializeObject(input));

            Session session = input.Session;
            if (session.Attributes == null)
                session.Attributes = new Dictionary<string, object>();

            Type requestType = input.GetRequestType();
            if (input.GetRequestType() == typeof(LaunchRequest))
            {
                string speech = "Welcome! Say pub sale of the week to get sub for sale and info";
                Reprompt rp = new Reprompt("Say pub sale of the week to start");
                return ResponseBuilder.Ask(speech, rp, session);
            }
            else if (input.GetRequestType() == typeof(SessionEndedRequest))
            {
                return ResponseBuilder.Tell("Goodbye!");
            }
            else if (input.GetRequestType() == typeof(IntentRequest))
            {
                var intentRequest = (IntentRequest)input.Request;
                switch (intentRequest.Intent.Name)
                {
                    case "AMAZON.CancelIntent":
                    case "AMAZON.StopIntent":
                        return ResponseBuilder.Tell("Goodbye!");
                    case "AMAZON.HelpIntent":
                        {
                            Reprompt rp = new Reprompt("What's next?");
                            return ResponseBuilder.Ask("Here's some help. What's next?", rp, session);
                        }
                    case "PubSubIntent":
                        {
                            dynamic data = JsonConvert.DeserializeObject(SubInfo(), typeof(object));
                            string sub_name = data.products.name;
                            string sub_recipe = data.products.sub_recipe;
                            if (!string.IsNullOrEmpty(sub_name))
                            {
                                return ResponseBuilder.Tell("the current sub on sale at Publix is " + sub_name + ". The recipe " + sub_recipe);
                            }

                            //  return ResponseBuilder.Ask(next, rp, session);
                            else
                                return ResponseBuilder.Tell("Sub on sale cannot be found");
                        }

                    default:
                        {
                            log.LogLine($"Unknown intent: " + intentRequest.Intent.Name);
                            string speech = "I didn't understand - try again?";
                            Reprompt rp = new Reprompt(speech);
                            return ResponseBuilder.Ask(speech, rp, session);
                        }
                }
            }
            return ResponseBuilder.Tell("Goodbye!");
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

                string rawstr = match.Value;

                subdata = System.Text.RegularExpressions.Regex.Unescape("{" + rawstr + "}").Replace("&#39;", "'").Replace("&amp;", "&").Trim();


            }


            return subdata;
        }


    }
}