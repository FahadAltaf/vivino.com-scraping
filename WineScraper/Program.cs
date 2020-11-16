using CsvHelper;
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
        static void Main(string[] args)
        {
            try
            {
                //Load Settings
                var settingsJson = File.ReadAllText("settings.json");
                settings = JsonConvert.DeserializeObject<Settings>(settingsJson);
                Console.WriteLine("Settings Loaded...");

                ScrapeData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"We are unable to continue due to \"{ex.Message}\"");
            }
            
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        public void ExtractData()
        {
            foreach (var file in Directory.GetFiles(@"C:\Users\kernal\source\repos\WineScraper\WineScraper\bin\Debug\Data", "*.json"))
            {
                var queryResult = JsonConvert.DeserializeObject<Root>(File.ReadAllText(file));
                var data = queryResult.explore_vintage.matches;

                foreach (var record in data)
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

                            if (record.vintage.wine.region != null)
                            {
                                entry.vintage_wine_region_id = record.vintage.wine.region.id;
                                entry.vintage_wine_region_name = record.vintage.wine.region.name;
                                entry.vintage_wine_region_seo_name = record.vintage.wine.region.seo_name;
                                if (record.vintage.wine.region.country != null)
                                {
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
                Console.WriteLine(file);
            }

            using (var writer = new StreamWriter("vintage_wine_style_grapes.csv", false, Encoding.UTF8))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(vintage_wine_style_grapes);
            }

            using (var writer = new StreamWriter("vintage_wine_style_food.csv", false, Encoding.UTF8))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(vintage_wine_style_food);
            }

            using (var writer = new StreamWriter("result.csv", false, Encoding.UTF8))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(entries);
            }

            using (var writer = new StreamWriter("Vintage_Wine_Taste_Flavor.csv", false, Encoding.UTF8))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(vintage_wine_taste_flavor);
            }

            using (var writer = new StreamWriter("vintage_wine_region_country_most_used_grapes.csv", false, Encoding.UTF8))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(vintage_wine_region_country_most_used_grapes);
            }

            using (var writer = new StreamWriter("vintage_wine_style_country_most_used_grapes.csv", false, Encoding.UTF8))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(vintage_wine_style_country_most_used_grapes);
            }

            using (var writer = new StreamWriter("vintage_top_list_rankings.csv", false, Encoding.UTF8))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(vintage_top_list_rankings);
            }
        }
        public static void GetPrice(string id)
        {
            var cookie = @"first_time_visit=Nm45eTlnMUtBR2tzQkpNWmorMzlpdz09LS1WdENXRjRPMi9VRzhiaXNZTHJzRUN3PT0%3D--f0b967e8db68b696ae4c076ea251f459149d5912; _ga=GA1.2.367751601.1601838394; __auc=c9251cff174f5022b646f6e8bb2; _fbp=fb.1.1601838394798.673697649; _hjTLDTest=1; _hjid=b7315f78-1cd0-4647-9ee4-c4c27d56c302; __stripe_mid=712fe9c9-b35a-4a89-84b6-c2b22984a30aee5dfc; eeny_meeny_test_checkout_login_v1=Urz8CGWB3oeEx3lZtuoMB05%2F3g4MLZ6kSowQX3dASQRQDga3h5KHm66FO6hg89S%2BK3BLH3R50PQRGua6Pwocnw%3D%3D; client_cache_key=UkNBQ01TQXUwcE1mQnpkWHlnQW1hRFU0SzdGN1AvK2FhbkRJcE1QZU1xTT0tLXdjT0JPdGtKWUhvL3ZNK1FoK1RRQ1E9PQ%3D%3D--d83de0ad86ca263c9c92a7684c1c9655c276b1e2; _gid=GA1.2.211538869.1602500694; __asc=8ce1d06b1751c7c1005a95aba83; _hjIncludedInSessionSample=1; _hjAbsoluteSessionInProgress=0; _hp2_ses_props.3503103446=%7B%22us%22%3A%22123201%22%2C%22um%22%3A%22impact%22%2C%22uc%22%3A%22mediapartner%22%2C%22ts%22%3A1602500695234%2C%22d%22%3A%22www.vivino.com%22%2C%22h%22%3A%22%2F%22%2C%22q%22%3A%22%3Firgwc%3D1%26clickid%3DTAKWbO0BYxyLTZA0O7XQxx3QUkE0J11VUS45xk0%26utm_medium%3Dimpact%26utm_source%3D123201%26utm_content%3Dmediapartner%26affsrc%3D1%22%7D; recently_viewed=bXlZaURFNzVGcVFrRnFkc2FDNHlYbU85TTU5Ky90QjNsQjd5S3dKV0ZrNkFPRkZFS0V6ZGZhUlZEdnkyMzBtSmJ4Z0orTklZUjd1elVzbEJsVE0rRDkwV2FFdXptV1RLNytiOTRDR3JJWjdvSnBDSkRPOUNqdGphQnNNOS93WGc3STltK0JpUk9wNjRwRDZjMGtMaDNzNFhvaWNwaTZPS04yeXNFNXBoQ2lkUzl6U3Q2b2RZNzFwMjZuVmx2WmJZVXl3UDdqL0s1T09IK0ZUczRmSlA4VUxqQkU4TmJFbjczdFRTdTlEMnVsWXlLN0tjeHdHNkExeXlqM2tCbHl4MnRTQlN2VldISUo4b0UzSTlsL2o5RHpDekF0NDNMM1ZBU3djTURlUmphYi9mY3lwWW1GdzJBYmVaRTJnb3FzYmFVUXdyczIwdENCbU85N0lRdldsMUhEbm9aQUcrOGFGd0MvUjVWcUp1b2dnaC9WeE5LNFVtWitaZXdmb0RVMnh3MGMwNEVPOUJxUXVtVmZOU0R6czBKeDhrUmNrblNiT3RzRzNjd1NFMzhWNStPUXNoZ2FZZHRHRHRFaXFDMmZzTXdwMzdoWDFwM0tlU0JuTEdJbXp0V2pmWXRobjkybzB2NWRnYzAvbWMrK1R6bVVoeE5oZFJTVldPTFJraVEyWTgrbnMzRkE0RWlJYXVXMU5GbWJFeC9WeDRaanBZMnZQbWNSb1JYRmw0Y2tmam84VlpUK1duTXFrdzl2b0JGWVEwK29uZ0VmZE9sdzVDU2FtTGcvdEl3MDd6a0JFTm1LRTJadjVqZzNNMGJ0dz0tLWdWQjlsaURvUlNpZ2NjN3A0M3RFeXc9PQ%3D%3D--56866ddd49a33bd6dbdb07df652488b999213acd; _hp2_id.3503103446=%7B%22userId%22%3A%226963833353307581%22%2C%22pageviewId%22%3A%222894796524267915%22%2C%22sessionId%22%3A%227578634507964165%22%2C%22identity%22%3Anull%2C%22trackerVersion%22%3A%224.0%22%7D; _ruby-web_session=OEsvb0tvUGF1d2lESjVBeExiRUVsTEREQXRycmQ2WGRTSCs3NEVJRHNQeWN4MVoyMWVTenR5YkI2QmVZdGt6M1BoNHd1VVE0dGFCRTFFYXg2Q0xsTERjSWxlVzBZZWV1WmpLVzFjS3E0bm4wcnBXTUdqNmVBUlVncURRTVg0NS9rM2JGSFJxTm9YOHdEV294VklpWG9OaHJvR3FoQ3pNZDNzNVlTV0NXYmYzWi80Y0xMM0lINktoWHBrcFZlcXNSMHVuU0pCMzA2WnpFd2JXYjdROEFPS25QTGJMQWlTelNYSnBJMWtJMUhsS29YZFV2QUNDYkVXYzdRdzhuZGtobmVqbDQwU2RTak5RZWZEMkhjYVdiUlpvOXptNkxYNUZSVW1YNTVITDN2cHUzKzloNDN0Zy81SGowMHcyYUlLeFNQbE4yalVlRXdjYlJocXRXNVQ2YkRIaEg3NlNuc3J5TGliQURJZU9TVmV6ek05cWdvemZlM2xGekZMT2gzb3BscytCYzYyNk4vdnJvRWNlQTRidnMzZnZsUUI3cVlOU2VQYVFWYVFpcUZ1Zjh3ZnYwMnpYNitESkxsdWJoMzZ1VTFDZS9zblZ0aGJyREpiYll1TTJRaE1BNE1aZXB0MzBhWmQ1cGZIazRiNkU9LS1vQ2Q4SzZVWE1xRjloR0xhKzEzenJnPT0%3D--0896c0cc3b64e296c475ae86e78fed05d402c12a";
            var client = new RestClient("https://www.vivino.com/");
            //client.Proxy = new WebProxy("127.0.0.1", 2400);
            var request = new RestRequest($"api/prices?vintage_ids[]={id}&currency_code=CA", Method.GET);
            //request.AddHeader("cookie", cookie);
            request.AddCookie("_ruby-web_session", "NHcrWUJSV1ZUS0lWMXZhT2F5aGdRbUVZczl0TG1LYUhVYnpnRGZpY3BJSUNVeC9IdlVuK2VqbUliWFZGMnltNjk5ME1mSFlUNFBHc3N5K0hKU3VlNzltUU5NaFA3R2s5RmZjQUNPZUMyRG9ZSHBrRFcra3BiQ05tOE1HcnV1MmhkNVhjaTVWOEtkMnBsaHA4TmcxdWpIMVVXLzZlMXdDRFVxOGlaRW1FcTBCd3VUYWhQbnNGNjJWSzhwam55QmkwWnNYOGl2Y3ZTbm9XbHhiTmQyeFJvSnppeitBK1dQV1JJdjVlays0ZGNzbkYvdm1SWXI4VE4xT292R3RwclVZcDJrTGRKdDNwMEppMHE4VzRWSzNXMUpOdXNWczZWSjBOa3ZyUjRHZnBqNXc2c2I2M3JqSjdXUFlyWE8rano3RGpuV2xYT29abTJqNnNBLzFpWnRQVzY3TmpWNU1xWGs2UjJpVGd4aVVrSE9YdXEzbnpBS2huN2hnNkp2SWV4Mk9LSzJ5SE5sb0EzdzZRc0tRQlExazFaUlU2OVlRaUtJcloySFdwQ3B6bVpPZmdtMVhwUVpnTXJIWExaT2ErODRPVi9EN0NCL3c1Y3ExOUszU1QyMmFFN0ZxeHRBRVBvcFlOczJqbjNMOWNyN289LS1pMjBOcUZHTDVOeVpFT3BkdVB1RjhBPT0%3D--242c9be2335f3028781fd935fc607dbfa2d5b3ef");
            var queryResult = client.Execute(request);
            if (queryResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string[] parts = queryResult.Content.Split(new string[] { $"\"{id}\":" }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Count() > 1)
                {
                    string text = parts[1];
                    text = text.Substring(0, text.Length - 3);
                    var data = JsonConvert.DeserializeObject<Price_Root>(text);
                    if (data != null && data.price != null)
                    {
                        Console.WriteLine(data.price.amount);
                    }
                    else
                        Console.WriteLine($"Problem: {id}");
                }
                else
                    Console.WriteLine("No Results");

            }
        }
        public static void ScrapeData()
        {
            //Possible wine Ids
            List<int> wineIds = settings.WineTypes.Select(x => x.Id).ToList();


            foreach (var wineId in wineIds)
            {
                //Possible rating filter
                for (int rating = 1; rating <= 5; rating++)
                {
                    int minPrice = 0; int maxPrice = 0;
                    //max and min price filter
                    for (int p = 10; p <= 500; p += 2)
                    {
                        maxPrice = p;
                        var content = RequestData(new FilterModel { price_range_min = minPrice, price_range_max = maxPrice, min_rating = rating, wine_type_ids = new List<int> { wineId } });
                        if (!string.IsNullOrEmpty(content))
                        {
                            //got result let parse it and check how many total records are there
                            var data = JsonConvert.DeserializeObject<Root>(content);
                            if (data.explore_vintage.records_matched < 2025)
                            {
                                Console.WriteLine($"Check min:{minPrice}, max:{maxPrice} " + data.explore_vintage.records_matched);
                                continue;
                            }
                            else
                            {
                             
                                content = RequestData(new FilterModel { price_range_min = minPrice, price_range_max = maxPrice-2, min_rating = rating, wine_type_ids = new List<int> { wineId } });
                                minPrice = maxPrice-2;
                                if (!string.IsNullOrEmpty(content))
                                {
                                    data = JsonConvert.DeserializeObject<Root>(content);
                                    Console.WriteLine("Event Reached " + data.explore_vintage.records_matched);
                                }

                            }
                        }
                    }

                }
            }
            List<int> list = new List<int>();
            for (int i = 1; i <= 991; i++)
            {
                if (!File.Exists($"Data/Page{i}.json"))
                    list.Add(i);
            }
            int num1 = (list.Count + 5 - 1) / 5;
            List<Task> taskList = new List<Task>();
            for (int index = 1; index <= num1; ++index)
            {
                int num2 = index - 1;
                var data = list.Skip(num2 * 5).Take(5).ToList();
                Task task1 = Task.Factory.StartNew((Action)(() => GetPagesData(data)));
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
        private static void GetPagesData(List<int> pages)
        {

            foreach (var page in pages)
            {
                RequestData(new FilterModel { });
            }
        }
        private static string RequestData(FilterModel model)
        {
            string content = string.Empty;

            var client = new RestClient("https://www.vivino.com/");
            client.Proxy = new WebProxy("127.0.0.1", 2400);
            string url = $"api/explore/explore?country_code={model.country_code}&currency_code={model.currency_code}&grape_filter=varietal&min_rating={model.min_rating}&order_by=ratings_average&order=desc&page={model.page}&price_range_max={model.price_range_max}&price_range_min={model.price_range_min}&vc_only=null";
            foreach (var wine in model.wine_type_ids)
            {
                url += $"&wine_type_ids[]={wine}";
            }
            var request = new RestRequest(url, Method.GET);
            var queryResult = client.Execute(request);
            content = (queryResult.StatusCode == System.Net.HttpStatusCode.OK) ? queryResult.Content : "";
            lif (string.IsNullOrEmpty(content))
                Console.WriteLine($"Error: Unable to load {url}");
            return content;
        }
    }
}
