using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WineScraper.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Price_Vintage
    {
        public string id { get; set; }
        public string year { get; set; }
        public object grapes { get; set; }
        public bool has_valid_ratings { get; set; }
    }

    public class Price_Median
    {
        public string amount { get; set; }
        public string type { get; set; }
        public string discounted_from { get; set; }
    }

    public class Price_Price
    {
        public string id { get; set; }
        public string amount { get; set; }
        public string discounted_from { get; set; }
        public string type { get; set; }
    }

    public class Price_Root
    {
        public Price_Vintage vintage { get; set; }
        public Price_Median median { get; set; }
        public Price_Price price { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Currency
    {
        public string code { get; set; }
        public string name { get; set; }
        public string prefix { get; set; }
        public object suffix { get; set; }
    }

    public class Market
    {
        public string country { get; set; }
        public Currency currency { get; set; }
    }

    public class Statistics
    {
        public string status { get; set; }
        public string ratings_count { get; set; }
        public string ratings_average { get; set; }
        public string labels_count { get; set; }
    }

    public class Variations
    {
        public string bottle_large { get; set; }
        public string bottle_medium { get; set; }
        public string bottle_medium_square { get; set; }
        public string bottle_small { get; set; }
        public string bottle_small_square { get; set; }
        public string label { get; set; }
        public string label_large { get; set; }
        public string label_medium { get; set; }
        public string label_medium_square { get; set; }
        public string label_small_square { get; set; }
        public string large { get; set; }
        public string medium { get; set; }
        public string medium_square { get; set; }
        public string small_square { get; set; }
    }

    public class Image
    {
        public string location { get; set; }
        public Variations variations { get; set; }
    }

    public class Currency2
    {
        public string code { get; set; }
        public string name { get; set; }
        public string prefix { get; set; }
        public object suffix { get; set; }
    }

    public class MostUsedGrape
    {
        public string id { get; set; }
        public string name { get; set; }
        public string seo_name { get; set; }
        public bool has_detailed_info { get; set; }
        public string wines_count { get; set; }
    }

    public class Country
    {
        public string code { get; set; }
        public string name { get; set; }
        public string native_name { get; set; }
        public string seo_name { get; set; }
        public Currency2 currency { get; set; }
        public string regions_count { get; set; }
        public string users_count { get; set; }
        public string wines_count { get; set; }
        public string wineries_count { get; set; }
        public List<MostUsedGrape> most_used_grapes { get; set; } = new List<MostUsedGrape>();
    }

    public class BackgroundImage
    {
    }

    public class Class2
    {
    }

    public class TypecastMap
    {
        public BackgroundImage background_image { get; set; }
        public Class2 @class { get; set; }
    }

    public class Class
    {
        public TypecastMap typecast_map { get; set; }
    }

    public class Variations2
    {
        public string large { get; set; }
        public string medium { get; set; }
    }

    public class BackgroundImage2
    {
        public string location { get; set; }
        public Variations2 variations { get; set; }
    }

    public class Region
    {
        public string id { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string seo_name { get; set; }
        public Country country { get; set; }
        public Class @class { get; set; }
        public BackgroundImage2 background_image { get; set; }
    }

    public class Winery
    {
        public string id { get; set; }
        public string name { get; set; }
        public string seo_name { get; set; }
        public string status { get; set; }
    }

    public class Structure
    {
        public string acidity { get; set; }
        public string fizziness { get; set; }
        public string intensity { get; set; }
        public string sweetness { get; set; }
        public string tannin { get; set; }
        public string user_structure_count { get; set; }
        public string calculated_structure_count { get; set; }
    }

    public class Stats
    {
        public string count { get; set; }
        public string score { get; set; }
    }

    public class Vintage_Wine_Taste_Flavor
    {
        public string vintage_id { get; set; }
        public string vintage_wine_id { get; set; }
        public string group { get; set; }
        public string stats_count { get; set; }
        public string stats_score { get; set; }
    }

    public class Flavor
    {
        public string group { get; set; }
        public Stats stats { get; set; }
    }

    public class Taste
    {
        public Structure structure { get; set; } = new Structure();
        public List<Flavor> flavor { get; set; } = new List<Flavor>();
    }

    public class Statistics2
    {
        public string status { get; set; }
        public string ratings_count { get; set; }
        public string ratings_average { get; set; }
        public string labels_count { get; set; }
        public string vintages_count { get; set; }
    }

    public class Variations3
    {
        public string small { get; set; }
    }

    public class BackgroundImage3
    {
        public string location { get; set; }
        public Variations3 variations { get; set; }
    }

    public class Currency3
    {
        public string code { get; set; }
        public string name { get; set; }
        public string prefix { get; set; }
        public object suffix { get; set; }
    }

    public class MostUsedGrape2
    {
        public string id { get; set; }
        public string name { get; set; }
        public string seo_name { get; set; }
        public bool has_detailed_info { get; set; }
        public string wines_count { get; set; }
    }


    public class Vintage_Wine_Style_Country_MostUsedGrape : MostUsedGrape
    {
        public string vintage_id { get; set; }
        public string wine_id { get; set; }
        public string style_id { get; set; }
    }

    public class Vintage_Wine_Region_Country_MostUsedGrape : MostUsedGrape
    {
        public string vintage_id { get; set; }
        public string wine_id { get; set; }
        public string region_id { get; set; }
    }


    public class Country2
    {
        public string code { get; set; }
        public string name { get; set; }
        public string native_name { get; set; }
        public string seo_name { get; set; }
        public Currency3 currency { get; set; }
        public string regions_count { get; set; }
        public string users_count { get; set; }
        public string wines_count { get; set; }
        public string wineries_count { get; set; }
        public List<MostUsedGrape2> most_used_grapes { get; set; }
    }

    public class Variations4
    {
        public string small { get; set; }
    }

    public class BackgroundImage4
    {
        public string location { get; set; }
        public Variations4 variations { get; set; }
    }

    public class Food
    {
        public string id { get; set; }
        public string name { get; set; }
        public BackgroundImage4 background_image { get; set; }
        public string seo_name { get; set; }
    }
    public class Vintage_Wine_Style_Food
    {
        public string vintage_id { get; set; }
        public string wine_id { get; set; }
        public string style_id { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string seo_name { get; set; }
    }

    public class Grape
    {
        public string id { get; set; }
        public string name { get; set; }
        public string seo_name { get; set; }
        public bool has_detailed_info { get; set; }
        public string wines_count { get; set; }
    }

    public class Vintage_Wine_Style_Grapes
    {
        public string vintage_id { get; set; }
        public string wine_id { get; set; }
        public string style_id { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string seo_name { get; set; }
        public bool has_detailed_info { get; set; }
        public string wines_count { get; set; }
    }
    public class Currency4
    {
        public string code { get; set; }
        public string name { get; set; }
        public string prefix { get; set; }
        public object suffix { get; set; }
    }

    public class MostUsedGrape3
    {
        public string id { get; set; }
        public string name { get; set; }
        public string seo_name { get; set; }
        public bool has_detailed_info { get; set; }
        public string wines_count { get; set; }
    }

    public class Country3
    {
        public string code { get; set; }
        public string name { get; set; }
        public string native_name { get; set; }
        public string seo_name { get; set; }
        public Currency4 currency { get; set; }
        public string regions_count { get; set; }
        public string users_count { get; set; }
        public string wines_count { get; set; }
        public string wineries_count { get; set; }
        public List<MostUsedGrape3> most_used_grapes { get; set; }
    }

    public class Variations5
    {
        public string large { get; set; }
        public string medium { get; set; }
    }

    public class BackgroundImage5
    {
        public string location { get; set; }
        public Variations5 variations { get; set; }
    }

    public class BackgroundImage6
    {
    }

    public class Class4
    {
    }

    public class TypecastMap2
    {
        public BackgroundImage6 background_image { get; set; }
        public Class4 @class { get; set; }
    }

    public class Class3
    {
        public TypecastMap2 typecast_map { get; set; }
    }

    public class Region2
    {
        public string id { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string seo_name { get; set; }
        public Country3 country { get; set; }
        public BackgroundImage5 background_image { get; set; }
        public Class3 @class { get; set; }
    }

    public class Style
    {
        public string id { get; set; }
        public string seo_name { get; set; }
        public string regional_name { get; set; }
        public string varietal_name { get; set; }
        public string name { get; set; }
        public object image { get; set; }
        public BackgroundImage3 background_image { get; set; }
        public string description { get; set; }
        public string blurb { get; set; }
        public List<string> interesting_facts { get; set; } = new List<string>();
        public string body { get; set; }
        public string body_description { get; set; }
        public string acidity { get; set; }
        public string acidity_description { get; set; }
        public Country2 country { get; set; }
        public string wine_type_id { get; set; }
        public List<Food> food { get; set; }
        public List<Grape> grapes { get; set; }
        public Region2 region { get; set; }
    }

    public class Wine
    {
        public string id { get; set; }
        public string name { get; set; }
        public string seo_name { get; set; }
        public string type_id { get; set; }
        public string vintage_type { get; set; }
        public bool is_natural { get; set; }
        public Region region { get; set; }
        public Winery winery { get; set; }
        public Taste taste { get; set; }
        public Statistics2 statistics { get; set; }
        public Style style { get; set; }
        public bool has_valid_ratings { get; set; }
    }

    public class TopList
    {
        public string id { get; set; }
        public string location { get; set; }
        public string name { get; set; }
        public string seo_name { get; set; }
        public string type { get; set; }
        public string year { get; set; }
    }

    public class TopListRanking
    {
        public string rank { get; set; }
        public string previous_rank { get; set; }
        public object description { get; set; }
        public TopList top_list { get; set; }
    }

    public class Vintage_TopListRanking
    {
        public string vintage_id { get; set; }
        public string vintage_topListRanking_rank { get; set; }
        public string vintage_topListRanking_previous_rank { get; set; }
        public object vintage_topListRanking_description { get; set; }
        public string vintage_topListRanking_top_list_id { get; set; }
        public string vintage_topListRanking_top_list_location { get; set; }
        public string vintage_topListRanking_top_list_name { get; set; }
        public string vintage_topListRanking_top_list_seo_name { get; set; }
        public string vintage_topListRanking_top_list_type { get; set; }
        public string vintage_topListRanking_top_list_year { get; set; }
    }

    public class Vintage
    {
        public string id { get; set; }
        public string seo_name { get; set; }
        public string name { get; set; }
        public Statistics statistics { get; set; }
        public Image image { get; set; }
        public Wine wine { get; set; }
        public List<TopListRanking> top_list_rankings { get; set; } = new List<TopListRanking>();
        public string year { get; set; }
        public object grapes { get; set; }
        public bool has_valid_ratings { get; set; }
    }

    public class Currency5
    {
        public string code { get; set; }
        public string name { get; set; }
        public string prefix { get; set; }
        public object suffix { get; set; }
    }

    public class BottleType
    {
        public string id { get; set; }
        public string name { get; set; }
        public string short_name { get; set; }
        public string short_name_plural { get; set; }
        public string volume_ml { get; set; }
    }

    public class Price
    {
        public string id { get; set; }
        public string amount { get; set; }
        public string discounted_from { get; set; }
        public string discount_percent { get; set; }
        public string type { get; set; }
        public string sku { get; set; }
        public string url { get; set; }
        public string visibility { get; set; }
        public string bottle_type_id { get; set; }
        public string bottle_quantity { get; set; }
        public Currency5 currency { get; set; }
        public BottleType bottle_type { get; set; }
    }

    public class Currency6
    {
        public string code { get; set; }
        public string name { get; set; }
        public string prefix { get; set; }
        public object suffix { get; set; }
    }

    public class BottleType2
    {
        public string id { get; set; }
        public string name { get; set; }
        public string short_name { get; set; }
        public string short_name_plural { get; set; }
        public string volume_ml { get; set; }
    }

    public class Price2
    {
        public string id { get; set; }
        public string amount { get; set; }
        public string discounted_from { get; set; }
        public string discount_percent { get; set; }
        public string type { get; set; }
        public string sku { get; set; }
        public string url { get; set; }
        public string visibility { get; set; }
        public string bottle_type_id { get; set; }
        public string bottle_quantity { get; set; }
        public Currency6 currency { get; set; }
        public BottleType2 bottle_type { get; set; }
    }

    public class Match
    {
        public Vintage vintage { get; set; }
        public Price price { get; set; }
        public List<Price2> prices { get; set; }
    }

    public class Statistics3
    {
        public string status { get; set; }
        public string ratings_count { get; set; }
        public string ratings_average { get; set; }
        public string labels_count { get; set; }
    }

    public class Variations6
    {
        public string bottle_large { get; set; }
        public string bottle_medium { get; set; }
        public string bottle_medium_square { get; set; }
        public string bottle_small { get; set; }
        public string bottle_small_square { get; set; }
        public string label { get; set; }
        public string label_large { get; set; }
        public string label_medium { get; set; }
        public string label_medium_square { get; set; }
        public string label_small_square { get; set; }
        public string large { get; set; }
        public string medium { get; set; }
        public string medium_square { get; set; }
        public string small_square { get; set; }
    }

    public class Image2
    {
        public string location { get; set; }
        public Variations6 variations { get; set; }
    }

    public class Currency7
    {
        public string code { get; set; }
        public string name { get; set; }
        public string prefix { get; set; }
        public object suffix { get; set; }
    }

    public class MostUsedGrape4
    {
        public string id { get; set; }
        public string name { get; set; }
        public string seo_name { get; set; }
        public bool has_detailed_info { get; set; }
        public string wines_count { get; set; }
    }

    public class Country4
    {
        public string code { get; set; }
        public string name { get; set; }
        public string native_name { get; set; }
        public string seo_name { get; set; }
        public Currency7 currency { get; set; }
        public string regions_count { get; set; }
        public string users_count { get; set; }
        public string wines_count { get; set; }
        public string wineries_count { get; set; }
        public List<MostUsedGrape4> most_used_grapes { get; set; }
    }

    public class BackgroundImage7
    {
    }

    public class Class6
    {
    }

    public class TypecastMap3
    {
        public BackgroundImage7 background_image { get; set; }
        public Class6 @class { get; set; }
    }

    public class Class5
    {
        public TypecastMap3 typecast_map { get; set; }
    }

    public class Variations7
    {
        public string large { get; set; }
        public string medium { get; set; }
    }

    public class BackgroundImage8
    {
        public string location { get; set; }
        public Variations7 variations { get; set; }
    }

    public class Region3
    {
        public string id { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string seo_name { get; set; }
        public Country4 country { get; set; }
        public Class5 @class { get; set; }
        public BackgroundImage8 background_image { get; set; }
    }

    public class Winery2
    {
        public string id { get; set; }
        public string name { get; set; }
        public string seo_name { get; set; }
        public string status { get; set; }
    }

    public class Structure2
    {
        public string acidity { get; set; }
        public object fizziness { get; set; }
        public string intensity { get; set; }
        public string sweetness { get; set; }
        public string tannin { get; set; }
        public string user_structure_count { get; set; }
        public string calculated_structure_count { get; set; }
    }

    public class Stats2
    {
        public string count { get; set; }
        public string score { get; set; }
    }

    public class Flavor2
    {
        public string group { get; set; }
        public Stats2 stats { get; set; }
    }

    public class Taste2
    {
        public Structure2 structure { get; set; }
        public List<Flavor2> flavor { get; set; }
    }

    public class Statistics4
    {
        public string status { get; set; }
        public string ratings_count { get; set; }
        public string ratings_average { get; set; }
        public string labels_count { get; set; }
        public string vintages_count { get; set; }
    }

    public class Variations8
    {
        public string small { get; set; }
    }

    public class BackgroundImage9
    {
        public string location { get; set; }
        public Variations8 variations { get; set; }
    }

    public class Currency8
    {
        public string code { get; set; }
        public string name { get; set; }
        public string prefix { get; set; }
        public object suffix { get; set; }
    }

    public class MostUsedGrape5
    {
        public string id { get; set; }
        public string name { get; set; }
        public string seo_name { get; set; }
        public bool has_detailed_info { get; set; }
        public string wines_count { get; set; }
    }

    public class Country5
    {
        public string code { get; set; }
        public string name { get; set; }
        public string native_name { get; set; }
        public string seo_name { get; set; }
        public Currency8 currency { get; set; }
        public string regions_count { get; set; }
        public string users_count { get; set; }
        public string wines_count { get; set; }
        public string wineries_count { get; set; }
        public List<MostUsedGrape5> most_used_grapes { get; set; }
    }

    public class Variations9
    {
        public string small { get; set; }
    }

    public class BackgroundImage10
    {
        public string location { get; set; }
        public Variations9 variations { get; set; }
    }

    public class Food2
    {
        public string id { get; set; }
        public string name { get; set; }
        public BackgroundImage10 background_image { get; set; }
        public string seo_name { get; set; }
    }

    public class Grape2
    {
        public string id { get; set; }
        public string name { get; set; }
        public string seo_name { get; set; }
        public bool has_detailed_info { get; set; }
        public string wines_count { get; set; }
    }

    public class Currency9
    {
        public string code { get; set; }
        public string name { get; set; }
        public string prefix { get; set; }
        public object suffix { get; set; }
    }

    public class MostUsedGrape6
    {
        public string id { get; set; }
        public string name { get; set; }
        public string seo_name { get; set; }
        public bool has_detailed_info { get; set; }
        public string wines_count { get; set; }
    }

    public class Country6
    {
        public string code { get; set; }
        public string name { get; set; }
        public string native_name { get; set; }
        public string seo_name { get; set; }
        public Currency9 currency { get; set; }
        public string regions_count { get; set; }
        public string users_count { get; set; }
        public string wines_count { get; set; }
        public string wineries_count { get; set; }
        public List<MostUsedGrape6> most_used_grapes { get; set; }
    }

    public class Variations10
    {
        public string large { get; set; }
        public string medium { get; set; }
    }

    public class BackgroundImage11
    {
        public string location { get; set; }
        public Variations10 variations { get; set; }
    }

    public class BackgroundImage12
    {
    }

    public class Class8
    {
    }

    public class TypecastMap4
    {
        public BackgroundImage12 background_image { get; set; }
        public Class8 @class { get; set; }
    }

    public class Class7
    {
        public TypecastMap4 typecast_map { get; set; }
    }

    public class Region4
    {
        public string id { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public string seo_name { get; set; }
        public Country6 country { get; set; }
        public BackgroundImage11 background_image { get; set; }
        public Class7 @class { get; set; }
    }

    public class Style2
    {
        public string id { get; set; }
        public string seo_name { get; set; }
        public string regional_name { get; set; }
        public string varietal_name { get; set; }
        public string name { get; set; }
        public object image { get; set; }
        public BackgroundImage9 background_image { get; set; }
        public string description { get; set; }
        public string blurb { get; set; }
        public List<string> interesting_facts { get; set; }
        public string body { get; set; }
        public string body_description { get; set; }
        public string acidity { get; set; }
        public string acidity_description { get; set; }
        public Country5 country { get; set; }
        public string wine_type_id { get; set; }
        public List<Food2> food { get; set; }
        public List<Grape2> grapes { get; set; }
        public Region4 region { get; set; }
    }

    public class Wine2
    {
        public string id { get; set; }
        public string name { get; set; }
        public string seo_name { get; set; }
        public string type_id { get; set; }
        public string vintage_type { get; set; }
        public bool is_natural { get; set; }
        public Region3 region { get; set; }
        public Winery2 winery { get; set; }
        public Taste2 taste { get; set; }
        public Statistics4 statistics { get; set; }
        public Style2 style { get; set; }
        public bool has_valid_ratings { get; set; }
    }

    public class TopList2
    {
        public string id { get; set; }
        public string location { get; set; }
        public string name { get; set; }
        public string seo_name { get; set; }
        public string type { get; set; }
        public string year { get; set; }
    }

    public class TopListRanking2
    {
        public string rank { get; set; }
        public string previous_rank { get; set; }
        public object description { get; set; }
        public TopList2 top_list { get; set; }
    }

    public class Vintage2
    {
        public string id { get; set; }
        public string seo_name { get; set; }
        public string name { get; set; }
        public Statistics3 statistics { get; set; }
        public Image2 image { get; set; }
        public Wine2 wine { get; set; }
        public List<TopListRanking2> top_list_rankings { get; set; }
        public object year { get; set; }
        public object grapes { get; set; }
        public bool has_valid_ratings { get; set; }
    }

    public class Currency10
    {
        public string code { get; set; }
        public string name { get; set; }
        public string prefix { get; set; }
        public object suffix { get; set; }
    }

    public class BottleType3
    {
        public string id { get; set; }
        public string name { get; set; }
        public string short_name { get; set; }
        public string short_name_plural { get; set; }
        public string volume_ml { get; set; }
    }

    public class Price3
    {
        public string id { get; set; }
        public string amount { get; set; }
        public string discounted_from { get; set; }
        public string discount_percent { get; set; }
        public string type { get; set; }
        public string sku { get; set; }
        public string url { get; set; }
        public string visibility { get; set; }
        public string bottle_type_id { get; set; }
        public string bottle_quantity { get; set; }
        public Currency10 currency { get; set; }
        public BottleType3 bottle_type { get; set; }
    }

    public class Currency11
    {
        public string code { get; set; }
        public string name { get; set; }
        public string prefix { get; set; }
        public object suffix { get; set; }
    }

    public class BottleType4
    {
        public string id { get; set; }
        public string name { get; set; }
        public string short_name { get; set; }
        public string short_name_plural { get; set; }
        public string volume_ml { get; set; }
    }

    public class Price4
    {
        public string id { get; set; }
        public string amount { get; set; }
        public string discounted_from { get; set; }
        public string discount_percent { get; set; }
        public string type { get; set; }
        public string sku { get; set; }
        public string url { get; set; }
        public string visibility { get; set; }
        public string bottle_type_id { get; set; }
        public string bottle_quantity { get; set; }
        public Currency11 currency { get; set; }
        public BottleType4 bottle_type { get; set; }
    }

    public class Record
    {
        public Vintage2 vintage { get; set; }
        public Price3 price { get; set; }
        public List<Price4> prices { get; set; }
    }

    public class ExploreVintage
    {
        public Market market { get; set; }
        public string records_matched { get; set; }
        public List<Match> matches { get; set; }
        public List<object> bottle_type_errors { get; set; }
        public List<Record> records { get; set; }
    }

    public class Variations11
    {
        public string small { get; set; }
    }

    public class BackgroundImage13
    {
        public string location { get; set; }
        public Variations11 variations { get; set; }
    }

    public class Item
    {
        public string id { get; set; }
        public string name { get; set; }
        public BackgroundImage13 background_image { get; set; }
    }

    public class SelectedFilter
    {
        public string type { get; set; }
        public List<Item> items { get; set; }
    }

    public class Root
    {
        public ExploreVintage explore_vintage { get; set; }
        public List<SelectedFilter> selected_filters { get; set; }
        public string e { get; set; }
    }


}
