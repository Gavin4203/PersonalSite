using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GavinHsSecurity
{
    public class Sanitization
    {
        private static readonly Regex htmlTagPattern = new(@"<[^>]*>", RegexOptions.Compiled);

        public static string StripHtml(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Remove tags
            string noTags = htmlTagPattern.Replace(input, string.Empty);
            // Optionally, decode any remaining HTML entities like &lt; &gt;
            return System.Net.WebUtility.HtmlEncode(noTags);
        }
    }
}
