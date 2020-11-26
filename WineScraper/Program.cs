using CsvHelper;
using FahadKernal.NordVPN.ServerSwitcher;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Management;
using WineScraper.Models;

namespace WineScraper
{
    class Program
    {
        static Settings settings = new Settings();
        static List<Vintage_TopListRanking> vintage_top_list_rankings = new List<Vintage_TopListRanking>();
        static List<DataModel> entries = new List<DataModel>();
        public static List<Vintage_Wine_Region_Country_MostUsedGrape> vintage_wine_region_country_most_used_grapes = new List<Vintage_Wine_Region_Country_MostUsedGrape>();
        public static List<Vintage_Wine_Style_Country_MostUsedGrape> vintage_wine_style_country_most_used_grapes = new List<Vintage_Wine_Style_Country_MostUsedGrape>();
        static List<Vintage_Wine_Taste_Flavor> vintage_wine_taste_flavor = new List<Vintage_Wine_Taste_Flavor>();
        public static List<Vintage_Wine_Style_Food> vintage_wine_style_food = new List<Vintage_Wine_Style_Food>();
        public static List<Vintage_Wine_Style_Grapes> vintage_wine_style_grapes = new List<Vintage_Wine_Style_Grapes>();
        static string Session = "";
        static string ScrapingRegion = "";
        static string RegionTitle = "";
        public static ServersBy SelectedRegion { get; set; }
        static int Counter = -1;
        static void Main(string[] args)
        {
            try
            {

                var servers = NordManager.ServersByUSCities();
                Console.WriteLine("Please select region");
                foreach (var server in servers.Where(x => x.Servers.Count > 0))
                {
                    Console.WriteLine($"{servers.IndexOf(server)} - {server.Name}({server.Servers.Count})");
                }
                Console.WriteLine("Select Region: ");
                var index = Convert.ToInt32(Console.ReadLine());
                SelectedRegion = servers[index];
                Console.Clear();
                Console.WriteLine("You have selected: " + SelectedRegion.Name);
                var today = DateTime.Now;
                Session = $"{today.Year}{today.Month}{today.Day}{today.Hour}{today.Minute}{today.Second}";
                //Load Settings
                var settingsJson = File.ReadAllText("settings.json");
                settings = JsonConvert.DeserializeObject<Settings>(settingsJson);
                Console.WriteLine("Settings loaded...");

                ScrapeData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to continue due to \"{ex.Message}\"");
            }

            Console.WriteLine("Press any key to continue...");

            Console.ReadKey();
        }
        private static void MapData(Root data)
        {
            foreach (var record in data.explore_vintage.matches)
            {
                if (!entries.Exists(x => x.vintage_id == record.vintage.id))
                {
                    var entry = new DataModel();
                    if (record.vintage != null)
                    {
                        entry.vintage_id = record.vintage.id;
                        entry.vintage_seo_name = record.vintage.seo_name;
                        entry.vintage_name = record.vintage.name;

                        entry.vintage_year = record.vintage.year;

                        entry.vintage_has_valid_ratings = record.vintage.has_valid_ratings;

                        if (record.vintage.grapes != null)
                        {
                            entry.vintage_grapes = record.vintage.grapes;
                        }

                        if (record.vintage.statistics != null)
                        {
                            entry.vintage_statistics_status = record.vintage.statistics.status;
                            entry.vintage_statistics_ratings_count = record.vintage.statistics.ratings_count;
                            entry.vintage_statistics_ratings_average = record.vintage.statistics.ratings_average;
                            entry.vintage_statistics_labels_count = record.vintage.statistics.labels_count;
                        }

                        if (record.vintage.image != null)
                        {
                            entry.vintage_image_location = record.vintage.image.location;
                            if (record.vintage.image.variations != null)
                            {
                                entry.vintage_image_variations_bottle_large = record.vintage.image.variations.bottle_large;
                                entry.vintage_image_variations_label_large = record.vintage.image.variations.label_large;
                            }
                        }

                        if (record.vintage.wine != null)
                        {
                            entry.vintage_wine_id = record.vintage.wine.id;
                            entry.vintage_wine_name = record.vintage.wine.name;
                            entry.vintage_wine_seo_name = record.vintage.wine.seo_name;
                            entry.vintage_wine_has_valid_ratings = record.vintage.has_valid_ratings;
                            if (record.vintage.wine.winery != null)
                            {
                                entry.vintage_wine_winery_id = record.vintage.wine.winery.id;
                                entry.vintage_wine_winery_name = record.vintage.wine.winery.name;
                                entry.vintage_wine_winery_seo_name = record.vintage.wine.winery.seo_name;
                            }
                            if (record.vintage.wine.region != null)
                            {
                                entry.vintage_wine_region_id = record.vintage.wine.region.id;
                                entry.vintage_wine_region_name = record.vintage.wine.region.name;
                                entry.vintage_wine_region_seo_name = record.vintage.wine.region.seo_name;
                                if (record.vintage.wine.region.country != null)
                                {
                                    entry.vintage_wine_region_country_code = record.vintage.wine.region.country.code;
                                    entry.vintage_wine_region_country_native_name = record.vintage.wine.region.country.native_name;
                                    entry.vintage_wine_region_country_currency_code = record.vintage.wine.region.country.currency.code;
                                    entry.vintage_wine_region_country_currency_name = record.vintage.wine.region.country.currency.name;
                                    entry.vintage_wine_region_country_currency_prefix = record.vintage.wine.region.country.currency.prefix;

                                    entry.vintage_wine_region_country_name = record.vintage.wine.region.country.name;
                                    entry.vintage_wine_region_country_seo_name = record.vintage.wine.region.country.seo_name;
                                    entry.vintage_wine_region_country_regions_count = record.vintage.wine.region.country.regions_count;
                                    entry.vintage_wine_region_country_users_count = record.vintage.wine.region.country.users_count;
                                    entry.vintage_wine_region_country_wineries_count = record.vintage.wine.region.country.wineries_count;

                                    if (record.vintage.wine.region.country.most_used_grapes != null && record.vintage.wine.region.country.most_used_grapes.Count > 0)
                                        foreach (var item in record.vintage.wine.region.country.most_used_grapes)
                                        {
                                            vintage_wine_region_country_most_used_grapes.Add(new Vintage_Wine_Region_Country_MostUsedGrape
                                            {
                                                vintage_id = record.vintage.id,
                                                wine_id = record.vintage.wine.id,
                                                region_id = record.vintage.wine.region.id,
                                                id = item.id,
                                                name = item.name,
                                                seo_name = item.seo_name,
                                                wines_count = item.wines_count,
                                                has_detailed_info = item.has_detailed_info
                                            });
                                        }

                                }

                            }

                            if (record.vintage.wine.taste != null && record.vintage.wine.taste.structure != null)
                            {
                                entry.vintage_wine_taste_structure_acidity = record.vintage.wine.taste.structure.acidity;
                                entry.vintage_wine_taste_structure_fizziness = record.vintage.wine.taste.structure.fizziness;
                                entry.vintage_wine_taste_structure_intensity = record.vintage.wine.taste.structure.intensity;
                                entry.vintage_wine_taste_structure_sweetness = record.vintage.wine.taste.structure.sweetness;
                                entry.vintage_wine_taste_structure_tannin = record.vintage.wine.taste.structure.tannin;
                                entry.vintage_wine_taste_structure_user_structure_count = record.vintage.wine.taste.structure.user_structure_count;
                                entry.vintage_wine_taste_structure_calculated_structure_count = record.vintage.wine.taste.structure.calculated_structure_count;
                                if (record.vintage.wine.taste.flavor != null && record.vintage.wine.taste.flavor.Count > 0)
                                {
                                    foreach (var item in record.vintage.wine.taste.flavor)
                                    {
                                        var xx = new Vintage_Wine_Taste_Flavor
                                        {
                                            vintage_id = record.vintage.id,
                                            vintage_wine_id = record.vintage.wine.id,
                                            group = item.group,

                                        };
                                        if (item.stats != null)
                                        {
                                            xx.stats_count = item.stats.count;
                                            xx.stats_score = item.stats.score;
                                        }
                                        vintage_wine_taste_flavor.Add(xx);
                                    }

                                }

                            }

                            if (record.vintage.wine.statistics != null)
                            {
                                entry.vintage_wine_statistics_status = record.vintage.wine.statistics.status;
                                entry.vintage_wine_statistics_ratings_count = record.vintage.wine.statistics.ratings_count;
                                entry.vintage_wine_statistics_ratings_average = record.vintage.wine.statistics.ratings_average;
                                entry.vintage_wine_statistics_labels_count = record.vintage.wine.statistics.labels_count;
                                entry.vintage_wine_statistics_vintages_count = record.vintage.wine.statistics.vintages_count;
                            }


                            if (record.vintage.wine.style != null)
                            {
                                entry.vintage_wine_style_id = record.vintage.wine.style.id;
                                entry.vintage_wine_style_seo_name = record.vintage.wine.style.seo_name;
                                entry.vintage_wine_style_regional_name = record.vintage.wine.style.regional_name;
                                entry.vintage_wine_style_varietal_name = record.vintage.wine.style.varietal_name;
                                entry.vintage_wine_style_description = record.vintage.wine.style.description;
                                entry.vintage_wine_style_blurb = record.vintage.wine.style.blurb;
                                if (record.vintage.wine.style.interesting_facts != null && record.vintage.wine.style.interesting_facts.Count > 0)
                                {
                                    entry.vintage_wine_styleinteresting_facts = string.Join(";", record.vintage.wine.style.interesting_facts);
                                }

                                if (record.vintage.wine.style.region != null)
                                {

                                    entry.vintage_wine_style_region_id = record.vintage.wine.style.region.id;
                                    entry.vintage_wine_style_region_name_en = record.vintage.wine.style.region.name_en;
                                    entry.vintage_wine_style_region_seo_name = record.vintage.wine.style.region.seo_name;
                                    if (record.vintage.wine.style.region.country != null)
                                    {
                                        entry.vintage_wine_style_region_country_code = record.vintage.wine.style.region.country.code;
                                        entry.vintage_wine_style_region_country_name = record.vintage.wine.style.region.country.name;
                                        entry.vintage_wine_style_region_country_native_name = record.vintage.wine.style.region.country.native_name;
                                        entry.vintage_wine_style_region_country_seo_name = record.vintage.wine.style.region.country.seo_name;
                                        if (record.vintage.wine.style.region.country.currency != null)
                                        {
                                            entry.vintage_wine_style_region_country_currency_code = record.vintage.wine.style.region.country.currency.code;
                                            entry.vintage_wine_style_region_country_currency_name = record.vintage.wine.style.region.country.currency.name;
                                            entry.vintage_wine_style_region_country_currency_prefix = record.vintage.wine.style.region.country.currency.prefix;
                                        }

                                        entry.vintage_wine_style_region_country_regions_count = record.vintage.wine.style.region.country.regions_count;
                                        entry.vintage_wine_style_region_country_users_count = record.vintage.wine.style.region.country.users_count;
                                        entry.vintage_wine_style_region_country_wines_count = record.vintage.wine.style.region.country.wines_count;
                                        entry.vintage_wine_style_region_country_wineries_count = record.vintage.wine.style.region.country.wineries_count;
                                    }

                                }




                                entry.vintage_wine_style_body = record.vintage.wine.style.body;
                                entry.vintage_wine_style_body_description = record.vintage.wine.style.description;
                                entry.vintage_wine_style_acidity = record.vintage.wine.style.acidity;
                                entry.vintage_wine_style_acidity_description = record.vintage.wine.style.description;

                                if (record.vintage.wine.style.food != null && record.vintage.wine.style.food.Count > 0)
                                {
                                    foreach (var item in record.vintage.wine.style.food)
                                    {
                                        vintage_wine_style_food.Add(new Vintage_Wine_Style_Food
                                        {
                                            vintage_id = record.vintage.id,
                                            wine_id = record.vintage.wine.id,
                                            style_id = record.vintage.wine.style.id,
                                            id = item.id,
                                            name = item.name,
                                            seo_name = item.seo_name
                                        });
                                    }
                                }

                                if (record.vintage.wine.style.grapes != null && record.vintage.wine.style.grapes.Count > 0)
                                {
                                    foreach (var item in record.vintage.wine.style.grapes)
                                    {
                                        vintage_wine_style_grapes.Add(new Vintage_Wine_Style_Grapes
                                        {
                                            vintage_id = record.vintage.id,
                                            wine_id = record.vintage.wine.id,
                                            style_id = record.vintage.wine.style.id,
                                            id = item.id,
                                            name = item.name,
                                            seo_name = item.seo_name,
                                            has_detailed_info = item.has_detailed_info,
                                            wines_count = item.wines_count
                                        });
                                    }

                                }

                                if (record.vintage.wine.style.country != null)
                                {

                                    entry.vintage_wine_style_country_code = record.vintage.wine.style.country.code;
                                    entry.vintage_wine_style_country_native_name = record.vintage.wine.style.country.native_name;
                                    entry.vintage_wine_style_country_name = record.vintage.wine.style.country.name;
                                    entry.vintage_wine_style_country_seo_name = record.vintage.wine.style.country.seo_name;
                                    entry.vintage_wine_style_country_regions_count = record.vintage.wine.style.country.regions_count;
                                    entry.vintage_wine_style_country_users_count = record.vintage.wine.style.country.users_count;
                                    entry.vintage_wine_style_country_wines_count = record.vintage.wine.style.country.wines_count;
                                    entry.vintage_wine_style_country_wineries_count = record.vintage.wine.style.country.wineries_count;

                                    if (record.vintage.wine.style.country.most_used_grapes != null && record.vintage.wine.style.country.most_used_grapes.Count > 0)
                                    {
                                        foreach (var item in record.vintage.wine.style.country.most_used_grapes)
                                        {
                                            vintage_wine_style_country_most_used_grapes.Add(new Vintage_Wine_Style_Country_MostUsedGrape
                                            {
                                                vintage_id = record.vintage.id,
                                                wine_id = record.vintage.wine.id,
                                                style_id = record.vintage.wine.style.id,
                                                id = item.id,
                                                name = item.name,
                                                seo_name = item.seo_name,
                                                wines_count = item.wines_count,
                                                has_detailed_info = item.has_detailed_info
                                            });
                                        }
                                    }
                                }
                            }
                        }

                        if (record.vintage.top_list_rankings.Count > 0)
                        {
                            foreach (var item in record.vintage.top_list_rankings)
                            {
                                vintage_top_list_rankings.Add(new Vintage_TopListRanking
                                {
                                    vintage_id = record.vintage.id,
                                    vintage_topListRanking_top_list_id = item.top_list.id,
                                    vintage_topListRanking_top_list_name = item.top_list.name,
                                    vintage_topListRanking_description = item.description,
                                    vintage_topListRanking_previous_rank = item.previous_rank,
                                    vintage_topListRanking_rank = item.rank,
                                    vintage_topListRanking_top_list_location = item.top_list.location,
                                    vintage_topListRanking_top_list_seo_name = item.top_list.seo_name,
                                    vintage_topListRanking_top_list_type = item.top_list.type,
                                    vintage_topListRanking_top_list_year = item.top_list.year
                                });
                            }
                            //Loop

                        }
                    }

                    if (record.price != null)
                    {
                        entry.price_id = record.price.id;
                        entry.price_amount = record.price.amount;
                        entry.price_discounted_from = record.price.discounted_from;
                        entry.price_sku = record.price.sku;
                        entry.price_url = record.price.url;
                        if (record.price.currency != null)
                            entry.price_currency_code = record.price.currency.code;

                        if (record.price.bottle_type != null)
                        {
                            entry.price_bottle_type_id = record.price.bottle_type.id;
                            entry.price_bottle_type_name = record.price.bottle_type.name;
                            entry.price_bottle_type_volume_ml = record.price.bottle_type.volume_ml;
                        }
                    }

                    entries.Add(entry);
                }
            }

            Console.WriteLine("Unique Records: " + entries.Count);
        }
        private static void ExportData()
        {
            var path = settings.SaveFilesTo + $"\\{Session}-{RegionTitle}";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (vintage_wine_style_grapes.Count > 0)
                using (var writer = new StreamWriter(Path.Combine(path, "vintage_wine_style_grapes.csv"), false, Encoding.UTF8))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(vintage_wine_style_grapes);
                }

            if (vintage_wine_style_food.Count > 0)
                using (var writer = new StreamWriter(Path.Combine(path, "vintage_wine_style_food.csv"), false, Encoding.UTF8))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(vintage_wine_style_food);
                }

            if (entries.Count > 0)
                using (var writer = new StreamWriter(Path.Combine(path, "result.csv"), false, Encoding.UTF8))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(entries);
                }

            if (vintage_wine_taste_flavor.Count > 0)
                using (var writer = new StreamWriter(Path.Combine(path, "Vintage_Wine_Taste_Flavor.csv"), false, Encoding.UTF8))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(vintage_wine_taste_flavor);
                }

            if (vintage_wine_region_country_most_used_grapes.Count > 0)
                using (var writer = new StreamWriter(Path.Combine(path, "vintage_wine_region_country_most_used_grapes.csv"), false, Encoding.UTF8))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(vintage_wine_region_country_most_used_grapes);
                }

            if (vintage_wine_style_country_most_used_grapes.Count > 0)
                using (var writer = new StreamWriter(Path.Combine(path, "vintage_wine_style_country_most_used_grapes.csv"), false, Encoding.UTF8))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(vintage_wine_style_country_most_used_grapes);
                }

            if (vintage_top_list_rankings.Count > 0)
                using (var writer = new StreamWriter(Path.Combine(path, "vintage_top_list_rankings.csv"), false, Encoding.UTF8))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(vintage_top_list_rankings);
                }
        }
        private static void ScrapeData()
        {
            //Possible wine Ids
            var wines = settings.WineTypes;

            foreach (var wine in wines)
            {
                //Possible rating filter
                foreach (var rating in settings.Rating)
                {
                    Console.WriteLine($"Wine: {wine.Title}, Rating: {rating}\n======================================================");
                    int minPrice = 0; int maxPrice = 0;
                    //max and min price filter
                    for (int p = settings.PriceRange.Min; p <= settings.PriceRange.Max; p += settings.PriceJump)
                    {
                        maxPrice = p;
                        if (minPrice == 0 && maxPrice == 0)
                            continue;
                        var data1 = RequestData(new FilterModel { price_range_min = minPrice, price_range_max = maxPrice, min_rating = rating, wine_type_ids = new List<int> { wine.Id } });
                        //got result let parse it and check how many total records are there
                        if (data1.explore_vintage.records_matched < 2025)
                        {
                            Console.WriteLine($"State:{data1.explore_vintage.market.state} ==> Price Range: [ min:{minPrice}, max:{maxPrice} ]. Results: " + data1.explore_vintage.records_matched);
                            if (maxPrice == 500)
                            {
                                var search1 = new FilterModel { price_range_min = minPrice, price_range_max = maxPrice, min_rating = rating, wine_type_ids = new List<int> { wine.Id } };
                                var data2 = RequestData(search1);
                                Console.WriteLine("Max limit reached:  " + data2.explore_vintage.records_matched);
                                ExtractData(data2, search1);
                                ExportData();
                            }
                            else
                                continue;
                        }
                        else
                        {
                            var searchModel = new FilterModel { price_range_min = minPrice, price_range_max = maxPrice - 2, min_rating = rating, wine_type_ids = new List<int> { wine.Id } };
                            data1 = RequestData(searchModel);
                            minPrice = maxPrice - settings.PriceJump;
                            p -= settings.PriceJump;

                            Console.WriteLine("Max limit reached:  " + data1.explore_vintage.records_matched);
                            ExtractData(data1, searchModel);
                            ExportData();

                        }
                    }

                    //Last filter search
                    var search = new FilterModel { price_range_min = minPrice, price_range_max = maxPrice, min_rating = rating, wine_type_ids = new List<int> { wine.Id } };
                    var data = RequestData(search);
                    Console.WriteLine("Max mimit reached:  " + data.explore_vintage.records_matched);
                    ExtractData(data, search);
                    ExportData();
                }
            }


        }
        private static void ExtractData(Root data, FilterModel searchModel)
        {
            int pages = (data.explore_vintage.records_matched + 25 - 1) / 25;

            List<int> list = new List<int>();
            for (int i = 1; i <= pages; i++)
            {
                list.Add(i);
            }
            int num1 = (list.Count + 5 - 1) / 5;
            List<Task> taskList = new List<Task>();
            for (int index = 1; index <= num1; ++index)
            {
                int num2 = index - 1;
                var dataPages = list.Skip(num2 * 5).Take(5).ToList();
                Task task1 = Task.Factory.StartNew((Action)(() => GetPagesData(dataPages, searchModel)));
                taskList.Add(task1);
                if (index % 30 == 0 || index == num1)
                {
                    foreach (Task task2 in taskList)
                    {
                        while (!task2.IsCompleted)
                        { }
                    }
                }
            }
        }
        private static void GetPagesData(List<int> pages, FilterModel model)
        {
            foreach (var page in pages)
            {
                model.page = page;
                MapData(RequestData(model, false));
                Console.WriteLine("Processing Page: " + page);

            }
        }
        private static Root RequestData(FilterModel model, bool switchServer = true)
        {
            try
            {
            tryagain:
                if (switchServer)
                {
                    if (Counter == -1 || Counter > settings.ServerSwitch)
                    {
                        var hostNames = SelectedRegion.Servers.Select(x => x.name).ToList();
                        if (!string.IsNullOrEmpty(ScrapingRegion))
                            hostNames.Remove(ScrapingRegion);
                        ScrapingRegion = NordManager.RotateNord(hostNames, SelectedRegion.Servers);
                        Counter = 0;
                    }
                    else
                        Counter += 1;
                }
                string content = string.Empty;

                var client = new RestClient("https://www.vivino.com/");
                string url = $"api/explore/explore?country_code={model.country_code}&currency_code={model.currency_code}&grape_filter=varietal&min_rating={model.min_rating}&order_by=ratings_average&order=desc&page={model.page}&price_range_max={model.price_range_max}&price_range_min={model.price_range_min}";
                foreach (var wine in model.wine_type_ids)
                {
                    url += $"&wine_type_ids[]={wine}";
                }
                var request = new RestRequest(url, Method.GET);
                IRestResponse queryResult = client.Execute(request);
                if (queryResult == null)
                {
                    Counter = settings.ServerSwitch + 1;
                    switchServer = true;
                    goto tryagain;
                }
                content = (queryResult.StatusCode == System.Net.HttpStatusCode.OK) ? queryResult.Content : "";
                if (string.IsNullOrEmpty(content))
                    Console.WriteLine($"Error: Unable to load {url}");

                if (!string.IsNullOrEmpty(content))
                {
                    //got result let parse it and check how many total records are there
                    var result = JsonConvert.DeserializeObject<Root>(content);
                    RegionTitle = result.explore_vintage.market.state;
                    return result;
                }
                else
                    Console.WriteLine("Server didn't returned any data");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something happened: {ex.Message}");
            }
            return null;
        }
    }
}
