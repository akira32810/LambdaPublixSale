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
using System.Net;
using System.IO;
using System.Text;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializerAttribute(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AlexaPubSale
{
    public class Function
    {

        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            SubSaleData.Root subsale = JsonConvert.DeserializeObject<SubSaleData.Root>(SubInfo());
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

                            var subsonsale = subsale.data.storeProductsSavingsSearchResult.storeProducts.Where(x => x.onSale == true);
                            StringBuilder sb = new StringBuilder();
                            if (subsonsale.Count() > 0)
                            {
                                foreach (var sub in subsonsale)
                                {

                                    sb.AppendLine("the sub on sale at Publix is " + sub.title + ". The recipes include " + sub.shortDescription + ".  The price " + sub.priceLine + " and " + sub.savingLine);


                                }
                                return ResponseBuilder.Tell(sb.ToString());
                            }

                            //  return ResponseBuilder.Ask(next, rp, session);
                            else
                            {
                                return ResponseBuilder.Tell("There are no sub on sale");
                            }
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

            var url = "https://services.publix.com/search/api/search/storeproductssavings/";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Headers["publixstore"] = "1158";
            httpRequest.ContentType = "application/json";

            var data = @"{
  ""operationName"": ""GetStoreProductsSavingsSearchResultAsync"",
  ""variables"": {
    ""take"": 30,
    ""skip"": 0,
    ""sortOrder"": ""featuredRank asc, titleSearch asc"",
    ""ispu"": true,
    ""categoryID"": ""2b88acd8-53a8-4092-b5ed-62746102e70b"",
    ""minMatch"": 0,
    ""boostVarIndex"": 0,
    ""spellCheckThreshold"": -1,
    ""wildcardSearch"": false,
    ""isPreviewSite"": false,
    ""pfVarIndex"": 0,
    ""qfVarIndex"": 0,
    ""getOrderHistory"": false
  },
  ""query"": ""query GetStoreProductsSavingsSearchResultAsync($keyword: String, $skip: Int!, $take: Int!, $facetOverrideStr: String, $facets: String, $sortOrder: String, $ispu: Boolean, $categoryID: String, $minMatch: Int!, $boostVarIndex: Int!, $spellCheckThreshold: Int!, $wildcardSearch: Boolean!, $isPreviewSite: Boolean!, $qfVarIndex: Int!, $pfVarIndex: Int!, $getOrderHistory: Boolean!) {\n  storeProductsSavingsSearchResult(\n    keyword: $keyword\n    skip: $skip\n    take: $take\n    facetOverrideStr: $facetOverrideStr\n    facets: $facets\n    sortOrder: $sortOrder\n    ispu: $ispu\n    categoryID: $categoryID\n    minMatch: $minMatch\n    boostVarIndex: $boostVarIndex\n    spellCheckThreshold: $spellCheckThreshold\n    wildcardSearch: $wildcardSearch\n    isPreviewSite: $isPreviewSite\n    pfVarIndex: $pfVarIndex\n    qfVarIndex: $qfVarIndex\n    getOrderHistory: $getOrderHistory\n  ) {\n    storeProducts {\n      baseProductId\n      itemCode\n      title\n      shortDescription\n      advancedNotice\n      sizeDescription\n      savingLine\n      onSale\n      priceLine\n      specialPromotionDescription\n      uiDisplayType\n      activationStatus\n      promoType\n      rss\n      imageUrls {\n        large {\n          a\n        }\n        small {\n          a\n        }\n      }\n    }\n    totalCount\n    facets {\n      DisplayName\n      Name\n      Values {\n        Value\n        Count\n        Name\n        DisplayName\n        MarketingImage\n      }\n    }\n    categories {\n      Name\n      FauxTaxonomy\n      ID\n      ProductCount\n      IsCurrentCategory\n      ImageUrl\n    }\n    parent {\n      Name\n      FauxTaxonomy\n      ID\n      IsCurrentCategory\n    }\n    topParent {\n      Name\n      FauxTaxonomy\n      ID\n      IsCurrentCategory\n    }\n    correctedSearchTerm\n  }\n}\n""
}";

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                subdata= streamReader.ReadToEnd();
            }

            // Console.WriteLine(httpResponse.StatusCode);



            return subdata;
        }


    }
}