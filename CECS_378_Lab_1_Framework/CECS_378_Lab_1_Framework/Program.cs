using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CECS_378_Lab_1_Framework
{
    class Program
    {
        // Word Checker from Third-Party NuGet Package with English Dictionary
        public static NetSpell.SpellChecker.Dictionary.WordDictionary wordDict;
        public static NetSpell.SpellChecker.Spelling wordChecker;

        // Frequency of each character in english, in order of most common to least.
        public static char[] englishCharFrequency = 
            {   'e', 't', 'a', 'o', 'i',
                'n', 's', 'r', 'h', 'd',
                'l', 'u', 'c', 'm', 'f',
                'y', 'w', 'g', 'p', 'b',
                'v', 'k', 'x', 'q', 'j', 'z' };

        // Characters we know are substituted.
        public static Dictionary<char, char> substitutionMap;

        static void Main(string[] args)
        {
            /// Initialization
            // Init Word Checker
            wordDict = new NetSpell.SpellChecker.Dictionary.WordDictionary();
            wordDict.DictionaryFile = "en-US.dic";
            wordDict.Initialize();
            wordChecker = new NetSpell.SpellChecker.Spelling();
            wordChecker.Dictionary = wordDict;

            // Init character map
            substitutionMap = new Dictionary<char, char>();

            /// Get Input
            string input = "fqjcb rwjwj vnjax bnkhj whxcq nawjv nfxdu mbvnu ujbbf nnc";
            Console.WriteLine(input);

            /// Break Up Source Text
            // Find char frequency
            // Find smallest words(?)
            char[] inputArr = input.ToCharArray();
            char[] inputFreq = GetCharFrequency(inputArr);

            /// Optimization Techniques.
            // First, find the most frequent character, and replace 'E'.
            // Repeat this for a character frequency table.

            // Create Substitution Map.
            for (int i = 0; i < inputFreq.Length; i++)
            {
                if(substitutionMap.ContainsKey(inputFreq[i]))
                {
                    substitutionMap[inputFreq[i]] = englishCharFrequency[i];
                }
                else
                {
                    substitutionMap.Add(inputFreq[i], englishCharFrequency[i]);
                }
            }
            PrintSubstitutionMap(substitutionMap);

            var firstSubResults = ApplySubstitutions(inputArr, substitutionMap);
            Console.WriteLine(new string(firstSubResults));

            /// Brute Force
            // Just loop through all of them until a word is found.
            // Modify the substitution table until we find a correct word.

            string[] words = input.Split(' ');
            char[] arr = words[0].ToCharArray();

            arr = ApplySubstitutions(arr, substitutionMap);
            if (wordChecker.TestWord(new string(arr)))
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    char c = arr[i];
                    if(substitutionMap.ContainsKey(c))
                    {
                        //substitutionMap[c] ==
                    }
                }
            }


            //// First check if any words are already correct. We might be able to find correctly mapped characters.
            //for (int i = 0; i < words.Length; i++)
            //{
            //    if (wordChecker.TestWord(words[i]))
            //    {
            //        Console.WriteLine(words[i]);
            //    }
            //}

            //string sourceWord = words[0];
            //char[] arr = sourceWord.ToCharArray();
            //BruteForce(ref arr, 0);



            // Print Map Results
            PrintSubstitutionMap(substitutionMap);

            // Finished. Keep Console Open.
            Console.Read();
        }

        ///-////////////////////////////
        /// Replaces a char with another in an array.
        public static void ReplaceChar(ref char[] source, char cFrom, char cTo)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if(source[i]== cFrom)
                {
                    source[i] = cTo;
                }
            }
        }

        public static void PrintSubstitutionMap(Dictionary<char, char> map)
        {
            Console.WriteLine("\nMAP RESULTS ================");
            foreach (KeyValuePair<char, char> subPair in map)
            {
                Console.WriteLine(subPair.Key + " = " + subPair.Value);
            }
            Console.WriteLine("============================");
        }

        ///-////////////////////////////
        /// Gets an array of characters from the input ordered by frequency
        public static char[] GetCharFrequency(char[] input)
        {
            Dictionary<char, int> charFreq = new Dictionary<char, int>();
            for (int i = 0; i < input.Length; i++)
            {
                if(input[i] == ' ')
                {
                    continue;
                }
                if (charFreq.ContainsKey(input[i]))
                {
                    charFreq[input[i]]++;
                }
                else
                {
                    charFreq.Add(input[i], 1);
                }
            }
            char[] resultFreq = new char[charFreq.Count];
            var ordered = charFreq.OrderByDescending(x => x.Value);
            int index = 0;
            foreach (KeyValuePair<char, int> pair in ordered)
            {
                resultFreq[index] = pair.Key;
                //Console.WriteLine(resultFreq[index]);
                index++;
            }
            return resultFreq;
        }
        

        public static bool SubBruteForce(char[] inputWord, Dictionary<char, char> subMap)
        {


            return false;
        }

        ///-////////////////////////////
        /// Applies a substitution to an array of characters.
        public static char[] ApplySubstitutions(char[] input, Dictionary<char, char> subMap)
        {
            List<char> keys = new List<char>(subMap.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                ReplaceChar(ref input, keys[i], subMap[keys[i]]);
            }
            return input;
        }



        public static bool BruteForce(ref char[] word, int charIndex = 0)
        {
            if (charIndex >= word.Length)
            {
                Console.WriteLine("----");
                return false;
            }
            for (int i = 0; i < englishCharFrequency.Length; i++)
            {
                ReplaceChar(ref word, word[charIndex], englishCharFrequency[i]);
                Console.WriteLine(new string(word));
                if (wordChecker.TestWord(new string(word)) == true)
                {
                    return true;
                }
                else
                {
                    if (BruteForce(ref word, charIndex + 1))
                    {
                        return true;
                    }
                }

            }

            return false;
        }
    }
}


//for (int i = 0; i < words.Length; i++)
//{
//    string sourceWord = words[i];
//    char[] arr = sourceWord.ToCharArray();
//    // Apply 

//    while (wordChecker.TestWord(sourceWord) == false)
//    {
//        //ReplaceChar(arr)
//        sourceWord = new string(arr);
//    }

//}