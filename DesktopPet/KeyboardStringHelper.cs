using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPet;

internal static class KeyboardStringHelper
{
    private const string FROM_RUSSIAN = "йцукенгшщзхъфывапролджэячсмитьбюё";
    private const string TO_RUSSIAN = "qwertyuiop[]asdfghjkl;'zxcvbnm,.`";
    private static readonly Dictionary<string, string> textKeyPairs = new (){
        {"[","oem4" },
        {"]","oem6" },
        {";","oemsemicolon" },
        {"'","oem7" },
        {",","oemcomma" },
        {".","oemperiod" },
        {"`","oem3" },
        {" ","space" }
    };
    public static string FromRussian(this string russian)
    {
        StringBuilder builder = new StringBuilder(russian);
        for (int i = 0; i < FROM_RUSSIAN.Length; i++)
        {
            builder.Replace(FROM_RUSSIAN[i], TO_RUSSIAN[i]);
        }
        return builder.ToString();
    }
    public static string ToKeys(this string text)
    {
        StringBuilder builder = new StringBuilder(text);
        foreach (var pair in textKeyPairs)
        {
            builder.Replace(pair.Key,pair.Value);
        }
        return builder.ToString();
    }
}
