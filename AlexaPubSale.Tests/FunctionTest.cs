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
using System.IO;
using System.Net;



// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.


namespace AlexaPubSale.Tests
{
    public class FunctionTest
    {
        // [Fact]
        public static void Main()
        {
            SubSaleData.Root myDeserializedClass = JsonConvert.DeserializeObject<SubSaleData.Root>(SubInfo());

        var subonSale  =  myDeserializedClass.data.storeProductsSavingsSearchResult.storeProducts.Where(x => x.onSale == true);

            foreach (var item in subonSale) {

                Console.WriteLine(item.title+ ", " + item.shortDescription+ ", "+ item.savingLine + ", " + item.priceLine);
            }
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

            Console.WriteLine(httpResponse.StatusCode);


            return subdata;
        }

            // Console.Write("hello world");

           // dynamic data = JsonConvert.DeserializeObject(blob,typeof(object));



        }

    }
