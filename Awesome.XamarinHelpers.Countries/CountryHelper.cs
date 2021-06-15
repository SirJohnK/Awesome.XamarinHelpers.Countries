using Awesome.XamarinHelpers.Countries.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Awesome.XamarinHelpers.Countries
{
    public sealed class CountryHelper
    {
        private static readonly Lazy<CountryHelper> instance = new Lazy<CountryHelper>(() => new CountryHelper());

        public static CountryHelper Instance => instance.Value;
        public ReadOnlyDictionary<string, string>? Alpha2To3Codes { get; set; }
        public ReadOnlyDictionary<string, string>? Alpha3To2Codes { get; set; }
        public IEnumerable<string>? Alpha2Codes => Alpha2To3Codes?.Keys;
        public IEnumerable<string>? Alpha3Codes => Alpha3To2Codes?.Keys;

        public string? Alpha2Code(string code)
        {
            //Attempt to get Alpha2 code
            code = code.Trim().ToUpper();
            if (Alpha2Codes?.Contains(code, StringComparer.InvariantCultureIgnoreCase) ?? false) return code;
            if (Alpha3To2Codes?.ContainsKey(code) ?? false) return Alpha3To2Codes[code];
            return default;
        }

        public string? Alpha3Code(string code)
        {
            //Attempt to get Alpha3 code
            code = code.Trim().ToUpper();
            if (Alpha3Codes?.Contains(code, StringComparer.InvariantCultureIgnoreCase) ?? false) return code;
            if (Alpha2To3Codes?.ContainsKey(code) ?? false) return Alpha2To3Codes[code];
            return default;
        }

        public bool IsCode(string code) => !string.IsNullOrEmpty(Alpha2Code(code) ?? Alpha3Code(code));

        public string? Name(string code) => IsCode(code) ? CountryNames.ResourceManager.GetString(Alpha2Code(code)) : default;

        public string? Image(string code) => IsCode(code) ? $"{Alpha2Code(code)?.ToLower()}-flag.png" : default;

        public CountryInfo? Country(string code) => IsCode(code) ? new CountryInfo() { Alpha2Code = Alpha2Code(code), Alpha3Code = Alpha3Code(code), Name = Name(code), Image = Image(code) } : default;

        public CountryInfo? Country(RegionInfo region) => Country(region.TwoLetterISORegionName);

        public CountryInfo? Country(CultureInfo culture) => !culture.IsNeutralCulture ? Country(new RegionInfo(culture.LCID)) : default;

        public IEnumerable<CountryInfo?> Countries(IEnumerable<string> codes) => codes.Where(code => IsCode(code)).Select(code => Country(code)).OrderBy(country => country?.Name);

        public IEnumerable<CountryInfo?> Countries() => Alpha2Codes.Select(code => Country(code)).OrderBy(country => country?.Name);

        private CountryHelper()
        {
            //Init
            var thisNameSpace = this.GetType().Namespace;
            var thisAssembly = typeof(CountryHelper).Assembly;
            var resourceStream = thisAssembly.GetManifestResourceStream($"{thisNameSpace}.CountryCodes.json");

            //Read Country Codes
            using var resourceReader = new StreamReader(resourceStream);
            var countryCodes = JsonSerializer.Deserialize<Dictionary<string, string>>(resourceReader.ReadToEnd());

            //Build Code Dictionaries
            Alpha2To3Codes = new ReadOnlyDictionary<string, string>(countryCodes);
            Alpha3To2Codes = new ReadOnlyDictionary<string, string>(countryCodes.ToDictionary(code => code.Value, code => code.Key));
        }
    }
}