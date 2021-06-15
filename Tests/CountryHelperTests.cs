using Awesome.XamarinHelpers.Countries;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class CountryHelperTests
    {
        [TestMethod]
        public void Alpha2To3Codes_ShouldNotBeNull()
        {
            // Arrange / Act
            var result = CountryHelper.Instance.Alpha2To3Codes;

            // Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void Alpha3To2Codes_ShouldNotBeNull()
        {
            // Arrange / Act
            var result = CountryHelper.Instance.Alpha3To2Codes;

            // Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void IsCode_ShouldBeTrue_WithValidAlpha2Code()
        {
            // Arrange / Act
            var result = CountryHelper.Instance.IsCode("Us");

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void IsCode_ShouldBeTrue_WithValidAlpha3Code()
        {
            // Arrange / Act
            var result = CountryHelper.Instance.IsCode("UsA");

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void IsCode_ShouldBeFalse_WithInvalidCode()
        {
            // Arrange / Act
            var result = CountryHelper.Instance.IsCode("SirJohnK");

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Name_ShouldBe_EnglishCountryName_WithValidCodeAndEnglishCulture()
        {
            // Arrange
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

            //Act
            var result = CountryHelper.Instance.Name("US");

            // Assert
            result.Should().Be("United States");
        }

        [TestMethod]
        public void Name_ShouldBe_SwedishCountryName_WithValidCodeAndSwedishCulture()
        {
            // Arrange
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("sv");

            //Act
            var result = CountryHelper.Instance.Name("US");

            // Assert
            result.Should().Be("USA");
        }

        [TestMethod]
        public void Name_ShouldBe_EnglishCountryName_WithValidCodeAndCultureMissingCountryNames()
        {
            // Arrange
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("xx");

            //Act
            var result = CountryHelper.Instance.Name("USA");

            // Assert
            result.Should().Be("United States", "because en culture is default for unknown cultures");
        }

        [TestMethod]
        public void Name_ShouldBeNull_WithInvalidCode()
        {
            // Arrange / Act
            var result = CountryHelper.Instance.Name("SirJohnK");

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void Image_ShouldBe_Alpha2CodeBasedFlagPngImageFileName_WithValidCode()
        {
            // Arrange / Act
            var result = CountryHelper.Instance.Image("USA");

            // Assert
            result.Should().Be("us-flag.png");
        }

        [TestMethod]
        public void Image_ShouldBeNull_WithInvalidCode()
        {
            // Arrange / Act
            var result = CountryHelper.Instance.Name("SirJohnK");

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void Country_ShouldBe_ValidCountryInfo_WithValidCode()
        {
            // Arrange
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

            // Act
            var result = CountryHelper.Instance.Country("US");

            // Assert
            result.Should().BeEquivalentTo(new CountryInfo() { Alpha2Code = "US", Alpha3Code = "USA", Image = "us-flag.png", Name = "United States" });
        }

        [TestMethod]
        public void Country_ShouldBeNull_WithInvalidCode()
        {
            // Arrange / Act
            var result = CountryHelper.Instance.Country("SirJohnK");

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void Country_ShouldBe_ValidCountryInfo_WithValidRegion()
        {
            // Arrange
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

            // Act
            var result = CountryHelper.Instance.Country(new RegionInfo("US"));

            // Assert
            result.Should().BeEquivalentTo(new CountryInfo() { Alpha2Code = "US", Alpha3Code = "USA", Image = "us-flag.png", Name = "United States" });
        }

        [TestMethod]
        public void Country_ShouldBe_ValidCountryInfo_WithValidNonNeutralCulture()
        {
            // Arrange
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

            // Act
            var result = CountryHelper.Instance.Country(CultureInfo.GetCultureInfo("en-US"));

            // Assert
            result.Should().BeEquivalentTo(new CountryInfo() { Alpha2Code = "US", Alpha3Code = "USA", Image = "us-flag.png", Name = "United States" });
        }

        [TestMethod]
        public void Country_ShouldBeNull_WithValidNeutralCulture()
        {
            // Arrange
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

            // Act
            var result = CountryHelper.Instance.Country(CultureInfo.GetCultureInfo("en"));

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void Countries_ShouldBe_ListOfValidCountryInfo_OrderedByCultureSpecificCountryName_WithEnglishCulture()
        {
            // Arrange
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

            // Act
            var result = CountryHelper.Instance.Countries();

            // Assert
            result.Should().HaveCount(249);
            result.ElementAt(234).Should().BeEquivalentTo(new CountryInfo() { Alpha2Code = "US", Alpha3Code = "USA", Image = "us-flag.png", Name = "United States" }, "because with english culture, US should be ordered at index 234");
        }

        [TestMethod]
        public void Countries_ShouldBe_ListOfValidCountryInfo_OrderedByCultureSpecificCountryName_WithSwedishCulture()
        {
            // Arrange
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("sv");

            // Act
            var result = CountryHelper.Instance.Countries();

            // Assert
            result.Should().HaveCount(249);
            result.ElementAt(238).Should().BeEquivalentTo(new CountryInfo() { Alpha2Code = "US", Alpha3Code = "USA", Image = "us-flag.png", Name = "USA" }, "because with swedish culture, US should be ordered at index 238");
        }

        [TestMethod]
        public void Countries_ShouldBe_ListOfValidCountryInfo_OrderedByCultureSpecificCountryName_WithCodeListAndEnglishCulture()
        {
            // Arrange
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

            // Act
            var result = CountryHelper.Instance.Countries(new List<string>() { "AT", "SE", "US" });

            // Assert
            result.Should().HaveCount(3);
            result.ElementAt(2).Should().BeEquivalentTo(new CountryInfo() { Alpha2Code = "US", Alpha3Code = "USA", Image = "us-flag.png", Name = "United States" }, "because with english culture, US should be ordered at index 2");
        }

        [TestMethod]
        public void Countries_ShouldBe_ListOfValidCountryInfo_OrderedByCultureSpecificCountryName_WithCodeListAnSwedishCulture()
        {
            // Arrange
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("sv");

            // Act
            var result = CountryHelper.Instance.Countries(new List<string>() { "AT", "SE", "US" });

            // Assert
            result.Should().HaveCount(3);
            result.ElementAt(1).Should().BeEquivalentTo(new CountryInfo() { Alpha2Code = "US", Alpha3Code = "USA", Image = "us-flag.png", Name = "USA" }, "because with swedish culture, US should be ordered at index 1");
        }
    }
}