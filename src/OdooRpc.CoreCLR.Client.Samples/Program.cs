using System;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using OdooRpc.CoreCLR.Client.Interfaces;
using OdooRpc.CoreCLR.Client.Models;

namespace OdooRpc.CoreCLR.Client.Samples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            var p = new Program();
            p.LoginToOdoo().Wait();
            
            Console.WriteLine("Done! Press a key to exit...");
            Console.ReadKey();
        }

        private OdooConnectionInfo OdooConnection;

        public Program()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            try
            {
                var settings = JsonConvert.DeserializeObject<JObject>(File.ReadAllText("appsettings.json"));
                this.OdooConnection = settings["OdooConnection"].ToObject<OdooConnectionInfo>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading app settings: {0}", ex.Message);
            }
        }

        public async Task LoginToOdoo()
        {
            try
            {
                var OdooRpc = new OdooRpcClient();
                await OdooRpc.Connect(this.OdooConnection);

                if (OdooRpc.SessionInfo.IsLoggedIn)
                {
                    Console.WriteLine("Login successful");
                }
                else
                {
                    Console.WriteLine("Login failed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting to Odoo: {0}", ex.Message);
            }
        }
    }
}