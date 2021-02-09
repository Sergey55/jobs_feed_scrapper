using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsFeedScrapper.Service
{
    public static class ContentHelper
    {
        public static string ReplaceSpecialSymbols(string content)
        { 
            return content
                .Replace("<br />", "\n")
                .Replace("&nbsp;", " ")
                .Replace("&bull;", "•")
                .Replace("&auml;", "ä")
                .Replace("&lsquo;", "'")
                .Replace("&rsquo;", "'")
                .Replace("&ntilde;", "ñ")
                .Replace("&amp;", "&")
                .Replace("&gt;", ">")
                .Replace("&lt;", "<")
                .Replace("&szlig;", "ß")
                .Replace("&ndash;", "-");
        }
    }
}
