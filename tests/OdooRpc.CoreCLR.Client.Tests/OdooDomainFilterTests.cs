using System;
using OdooRpc.CoreCLR.Client.Models;
using Xunit;

namespace OdooRpc.CoreCLR.Client.Tests
{
    public class OdooDomainFilterTests
    {
        [Fact]
        public void ToFilterArray_ShouldReturnAnObjectArray()
        {
            OdooDomainFilter testDomain = new OdooDomainFilter();

            Assert.IsType<object[]>(testDomain.ToFilterArray());
        }

        [Fact]
        public void Filter_ShouldReturnOdooDomainFilterInstance()
        {
            OdooDomainFilter testDomain = new OdooDomainFilter();

            Assert.IsType<OdooDomainFilter>(testDomain.Filter("is_company", "=", true));
        }

        [Fact]
        public void Filter_ShouldAddCriteriaToFilter()
        {
            OdooDomainFilter testDomain = new OdooDomainFilter()
                .Filter("is_company", "=", true)
                .Filter("customer", "!=", false);

            Assert.Equal(new object[] {
                    new object[] { "is_company", "=", true },
                    new object[] { "customer", "!=", false }
                },
                testDomain.ToFilterArray()
            );
        }

        [Fact]
        public void And_ShouldReturnOdooDomainFilterInstance()
        {
            OdooDomainFilter testDomain = new OdooDomainFilter();

            Assert.IsType<OdooDomainFilter>(testDomain.And());
        }

        [Fact]
        public void And_ShouldAddLogicalAndInPrefixFormToFilter()
        {
            OdooDomainFilter testDomain = new OdooDomainFilter()
                .Filter("id", ">", 1000)
                .And()
                .Filter("is_company", "=", true)
                .Filter("customer", "!=", false);

            Assert.Equal(new object[] {
                    new object[] { "id", ">", 1000 },
                    "&",
                    new object[] { "is_company", "=", true },
                    new object[] { "customer", "!=", false }
                },
                testDomain.ToFilterArray()
            );
        }

        [Fact]
        public void Or_ShouldReturnOdooDomainFilterInstance()
        {
            OdooDomainFilter testDomain = new OdooDomainFilter();

            Assert.IsType<OdooDomainFilter>(testDomain.Or());
        }

        [Fact]
        public void Or_ShouldAddLogicalOrInPrefixFormToFilter()
        {
            OdooDomainFilter testDomain = new OdooDomainFilter()
                .Filter("id", ">", 1000)
                .Or()
                .Filter("is_company", "=", true)
                .Filter("customer", "!=", false);

            Assert.Equal(new object[] {
                    new object[] { "id", ">", 1000 },
                    "|",
                    new object[] { "is_company", "=", true },
                    new object[] { "customer", "!=", false }
                },
                testDomain.ToFilterArray()
            );
        }

        [Fact]
        public void ToFilterArray_ShouldThrowExceptionIfFilterIsIncomplete()
        {
            OdooDomainFilter testDomain = new OdooDomainFilter()
                .Filter("id", ">", 1000)
                .And()
                .Filter("is_company", "=", true)
                .Or()
                .Filter("customer", "!=", false);

            Assert.Throws<InvalidOperationException>(() => testDomain.ToFilterArray());
        }

        [Fact]
        public void ToFilterArray_ShouldSupportMultipleFilterCriteria()
        {
            OdooDomainFilter testDomain = new OdooDomainFilter()
                .Filter("name", "=", "ABC")
                .Filter("language", "!=", "en_US")
                .Or()
                .Filter("country_id.code", "=", "be")
                .And()
                .Filter("country_id.code", "=", "de")
                .Filter("language", "=", "de_DE");

            Assert.Equal(new object[] {
                    new object[] { "name", "=", "ABC" },
                    new object[] { "language", "!=", "en_US" },
                    "|",
                    new object[] { "country_id.code", "=", "be" },
                    "&",
                    new object[] { "country_id.code", "=", "de" },
                    new object[] { "language", "=", "de_DE" }
                },
                testDomain.ToFilterArray()
            );
        }

    }
}