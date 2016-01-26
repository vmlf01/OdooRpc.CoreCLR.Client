using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using OdooRpc.CoreCLR.Client.Interfaces;
using OdooRpc.CoreCLR.Client.Models;
using OdooRpc.CoreCLR.Client.Models.Parameters;

namespace OdooRpc.CoreCLR.Client.Samples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            var p = new Program();
            p.LoginToOdoo().Wait();
            //p.GetDepartments().Wait();
            //p.SearchDepartments().Wait();
            //p.GetDepartmentsFields().Wait();
            //p.GetAllDepartments().Wait();
            //p.CreateDeleteDepartment().Wait();
            p.GetMetadata().Wait();
            Console.WriteLine("Done! Press a key to exit...");
            Console.ReadKey();
        }

        private OdooConnectionInfo OdooConnection;
        private IOdooRpcClient OdooRpcClient;

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
                this.OdooRpcClient = new OdooRpcClient(this.OdooConnection);

                var odooVersion = await this.OdooRpcClient.GetOdooVersion();

                Console.WriteLine("Odoo Version: {0} - {1}", odooVersion.ServerVersion, odooVersion.ProtocolVersion);

                await this.OdooRpcClient.Authenticate();

                if (this.OdooRpcClient.SessionInfo.IsLoggedIn)
                {
                    Console.WriteLine("Login successful => User Id: {0}", this.OdooRpcClient.SessionInfo.UserId);
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

        public async Task GetAllDepartments()
        {
            try
            {
                var fieldParams = new OdooFieldParameters();
                fieldParams.Add("name");
                fieldParams.Add("company_id");
                fieldParams.Add("color");

                var departments = await this.OdooRpcClient.GetAll<JObject[]>("hr.department", fieldParams, new OdooPaginationParameters().OrderByDescending("name"));

                Console.WriteLine(departments.FirstOrDefault());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting departments from Odoo: {0}", ex.Message);
            }
        }

        public async Task GetDepartments()
        {
            try
            {
                var reqParams = new OdooGetParameters("hr.department");
                reqParams.Ids.Add(6);
                //reqParams.Ids.Add(7);

                var fieldParams = new OdooFieldParameters();
                fieldParams.Add("name");
                fieldParams.Add("company_id");

                var departments = await this.OdooRpcClient.Get<JObject[]>(reqParams, fieldParams);

                Console.WriteLine(departments.FirstOrDefault());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting departments from Odoo: {0}", ex.Message);
            }
        }

        public async Task SearchDepartments()
        {
            try
            {
                var reqParams = new OdooSearchParameters(
                    "hr.department", 
                    new OdooDomainFilter().Filter("name", "like", "SIC")
                );

                var count = await this.OdooRpcClient.SearchCount(reqParams);
                var departments = await this.OdooRpcClient.Search<long[]>(reqParams, new OdooPaginationParameters(0, 1));

                Console.WriteLine(departments.FirstOrDefault());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting departments from Odoo: {0}", ex.Message);
            }
        }

        public async Task GetDepartmentsFields()
        {
            try
            {
                var reqParams = new OdooGetModelFieldsParameters(
                    "hr.department"
                );

                var fields = await this.OdooRpcClient.GetModelFields<dynamic>(reqParams);

                fields.ToList().ForEach(f => Console.WriteLine(f));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting partners from Odoo: {0}", ex.Message);
            }
        }

        public async Task CreateDeleteDepartment()
        {
            try
            {
                var id = await this.OdooRpcClient.Create<dynamic>("hr.department", new
                {
                    name = "test"
                });

                Console.WriteLine(id);

                await this.OdooRpcClient.Delete("hr.department", id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting partners from Odoo: {0}", ex.Message);
            }
        }

        public async Task GetMetadata()
        {
            try
            {
                var metaParams = new OdooMetadataParameters("res.groups", new System.Collections.Generic.List<long>() { 4 });

                var resp = await this.OdooRpcClient.GetMetadata<JObject[]>(metaParams);

                Console.WriteLine(resp.FirstOrDefault());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting metadata from Odoo: {0}", ex.Message);
            }
        }

    }
}