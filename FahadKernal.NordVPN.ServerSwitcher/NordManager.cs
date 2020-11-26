using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FahadKernal.NordVPN.ServerSwitcher
{
    public class ServersBy
    {
        public string Name { get; set; }
        public List<NordServers> Servers { get; set; } = new List<NordServers>();
        
    }

    public class NordManager
    {
        static string lastIp = "";
        public static List<NordServers> NordServers { get; set; } = new List<NordServers>();

        public static List<NordServers> FetchServers(string country = "", string city = "", int maxload = 0, bool forceLoad = false)
        {
            if (!forceLoad && NordServers.Count > 0)
                return NordServers;
            Console.WriteLine("Fetching Available Servers...");
            var client = new RestClient("https://api.nordvpn.com/");
            var request = new RestRequest("v1/servers?limit=500000", Method.GET);
            var result = client.Execute(request);
            result.Content = "{Servers:" + result.Content + "}";
            var servers = JsonConvert.DeserializeObject<NordRoot>(result.Content);
            var USAServers = servers.Servers.Where(x => x.status == "online").ToList();

            if (maxload > 0)
            {
                USAServers = USAServers.Where(x => x.load < maxload).ToList();
            }

            if (!string.IsNullOrEmpty(country))
            {
                USAServers = USAServers.Where(x => x.locations.Count > 0 && x.locations[0].country.name == country).ToList();
            }

            if (!string.IsNullOrEmpty(city))
            {
                USAServers = USAServers.Where(x => x.locations.Count > 0 && x.locations[0].country.city.name == city).ToList();
            }
            NordServers = USAServers;
            return USAServers;
        }
        public static List<ServersBy> ServersByCountry()
        {
            List<ServersBy> servers = new List<ServersBy>();
            if (NordServers.Count == 0)
            {
                FetchServers();
            }

            foreach (var server in NordServers.GroupBy(x => x.locations[0].country.name))
            {
                servers.Add(new ServersBy { Name = server.Key, Servers = server.OrderBy(x => x.load).ToList() });
            }

            return servers;
        }

        public static List<ServersBy> ServersByUSCities()
        {
            List<ServersBy> servers = new List<ServersBy>();
            if (NordServers.Count == 0)
            {
                FetchServers();
            }

            foreach (var server in NordServers.Where(x => x.locations[0].country.name == "United States").GroupBy(x => x.locations[0].country.city.name))
            {
                servers.Add(new ServersBy { Name = server.Key, Servers = server.OrderBy(x => x.load).ToList() });
            }

            return servers;
        }
        public static string RotateNord(List<string> regionServers,List<NordServers> servers)//remove current selected server
        {tryagain:
            //Make sure norvpn is running
            if (Process.GetProcessesByName("NordVPN").Length > 0)
            {
                var filtered = servers.Where(x=>x.load<15);
                if (filtered.Count() > 0)
                {
                    regionServers = filtered.Select(x=>x.name).ToList();
                }
                //Select random server
                var random = new Random().Next(0, regionServers.Count);
                var randomServer = regionServers[random];
                Console.WriteLine("Switching server...");
                RunCommands(new List<string> { $"nordvpn -c -n \"{randomServer}\"" }, @"C:\Program Files\NordVPN");
                
                
                int tryCount = 0;
                try
                {
                    while (tryCount < 3)
                    {
                        Thread.Sleep(20 * 1000);
                        var client = new RestClient("http://lumtest.com/");
                        var request = new RestRequest("myip.json", Method.GET);
                        var queryResult = client.Execute<IPRoot>(request);
                        if (queryResult != null && queryResult.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            if (queryResult.Data.ip != lastIp)
                            {
                                lastIp = queryResult.Data.ip;
                                Console.WriteLine("New Ip Address: "+lastIp);
                                Console.WriteLine("VPN switched successfully.");
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Still connecting...");
                                tryCount += 1;
                            }
                        }
                    }

                    if (tryCount > 2)
                    {
                        Console.WriteLine($"Unable to connected with new server \"{randomServer}\". Trying another");
                        goto tryagain;
                    }
                    
                }
                catch
                {
                    Console.WriteLine($"Something happended in switching server \"{randomServer}\". Trying another");
                    goto tryagain;
                }
                return randomServer;
            }
            else
                throw new Exception("NordVPN is not running");
        }

        static void RunCommands(List<string> cmds, string workingDirectory = "")
        {
            var process = new Process();
            var psi = new ProcessStartInfo();
            psi.FileName = "cmd.exe";
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;
            psi.WorkingDirectory = workingDirectory;
            process.StartInfo = psi;
            process.Start();
            process.OutputDataReceived += (sender, e) => { Console.WriteLine(e.Data); };
            process.ErrorDataReceived += (sender, e) => { Console.WriteLine(e.Data); };
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            using (StreamWriter sw = process.StandardInput)
            {
                foreach (var cmd in cmds)
                {
                    sw.WriteLine(cmd);
                }
            }
            process.WaitForExit();
        }
    }

    #region IP check model
    public class Asn
    {
        public int asnum { get; set; }
        public string org_name { get; set; }
    }

    public class Geo
    {
        public string city { get; set; }
        public string region { get; set; }
        public string region_name { get; set; }
        public string postal_code { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string tz { get; set; }
        public string lum_city { get; set; }
        public string lum_region { get; set; }
    }

    public class IPRoot
    {
        public string ip { get; set; }
        public string country { get; set; }
        public Asn asn { get; set; }
        public Geo geo { get; set; }
    }

    #endregion

    #region NordVPN API Models
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
    #endregion
}
