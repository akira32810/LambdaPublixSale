using System;
using System.Collections.Generic;
using System.Text;

namespace AlexaPubSale
{
    public class SubSaleData
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Large
        {
            public string a { get; set; }
        }

        public class Small
        {
            public string a { get; set; }
        }

        public class ImageUrls
        {
            public Large large { get; set; }
            public Small small { get; set; }
        }

        public class StoreProduct
        {
            public string baseProductId { get; set; }
            public int itemCode { get; set; }
            public string title { get; set; }
            public string shortDescription { get; set; }
            public string advancedNotice { get; set; }
            public object sizeDescription { get; set; }
            public string savingLine { get; set; }
            public bool onSale { get; set; }
            public string priceLine { get; set; }
            public object specialPromotionDescription { get; set; }
            public string uiDisplayType { get; set; }
            public string activationStatus { get; set; }
            public object promoType { get; set; }
            public int rss { get; set; }
            public ImageUrls imageUrls { get; set; }
        }

        public class Value
        {
            public string ThisValue { get; set; }
            public int Count { get; set; }
            public string Name { get; set; }
            public string DisplayName { get; set; }
            public object MarketingImage { get; set; }
        }

        public class Facet
        {
            public string DisplayName { get; set; }
            public string Name { get; set; }
            public List<Value> Values { get; set; }
        }

        public class Category
        {
            public string Name { get; set; }
            public string FauxTaxonomy { get; set; }
            public string ID { get; set; }
            public int ProductCount { get; set; }
            public bool IsCurrentCategory { get; set; }
            public object ImageUrl { get; set; }
        }

        public class Parent
        {
            public string Name { get; set; }
            public string FauxTaxonomy { get; set; }
            public string ID { get; set; }
            public bool IsCurrentCategory { get; set; }
        }

        public class TopParent
        {
            public string Name { get; set; }
            public string FauxTaxonomy { get; set; }
            public string ID { get; set; }
            public bool IsCurrentCategory { get; set; }
        }

        public class StoreProductsSavingsSearchResult
        {
            public List<StoreProduct> storeProducts { get; set; }
            public int totalCount { get; set; }
            public List<Facet> facets { get; set; }
            public List<Category> categories { get; set; }
            public Parent parent { get; set; }
            public TopParent topParent { get; set; }
            public object correctedSearchTerm { get; set; }
        }

        public class Data
        {
            public StoreProductsSavingsSearchResult storeProductsSavingsSearchResult { get; set; }
        }

        public class Root
        {
            public Data data { get; set; }
        }


    }
}
