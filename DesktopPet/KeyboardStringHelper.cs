using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPet;

internal static class KeyboardStringHelper
{
    private const string FROM_RUSSIAN = "йцукенгшщзхъфывапролджэячсмитьбюё";
    private const string TO_RUSSIAN =   "qwertyuiop[]asdfghjkl;'zxcvbnm,.`";
    private const string FROM_SHIFT_KEYS = "~!@#$%^&*()_+{}:\"<>?";
    private const string TO_SHIFT_KEYS =   "`1234567890-=[];',./";
    private static readonly Dictionary<Keys, string> keyStringPairs = new()
    {
        {Keys.D1,"1" },
        {Keys.D2,"2"},
        {Keys.D3,"3"},
        {Keys.D4,"4"},
        {Keys.D5,"5"},
        {Keys.D6,"6"},
        {Keys.D7,"7"},
        {Keys.D8,"8"},
        {Keys.D9,"9"},
        {Keys.D0,"0"},
        {Keys.OemMinus,"-"},
        {Keys.Oemplus,"="},
        {Keys.Space," "},
        {Keys.Oem4,"[" },
        {Keys.Oem6,"]"},
        {Keys.OemSemicolon,";"},
        {Keys.Oem7,"'" },
        {Keys.Oemcomma,","},
        {Keys.OemPeriod,"."},
        {Keys.Oem3,"`"}
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
    public static string FromShiftKeys(this string text)
    {
        StringBuilder builder = new StringBuilder(text);
        for (int i = 0; i < FROM_SHIFT_KEYS.Length; i++)
        {
            builder.Replace(FROM_SHIFT_KEYS[i], TO_SHIFT_KEYS[i]);
        }
        return builder.ToString();
    }
    public static string ToKeyString(this Keys key)
    {
        if (keyStringPairs.TryGetValue(key, out var result))
            return result;
        else if(key.ToString().Length == 1)
            return key.ToString().ToLower();
        else
            return string.Empty;
    }
}
