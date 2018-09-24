using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CECS_378_Lab_1_Framework
{
    // Check solution against https://quipqiup.com/
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

        //-////////////////////////////////////////////////////////
        // 
        static void Main(string[] args)
        {
            /// Initialization
            // Init Word Checker
            wordDict = new NetSpell.SpellChecker.Dictionary.WordDictionary();
            wordDict.DictionaryFile = "en-US.dic";
            wordDict.Initialize();
            wordChecker = new NetSpell.SpellChecker.Spelling();
            wordChecker.Dictionary = wordDict;

            // Init character substitution map
            substitutionMap = new Dictionary<char, char>();

            /// Get Input
            string input = "fqjcb rwjwj vnjax bnkhj whxcq nawjv nfxdu mbvnu ujbbf nnc";
            Console.WriteLine(input);

            /// Evaluate Input Text
            // Find char frequency
            input = Regex.Replace(input, @"\s+", "");
            Console.WriteLine(input);
            char[] inputArr = input.ToCharArray();
            char[] inputFreq = GetCharFrequency(inputArr);
            //string[] inputWords = input.Split(' ');

            /// Optimization Techniques
            // - Character frequency analysis
            // - Word pattern analysis?

            // Create Substitution Map based on frequency
            for (int i = 0; i < inputFreq.Length; i++)
            {
                if (substitutionMap.ContainsKey(inputFreq[i]))
                {
                    substitutionMap[inputFreq[i]] = englishCharFrequency[i];
                }
                else
                {
                    substitutionMap.Add(inputFreq[i], englishCharFrequency[i]);
                }
            }
            //PrintSubstitutionMap(substitutionMap);
            var firstSubResults = ApplySubstitutions(inputArr, substitutionMap);
            Console.WriteLine(new string(firstSubResults));


            /// Brute Force
            // Modify the substitution table until we find a table that renders a correct sentence.
            // Parse the first ~5 char for an english word each cycle.


            //string inputString = "dumbdoorsaredumb";
            string inputString = "whatsinanamearosebyanyothernamewouldsmellassweet";
            ParseEnglishSentence(inputString);


            /// Complete Decryption Process
            // Print Map Results
            PrintSubstitutionMap(substitutionMap);

            // Finished. Keep Console Open.
            Console.Read();
        }

        //-////////////////////////////////////////////////////////
        // Gets an array of characters from the input ordered by frequency
        public static char[] GetCharFrequency(char[] input)
        {
            Dictionary<char, int> charFreq = new Dictionary<char, int>();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == ' ')
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
                index++;
            }
            return resultFreq;
        }

        //-////////////////////////////////////////////////////////
        // Replaces a char with another in an array.
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

        //-////////////////////////////////////////////////////////
        // Applies a substitution to an array of characters.
        public static char[] ApplySubstitutions(char[] input, Dictionary<char, char> subMap)
        {
            List<char> keys = new List<char>(subMap.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                ReplaceChar(ref input, keys[i], subMap[keys[i]]);
            }
            return input;
        }


        //-////////////////////////////////////////////////////////
        //
        public static bool BruteForceSubstitution(char[] inputWord, Dictionary<char, char> subMap)
        {


            return false;
        }

        //-////////////////////////////////////////////////////////
        // Trys to find an english word while 
        public static char[] TryFindWord(ref char[] sourceArr, int startIndex=0, int sizeLimit=5, int sizeMin=2)
        {
            char[] testWord;
            int size = Math.Max(1, sizeMin);
            while(size <= sizeLimit)
            {
                int s = size;// Math.Min(size, Math.Min(sourceArr.Length - startIndex, 0));
                //Console.WriteLine((s).ToString());
                if (s > 0)
                {
                    testWord = new char[s];
                    for (int i = 0; i < s; i++)
                    {
                        //Console.WriteLine((startIndex + i).ToString());
                        testWord[i] = sourceArr[startIndex + i];
                    }
                    Console.WriteLine("Testing:\t" + new string(testWord));
                    if (testWord.Length == 1 && !(testWord[0] == 'a' || testWord[0] == 'A'))
                    {
                        // Single character word that isn't 'a'.
                    }
                    else if (wordChecker.TestWord(new string(testWord)))
                    {
                        Console.WriteLine("FOUND WORD:\t" + new string(testWord));
                        
                        return testWord; 
                    }
                }
                size++;
            }
            return new char[]{ };
        }

        //-////////////////////////////////////////////////////////
        // Finds a valid English sentence in the input string.
        private static bool ParseEnglishSentence(string inputString)
        {
            Console.WriteLine("INPUT:\t" + inputString);
            char[] inputArr = inputString.ToCharArray();
            int foundCharCount = 0;
            int minSize = 1;
            List<char[]> foundWords = new List<char[]>();
            while (foundCharCount < inputString.Length)
            {
                int startIndex = Math.Max(foundCharCount, 0);
                int maxSize = inputString.Length - foundCharCount;
                Console.WriteLine(startIndex + ", " + maxSize + ", " + minSize);
                char[] foundWord = TryFindWord(ref inputArr, startIndex, maxSize, minSize);
                if (foundWord.Length > 0)
                {
                    foundWords.Add(foundWord);
                    minSize = 1; // Reset the minSize to 1. Finishes the backtracking cycle.
                    // Note: This might cause a bad loop, and could get us stuck with 3 or more backtracks. Need to manage the variable better.
                    Console.WriteLine("Added:\t"+ new string(foundWord));
                }
                else if (foundWords.Count >= 1)
                {
                    Console.WriteLine("Couldnt find word. Backtracking...");
                    minSize = foundWords[foundWords.Count - 1].Length + 1;
                    foundWords.RemoveAt(foundWords.Count - 1);
                }
                else
                {
                    // Note: One typo can "destroy" the whole sentence due to backtracking methods. 
                    // There needs to be a limit on backtracking, and we should capture "typo" words.
                    Console.WriteLine("Couldnt find ANY words!");
                    return false;
                }

                foundCharCount = 0;
                for (int i = 0; i < foundWords.Count; i++)
                {
                    foundCharCount += foundWords[i].Length;
                }
            }
            Console.WriteLine();
            foreach (var word in foundWords)
            {
                Console.Write(new string(word) + " ");
            }
            Console.WriteLine();
            return true;
        }

        //-////////////////////////////////////////////////////////
        //
        public static void PrintSubstitutionMap(Dictionary<char, char> map)
        {
            Console.WriteLine("\nMAP RESULTS ================");
            foreach (KeyValuePair<char, char> subPair in map)
            {
                Console.WriteLine(subPair.Key + " = " + subPair.Value);
            }
            Console.WriteLine("============================");
        }


        #region Unused

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

        #endregion
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

/*
    // Check if any words are already correct. 
    // We might be able to quickly find correctly mapped characters.
    for (int i = 0; i < inputWords.Length; i++)
    {
        if (wordChecker.TestWord(inputWords[i]))
        {
            Console.WriteLine("ALREADY CORRECT: " + inputWords[i]);
        }
    }

    //string[] words = input.Split(' ');
    //char[] arr = inputWords[0].ToCharArray();

    //arr = ApplySubstitutions(arr, substitutionMap);
    //if (wordChecker.TestWord(new string(arr)))
    //{
    //    for (int i = 0; i < arr.Length; i++)
    //    {
    //        char c = arr[i];
    //        if(substitutionMap.ContainsKey(c))
    //        {
    //            //substitutionMap[c] ==
    //        }
    //    }
    //}


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


*/
