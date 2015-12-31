using OdooRpc.CoreCLR.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OdooRpc.CoreCLR.Client.Tests
{
    public class OdooSessionInfoTests
    {
        OdooConnectionInfo ConnectionInfo;
        OdooSessionInfo SessionInfo;

        public OdooSessionInfoTests()
        {
            this.ConnectionInfo = new OdooConnectionInfo()
            {
                IsSSL = false,
                Host = "test.odoo",
                Port = 1234,
                Username = "aaa",
                Password = "bbb"
            };

            this.SessionInfo = new OdooSessionInfo(ConnectionInfo);
        }

        [Fact]
        public void Constructor_ShouldInitWithUserLoggedOut()
        {
            Assert.False(SessionInfo.IsLoggedIn);
        }

        [Fact]
        public void Constructor_ShouldInitWithUserContext()
        {
            AssertIsDefaultUserContext();
        }

        [Fact]
        public void Reset_ShouldLogoutUser()
        {
            SessionInfo.IsLoggedIn = true;
            SessionInfo.Reset();

            Assert.False(SessionInfo.IsLoggedIn);
        }

        [Fact]
        public void Reset_ShouldResetUserContext()
        {
            SessionInfo.UserContext.Language = "pt_BR";
            SessionInfo.Reset();

            AssertIsDefaultUserContext();
        }

        private void AssertIsDefaultUserContext()
        {
            Assert.NotNull(SessionInfo.UserContext);
            Assert.Null(SessionInfo.UserContext.Language);
            Assert.Null(SessionInfo.UserContext.Timezone);
        }
    }
}
