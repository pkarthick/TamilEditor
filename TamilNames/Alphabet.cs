using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TamilExperiment
{
    public static class TamilProcessor
    {

        public static Dictionary<string, List<string>> TamilLifeLetters
        { get; set; }
        = new Dictionary<string, List<string>>
        {
            ["அ"] = new List<string> { "a" },
            ["ஆ"] = new List<string> { "aa" },
            ["இ"] = new List<string> { "i" },
            ["ஈ"] = new List<string> { "ee" },
            ["உ"] = new List<string> { "u" },
            ["ஊ"] = new List<string> { "oo" },
            ["எ"] = new List<string> { "e" },
            ["ஏ"] = new List<string> { "ae" },
            ["ஐ"] = new List<string> { "ai" },
            ["ஒ"] = new List<string> { "o" },
            ["ஓ"] = new List<string> { "O" },
            ["ஔ"] = new List<string> { "ow", "ou" },
            ["ஃ"] = new List<string> { "" },
        };

        public static Dictionary<string, string> TamilLifeLetterExtensions
        { get; set; }
        = new Dictionary<string, string>
        {
            ["அ"] = "",
            ["ஆ"] = "ா",
            ["இ"] = "ி",
            ["ஈ"] = "ீ",
            ["உ"] = "ு",
            ["ஊ"] = "ூ",
            ["எ"] = "ெ",
            ["ஏ"] = "ே",
            ["ஐ"] = "ை",
            ["ஒ"] = "ொ",
            ["ஓ"] = "ோ",
            ["ஔ"] = "ௌ",
            ["ஃ"] = "்",
        };

        public static Dictionary<string, List<string>> TamilBodyLetters
        { get; set; }
        = new Dictionary<string, List<string>>
        {
            ["க"] = new List<string> { "ka", "ga", "ca" },
            ["ங"] = new List<string> { "nga", "nGa"  },
            ["ச"] = new List<string> { "cha", "sa"  },
            ["ஞ"] = new List<string> { "nja", "nJa" },
            ["ட"] = new List<string> { "ta", "da" },
            ["ண"] = new List<string> { "Na" },
            ["த"] = new List<string> { "tha", "dha" },
            ["ந"] = new List<string> { "nha" },
            ["ப"] = new List<string> { "pa", "ba" },
            ["ம"] = new List<string> { "ma" },
            ["ய"] = new List<string> { "ya" },
            ["ர"] = new List<string> { "ra" },
            ["ல"] = new List<string> { "la" },
            ["வ"] = new List<string> { "va", "wa" },
            ["ழ"] = new List<string> { "za", "zha" },
            ["ள"] = new List<string> { "La" },
            ["ற"] = new List<string> { "Ra" },
            ["ன"] = new List<string> { "na" },
            ["ஜ"] = new List<string> { "ja" },
            ["ஷ"] = new List<string> { "sha" },
            ["ஸ"] = new List<string> { "Sa" },
            ["ஹ"] = new List<string> { "ha" },
            ["க்ஷ"] = new List<string> { "ksha" }
        };

        public static Dictionary<string, string> TamilToEnglishAlphabets
        { get; set; }
        = new Dictionary<string, string>
        {
            ["ஸ்ரீ"] = "sri",
            ["ஃ"] = "q"
        };

        public static Dictionary<string, string> EnglishToTamilAlphabets { get; set; } = new Dictionary<string, string>
        {
            ["sri"] = "ஸ்ரீ",
            ["q"] = "ஃ"
        };

        public static void Initialize()
        {
            try
            {
                foreach (var lifeLetter in TamilLifeLetters.Keys)
                {
                    if (TamilLifeLetters.ContainsKey(lifeLetter))
                    {
                        TamilToEnglishAlphabets[lifeLetter] = TamilLifeLetters[lifeLetter].First();

                        if(!string.IsNullOrEmpty(TamilLifeLetterExtensions[lifeLetter]))
                            TamilToEnglishAlphabets[TamilLifeLetterExtensions[lifeLetter]] = TamilToEnglishAlphabets[lifeLetter];
                    }

                    foreach (var bodyLetter in TamilBodyLetters.Keys)
                    {

                        TamilToEnglishAlphabets[bodyLetter] = TamilBodyLetters[bodyLetter].First();

                        foreach (var lifeLetterReadable in TamilLifeLetters[lifeLetter])
                        {
                            if (lifeLetter != TamilLifeLetters.Keys.Last())
                            {
                                EnglishToTamilAlphabets[lifeLetterReadable] = lifeLetter;

                            }
                            
                            var readableSuffixList = TamilBodyLetters[bodyLetter];

                            foreach (var readableSuffix in readableSuffixList)
                            {
                                string tamilLetter = bodyLetter + TamilLifeLetterExtensions[lifeLetter];
                                string englishReadable = readableSuffix.TrimEnd('a') + lifeLetterReadable;

                                EnglishToTamilAlphabets[englishReadable] = tamilLetter;
                            }
                        }

                    }

                }



            }
            catch (Exception ex)
            {

            }
        }

        private static string GetNative(string readable, bool isStart = false)
        {

            readable = readable.Trim();
            
            if (isStart)
            {
                readable = ProcessPrefix(readable);
            }
           
            if (EnglishToTamilAlphabets.ContainsKey(readable))
            {
                return EnglishToTamilAlphabets[readable];
            }

            if (EnglishToTamilAlphabets.ContainsKey(readable.ToLower()))
            {
                return EnglishToTamilAlphabets[readable.ToLower()];
            }

            return string.Empty;
        }

        private static string GetReadable(string native)
        {
            if (TamilToEnglishAlphabets.ContainsKey(native))
            {
                return TamilToEnglishAlphabets[native];
            }

            return string.Empty;

        }

        private static string ProcessNative(string readable)
        {
            if (readable.StartsWith("nth"))
                readable = "nhth" + readable.Substring(3);

            return readable;
        }

        private static string ProcessPrefix(string readable)
        {
            if (readable.StartsWith("ng"))
                readable = "nh" + readable.Substring(2);
            else if (readable.StartsWith("nG"))
                readable = "nh" + readable.Substring(2);
            else if (readable.StartsWith("NG"))
                readable = "nh" + readable.Substring(2);
            else if (readable.StartsWith("N"))
                readable = "nh" + readable.Substring(1);
            else if (readable.StartsWith("zh"))
                readable = "l" + readable.Substring(2);
            else if (readable.StartsWith("z"))
                readable = "l" + readable.Substring(1);
            else if (readable.StartsWith("L"))
                readable = "l" + readable.Substring(1);
            else if (readable.StartsWith("R"))
                readable = "r" + readable.Substring(1);
            else if (readable.StartsWith("ksh"))
                readable = "s" + readable.Substring(3);
            else if (readable.StartsWith("sh"))
                readable = "s" + readable.Substring(2);
            else if (readable.StartsWith("S"))
                readable = "s" + readable.Substring(1);
            
                return readable;
        }

        public static bool IsNative(string name)
        {
            if (name.ToCharArray().Any(c=> IsAlphabet(c)))
            {
                return false;
            }

            return true;

        }


        private static bool IsAlphabet(char ch)
        {
            var n = (int) ch;
            return (n >= 65 && n <= 90) || (n >= 97 && n <= 122);
        }

        public static string GetNative(string name)
        {
            StringBuilder sb = new StringBuilder();

            string[] fragments = name.Trim().Split(' ');

            foreach (var fragment in fragments)
            {
                var i = 0;
                while (i < fragment.Length)
                {
                    string unprocessedFragment = fragment.Substring(i);

                    for (var j = 5; j > 0; j--)
                    {

                        string alphaReadable = ProcessNative(unprocessedFragment);

                        if (unprocessedFragment.Length >= j)
                        {
                            alphaReadable = unprocessedFragment.Substring(0, j);
                        }

                        string alpha = GetNative(alphaReadable, i==0);

                        if (!string.IsNullOrEmpty(alpha))
                        {
                            sb.Append(alpha);
                            i += j;
                            break;
                        }
                        else if (alphaReadable.Length == 1)
                        {
                            sb.Append(alphaReadable);
                            i++;
                            break;
                        }


                    }

                }

                sb.Append(' ');

            }


            return sb.ToString().Trim();

        }

        public static string GetEnglish(string name)
        {
            StringBuilder sb = new StringBuilder();

            string[] fragments = name.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries );

            foreach (var fragment in fragments)
            {
                var i = 0;
                while (i < fragment.Length)
                {
                    int incr = 1;
                    string native = fragment.Substring(i, 1);

                    if (native == "ெ")
                    {
                        if (i + 1 < fragment.Length)
                        {
                            if (fragment.Substring(i + 1, 1) == "ா" || fragment.Substring(i + 1, 1) == "ௗ")
                            {
                                native = fragment.Substring(i, 2);
                                incr = 2;
                            }
                        }
                    }
                    else if (native == "ே")
                    {
                        if (i + 1 < fragment.Length)
                        {
                            if (fragment.Substring(i + 1, 1) == "ா")
                            {
                                native = fragment.Substring(i, 2);
                                incr = 2;
                            }
                        }
                    }

                    if (TamilLifeLetterExtensions.Values.Contains(native))
                        if (sb.Length > 0)
                        {
                            sb.Remove(sb.Length - 1, 1);
                        }

                    string alpha = GetReadable(native);

                    if (!string.IsNullOrEmpty(alpha))
                    {
                        sb.Append(alpha);
                        i+= incr;
                    }
                    else 
                    {
                        if(!IsNative(alpha))
                            sb.Append(native);
                        i+= incr;
                    }

                }

                sb.Append(' ');

            }


            return sb.ToString().Trim();

        }

    }
}
