using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WineScraper.Models;

namespace WineScraper
{

    public class DataStates
    {
        public string Name { get; set; }
        public List<NordServers> Servers { get; set; } = new List<NordServers>();
    }

    public class NordManager
    {
        static Settings settings = new Settings();
        public static List<DataStates> NordServers { get; set; } = new List<DataStates>();
        
        public static void FetchServers()
        {
            var settingsJson = File.ReadAllText("settings.json");
            settings = JsonConvert.DeserializeObject<Settings>(settingsJson);
            Console.WriteLine("Finding best servers. Please wait...");
            var client = new RestClient("https://api.nordvpn.com/");
            var request = new RestRequest("v1/servers?limit=500000", Method.GET);
            var result = client.Execute(request);
            result.Content = "{Servers:" + result.Content + "}";
            var servers = JsonConvert.DeserializeObject<NordRoot>(result.Content);
            var USAServers = servers.Servers.Where(x => x.locations.Count > 0 && x.locations[0].country.name == "United States" && x.status == "online").ToList();

            var filteredServers = new List<NordServers>();

            //List<Task> taskList = new List<Task>();
            //for (int i = 1; i <= USAServers.Count; ++i)
            //{
            //    int index = i-1;
            //    Task task = Task.Factory.StartNew((Action)(() => { if (ValidateServer(USAServers[index])) { filteredServers.Add(USAServers[index]); Console.WriteLine("Success: "+ USAServers[index].locations[0].country.city.name + "=>" + USAServers[index].hostname+"==========================="); } else Console.WriteLine("Failed: "+ USAServers[index].locations[0].country.city.name + "=>" + USAServers[index].hostname); }));
            //    taskList.Add(task);
            //    if ( i == USAServers.Count)
            //    {
            //        foreach (Task task2 in taskList)
            //        {
            //            while (!task2.IsCompleted)
            //            { }
            //        }
            //    }
            //}
            filteredServers = USAServers;
            foreach (var server in filteredServers.GroupBy(x => x.locations[0].country.city.name))
            {
                NordServers.Add(new DataStates { Name = server.Key, Servers = server.OrderBy(x => x.load).ToList() });
            }
        }

        public static bool ValidateServer(NordServers server)
        {
            try
            {
                FilterModel model = new FilterModel { };
                var client = new RestClient("https://www.vivino.com/");

                string url = $"api/explore/explore?country_code={model.country_code}&currency_code={model.currency_code}&grape_filter=varietal&min_rating={model.min_rating}&order_by=ratings_average&order=desc&page={model.page}&price_range_max={model.price_range_max}&price_range_min={model.price_range_min}";
                foreach (var wine in model.wine_type_ids)
                {
                    url += $"&wine_type_ids[]={wine}";
                }
                var request = new RestRequest(url, Method.GET);
                var proxy = new WebProxy
                {
                    Address = new Uri($"http://{server.hostname}:{80}"),
                    BypassProxyOnLocal = false,
                    UseDefaultCredentials = false,

                    // *** These creds are given to the proxy server, not the web server ***
                    Credentials = new NetworkCredential(
                     userName: settings.Proxy.Username,
                     password: settings.Proxy.Password)
                };
                client.Proxy = proxy;
                var queryResult = client.Execute<RootTest>(request);
                if (queryResult.StatusCode == System.Net.HttpStatusCode.OK)
                    return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }
    }



    public class RootTest
    {
        public string name { get; set; }
    }



    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string dns_name { get; set; }
        public int hub_score { get; set; }
    }

    public class Country
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public City city { get; set; }
    }

    public class Location
    {
        public int id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public Country country { get; set; }
    }

    public class Service
    {
        public int id { get; set; }
        public string name { get; set; }
        public string identifier { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }

    public class Metadata
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Pivot
    {
        public int technology_id { get; set; }
        public int server_id { get; set; }
        public string status { get; set; }
    }

    public class Technology
    {
        public int id { get; set; }
        public string name { get; set; }
        public string identifier { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public List<Metadata> metadata { get; set; } = new List<Metadata>();
        public Pivot pivot { get; set; }
    }

    public class Type
    {
        public int id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string title { get; set; }
        public string identifier { get; set; }
    }

    public class Group
    {
        public int id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string title { get; set; }
        public string identifier { get; set; }
        public Type type { get; set; }
    }

    public class Value
    {
        public int id { get; set; }
        public string value { get; set; }
    }

    public class Specification
    {
        public int id { get; set; }
        public string title { get; set; }
        public string identifier { get; set; }
        public List<Value> values { get; set; } = new List<Value>();
    }

    public class Ip2
    {
        public int id { get; set; }
        public string ip { get; set; }
        public int version { get; set; }
    }

    public class Ip
    {
        public int id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public int server_id { get; set; }
        public int ip_id { get; set; }
        public string type { get; set; }
        public Ip2 ip { get; set; }
    }

    public class NordServers
    {
        public int id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string name { get; set; }
        public string station { get; set; }
        public string hostname { get; set; }
        public int load { get; set; }
        public string status { get; set; }
        public List<Location> locations { get; set; } = new List<Location>();
        public List<Service> services { get; set; } = new List<Service>();
        public List<Technology> technologies { get; set; } = new List<Technology>();
        public List<Group> groups { get; set; } = new List<Group>();
        public List<Specification> specifications { get; set; } = new List<Specification>();
        public List<Ip> ips { get; set; } = new List<Ip>();
    }

    public class NordRoot
    {
        public List<NordServers> Servers { get; set; } = new List<NordServers>();
    }


}
