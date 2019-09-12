using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using MediaShareBot.Extensions;

namespace MediaShareBot.Utilities {

    public static class Currency {

        private static ConcurrentDictionary<string, string> _CurrencyCodeMap = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        static Currency() => _CurrencyCodeMap = CultureInfo.GetCultures(CultureTypes.AllCultures)
                    .Where(c => !c.IsNeutralCulture)
                    .Select(culture => {
                        try {
                            return new RegionInfo(culture.LCID);
                        } catch {
                            return null;
                        }
                    })
                    .Where(x => x != null)
                    .GroupBy(x => x.ISOCurrencySymbol)
                    .ToConcurrentDictionary(x => x.Key, x => x.First().CurrencySymbol, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Get the currency symbol from iso country code.
        /// </summary>
        public static string GetSymbolFromCountryCode(string code) {
            if (_CurrencyCodeMap.ContainsKey(code)) {
                return _CurrencyCodeMap[code];
            } else {
                return "?";
            }
        }

    }

}
