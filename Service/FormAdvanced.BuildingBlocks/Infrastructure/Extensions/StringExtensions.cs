using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormAdvanced.BuildingBlocks.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static IEnumerable<string> SplitPascalCaseWord(this string word)
        {
            //Regex that returns a group for every occurrence of a word in a pascal case string (e.g. => HelloWorld -> [Hello, World])
            var regex = new Regex("([A-Z]+[^A-Z]+)");

            return regex.Matches(word).Select(c => c.Value);
        }

        public static string GetFirstWordOfPascalCase(this string word)
        {
            return word.SplitPascalCaseWord().FirstOrDefault();
        }
    }
}
