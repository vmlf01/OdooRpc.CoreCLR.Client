using OdooRpc.CoreCLR.Client.Internals;
using OdooRpc.CoreCLR.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OdooRpc.CoreCLR.Client.Tests
{
    public class OdooEndpointsTests
    {
        public class HttpEndpoint
        {
            OdooConnectionInfo connectionInfo;

            public HttpEndpoint()
            {
                this.connectionInfo = new OdooConnectionInfo()
                {
                    IsSSL = false,
                    Host = "test.odoo",
                    Port = 1234,
                    Username = "aaa",
                    Password = "bbb"
                };
            }

            [Fact]
            public void ShouldCreateAuthenticationHttpUri()
            {
                var uri = OdooEndpoints.GetAuthenticationUri(this.connectionInfo);
                Assert.Equal("http://test.odoo:1234/web/session/authenticate", uri.ToString());
            }
        }
        public class HttpsEndpoint
        {
            OdooConnectionInfo connectionInfo;

            public HttpsEndpoint()
            {
                this.connectionInfo = new OdooConnectionInfo()
                {
                    IsSSL = true,
                    Host = "test.odoo",
                    Port = 1234,
                    Username = "aaa",
                    Password = "bbb"
                };
            }

            [Fact]
            public void ShouldCreateAuthenticationHttpsUri()
            {
                var uri = OdooEndpoints.GetAuthenticationUri(this.connectionInfo);
                Assert.Equal("https://test.odoo:1234/web/session/authenticate", uri.ToString());
            }
        }
    }
}
