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
        // Alphabet in order
        public static char[] englishAlphabet =
            {   'a', 'b', 'c', 'd', 'e',
                'f', 'g', 'h', 'i', 'j',
                'k', 'l', 'm', 'n', 'o',
                'p', 'q', 'r', 's', 't',
                'u', 'v', 'w', 'x', 'y', 'z' };

        // Characters we know are substituted.
        public static Dictionary<char, char> substitutionMap;

        public static string[] inputStrings =
        {
            "fqjcb rwjwj vnjax bnkhj whxcq nawjv nfxdu mbvnu ujbbf nnc",
            "oczmz vmzor jocdi bnojv dhvod igdaz admno ojbzo rcvot jprvi oviyv aozmo cvooj ziejt dojig toczr dnzno jahvi fdiyv xcdzq zoczn zxjiy",
            "ejitp spawa qleji taiul rtwll rflrl laoat wsqqj atgac kthls iraoa twlpl qjatw jufrh lhuts qataq itats aittk stqfj cae",
            "iyhqz ewqin azqej shayz niqbe aheum hnmnj jaqii yuexq ayqkn jbeuq iihed yzhni ifnun sayiz yudhe sqshu qesqa iluym qkque aqaqm oejjs hqzyu jdzqa diesh niznj jayzy uiqhq vayzq shsnj jejjz nshna hnmyt isnae sqfun dqzew qiead zevqi zhnjq shqze udqai jrmtq uishq ifnun siiqa suoij qqfni syyle iszhn bhmei squih nimnx hsead shqmr udquq uaqeu iisqe jshnj oihyy snaxs hqihe lsilu ymhni tyz"
        };

        //-////////////////////////////////////////////////////////
        // 
        static void Main(string[] args)
        {
            //char[] arr = { 'a', 'b', 'c' };
            //GetPer2(arr, (char[] result) =>
            //{
            //    Console.WriteLine(result);
            //    if(new string(result) == "bca")
            //    {
            //        Console.WriteLine("Found it!");
            //        return true;
            //    }
            //    return false;
            //});
            //return;

            /// Initialization
            // Init Word Checker
            wordDict = new NetSpell.SpellChecker.Dictionary.WordDictionary();
            wordDict.DictionaryFile = "en-US.dic";
            wordDict.Initialize();
            wordChecker = new NetSpell.SpellChecker.Spelling();
            wordChecker.Dictionary = wordDict;

            //string input = "fqjcb rwjwj vnjax bnkhj whxcq nawjv nfxdu mbvnu ujbbf nnc";
            //string input = "fqjcbrwjwjvnjaxbnkhjwhxcqnawjvnfxdumbvnuujbbfnnc";
            string input = inputStrings[2];
            Console.WriteLine("==============================");
            Console.WriteLine(input);

            bool success = Decrypt(input);

            /// Complete Decryption Process
            // Print Map Results
            PrintSubstitutionMap(substitutionMap);
            // Finished. Keep Console Open.
            Console.Read();
        }

        public static bool Decrypt(string input)
        {
            bool success = false;
            // Clean Input of whitespaces
            //input = Regex.Replace(input, @"\s+", "");
            input = new string(input.Where(c => !char.IsWhiteSpace(c)).ToArray());

            // Check if phrase is already decrypted
            //success = ParseEnglishSentence(input);

            // Check Shift cipher
            if (!success)
            {
                Console.WriteLine("ATTEMPTING SHIFT CIPHER DECRYPT");
                //success = DecryptShiftCipher(input);
                //PrintSubstitutionMap(substitutionMap);
            }
            // Check Substitution cipher
            if(!success)
            {
                Console.WriteLine("ATTEMPTING SUBSTITUTION CIPHER DECRYPT");
                success = DecryptSubstitutionCipher(input);
            }

            return success;
        }

        public static bool DecryptShiftCipher(string input)
        {
            bool success = false;

            Console.WriteLine(input);
            // Init character substitution map (cypher char, mapped to char for decrypt)
            substitutionMap = new Dictionary<char, char>();

            char[] inputChars = input.ToCharArray();
            // 1. Create sub map
            // 2. Check against input
            // 3. Shift sub map, and check again
            for (int i = 0; i < 26; i++)
            {
                ShiftSubMap(ref substitutionMap, englishAlphabet, i);
                //PrintSubstitutionMap(substitutionMap);
                char[] testChars = ApplySubstitutionMap(inputChars, substitutionMap);
                if (ParseEnglishSentence(new string(testChars)))
                {
                    Console.WriteLine("SUCESS!");
                    success = true;
                    break;
                }
            }
            Console.WriteLine("DONE");

            return success;
        }

        public static bool DecryptSubstitutionCipher(string input)
        {
            bool success = false;

            Console.WriteLine(input);
            // Init character substitution map (cypher char, mapped to char for decrypt)
            //substitutionMap = new Dictionary<char, char>();
            //for (int i = 0; i < englishAlphabet.Length; i++)
            //{
            //    //substitutionMap.Add(englishAlphabet[i], englishAlphabet[i]);
            //}

            /// Evaluate Input Text
            // Find char frequency
            char[] inputChars = input.ToCharArray();
            char[] inputFreq = GetCharFrequency(inputChars);

            // ToDo: Word pattern analysis
            // Sub = (CIPHER, ENGLISH)
            substitutionMap = new Dictionary<char, char>();
            //List<char> charsToMap = new List<char>(englishAlphabet);
            List<char> charsToMap = new List<char>(englishCharFrequency);
            // Create Substitution Map based on frequency
            for (int i = 0; i < inputFreq.Length; i++)
            //for (int i = 0; i < englishCharFrequency.Length; i++)
            {
                //if (substitutionMap.ContainsKey(inputFreq[i]))
                if (substitutionMap.ContainsKey(englishCharFrequency[i]))
                {
                    //substitutionMap[inputFreq[i]] = englishCharFrequency[i];
                    substitutionMap[englishCharFrequency[i]] = inputFreq[i];
                }
                else
                {
                    //substitutionMap.Add(inputFreq[i], englishCharFrequency[i]);
                    substitutionMap.Add(englishCharFrequency[i], inputFreq[i]);
                    //charsToMap.Remove(englishCharFrequency[i]);
                    charsToMap.Remove(englishCharFrequency[i]);
                }
            }
            PrintSubstitutionMap(substitutionMap);
            Queue<char> charsToMapQueue = new Queue<char>(charsToMap.ToArray());
            //for (int i = 0; i < englishAlphabet.Length; i++)
            //{
            //    if(substitutionMap.ContainsKey(englishAlphabet[i]) == false)
            //    {
            //        //substitutionMap.Add(englishAlphabet[i], charsToMapQueue.Dequeue());
            //        //substitutionMap.Add(charsToMapQueue.Dequeue(), englishAlphabet[i]);
            //    }
            //}
            for (int i = 0; i < englishCharFrequency.Length; i++)
            {
                if (substitutionMap.ContainsKey(englishCharFrequency[i]) == false)
                {
                    //substitutionMap.Add(englishAlphabet[i], charsToMapQueue.Dequeue());
                    //substitutionMap.Add(charsToMapQueue.Dequeue(), englishAlphabet[i]);
                    substitutionMap.Add(englishCharFrequency[i], '_');
                }
            }
            Console.WriteLine(charsToMapQueue.ToArray());
            PrintSubstitutionMap(substitutionMap);
            //return false;

            //PrintSubstitutionMap(substitutionMap);
            //char[] firstSubResults = ApplySubstitutions(inputArr, substitutionMap);
            //char[] firstSubResults = ApplySubstitutionMap(inputChars, substitutionMap);
            //Console.WriteLine(new string(firstSubResults));

            char[] keys = substitutionMap.Keys.ToArray();
            char[] values = substitutionMap.Values.ToArray();

            List<char[]> allMapPermutations = new List<char[]>();
            GetPer2(values, (char[] result) =>
            {
                Console.WriteLine("Permutation:\t" + new string(result));
                allMapPermutations.Add(result);

                //for (int k = 0; k < keys.Length; k++)
                //{
                //    substitutionMap[keys[k]] = result[k];
                //}

                //PrintSubstitutionMap(substitutionMap);
                char[] testChars = ApplySubstitutionMap(inputChars, keys, result);
                if (ParseEnglishSentence(testChars))
                {
                    Console.WriteLine("SUCESS!");
                    success = true;
                    return true;
                }
                return false;
            });

            return false;
            for (int i = 0; i < allMapPermutations.Count; i++)
            {
                //ShiftSubMap(ref substitutionMap, englishAlphabet, i);
                for (int k = 0; k < keys.Length; k++)
                {
                    substitutionMap[keys[k]] = allMapPermutations[i][k];
                }

                //PrintSubstitutionMap(substitutionMap);
                char[] testChars = ApplySubstitutionMap(inputChars, substitutionMap);
                if (ParseEnglishSentence(testChars))
                {
                    Console.WriteLine("SUCESS!");
                    success = true;
                    break;
                }
            }

            /// Brute Force
            // Modify the substitution table until we find a table that renders a correct sentence.
            // Parse the first ~5 char for an english word each cycle.


            //string inputString = "whatsinanamearosebyanyothernamewouldsmellassweet";
            //ParseEnglishSentence(inputString);

            return success;
        }

        public bool BruteForceDecrypt(char[] source, out char[] str, Dictionary<char, char> subMap)
        {
            str = source;



            return false;
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
            // BROKEN
            List<char> keys = new List<char>(subMap.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                Console.WriteLine(new string(input));
                ReplaceChar(ref input, keys[i], subMap[keys[i]]);
                //ReplaceChar(ref input, subMap[keys[i]], keys[i]);
            }
            return input;
        }

        public static char[] ApplySubstitutionMap(char[] input, Dictionary<char, char> subMap)
        {
            char[] ret = new char[input.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = '_';
            }

            for (int i = 0; i < input.Length; i++)
            {
                //Console.WriteLine(new string(ret));
                ret[i] = subMap[input[i]];
            }

            return ret;
        }
        public static char[] ApplySubstitutionMap(char[] input, char[] plaintext, char[] ciphertext)
        {
            Dictionary<char, char> subMap = new Dictionary<char, char>();
            for (int i = 0; i < ciphertext.Length; i++)
            {
                if(ciphertext[i] != '_')
                {
                    //Console.WriteLine(ciphertext[i]);
                    subMap.Add(ciphertext[i], plaintext[i]);
                }
            }
            return ApplySubstitutionMap(input, subMap);
        }

        public static void ShiftSubMap(ref Dictionary<char, char> subMap, char[] alphabet, int shiftAmount)
        {
            for (int i = 0; i < alphabet.Length; i++)
            {
                int newInt= (i + shiftAmount) % 26;
                char newChar = alphabet[newInt];
                if(subMap.ContainsKey(alphabet[i]))
                {
                    subMap[alphabet[i]] = newChar;
                }
                else
                {
                    subMap.Add(alphabet[i], newChar);
                }
            }
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
                    //Console.WriteLine("Testing:\t" + new string(testWord));
                    if (testWord.Length == 1 && !(testWord[0] == 'a' || testWord[0] == 'A'))
                    {
                        // Single character word that isn't 'a'.
                    }
                    else if (wordChecker.TestWord(new string(testWord)))
                    {
                        //Console.WriteLine("FOUND WORD:\t" + new string(testWord));
                        
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
            return ParseEnglishSentence(inputString.ToCharArray());
        }
        private static bool ParseEnglishSentence(char[] inputArr)
        {
            //Console.WriteLine("INPUT:\t" + inputString);
            int foundCharCount = 0;
            int minSize = 1;
            List<char[]> foundWords = new List<char[]>();
            while (foundCharCount < inputArr.Length)
            {
                int startIndex = Math.Max(foundCharCount, 0);
                int maxSize = inputArr.Length - foundCharCount;
                //Console.WriteLine(startIndex + ", " + maxSize + ", " + minSize);
                char[] foundWord = TryFindWord(ref inputArr, startIndex, maxSize, minSize);
                if (foundWord.Length > 0)
                {
                    foundWords.Add(foundWord);
                    minSize = 1; // Reset the minSize to 1. Finishes the backtracking cycle.
                    // Note: This might cause a bad loop, and could get us stuck with 3 or more backtracks. Need to manage the variable better.
                    //Console.WriteLine("Added:\t"+ new string(foundWord));
                }
                else if (foundWords.Count >= 1)
                {
                    //Console.WriteLine("Couldnt find word. Backtracking...");
                    minSize = foundWords[foundWords.Count - 1].Length + 1;
                    foundWords.RemoveAt(foundWords.Count - 1);
                }
                else
                {
                    // Note: One typo can "destroy" the whole sentence due to backtracking methods. 
                    // There needs to be a limit on backtracking, and we should capture "typo" words.
                    //Console.WriteLine("Couldnt find ANY words!");
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

        /// PERMUTATIONS ====================================
        private static void Swap(ref char a, ref char b)
        {
            if (a == b) return;

            a ^= b;
            b ^= a;
            a ^= b;
        }

        public static bool GetPer2(char[] list, Func<char[], bool> func = null)
        {
            int x = list.Length - 1;
            //return GetPer2(list, x, x, func);
            return GetPer2(list, 0, x, func);
        }
        private static bool GetPer2(char[] list, int depth, int arrayEnd, Func<char[], bool> func = null)
        {
            bool success = false;
            //Swap(ref list[depth], ref list[depth]);
            //Console.WriteLine(list);
            if (func != null)
            {
                success = func(list);
                if (success)
                {
                    return true;
                }
            }
            //if (depth >= 0)
            //{
            //    for (int i = depth - 1; i > 0; i--)
            //    {
            //        Swap(ref list[depth], ref list[i]);
            //        if (list[depth] != list[i])
            //        {
            //            success = GetPer2(list, depth - 1, 0, func);
            //            if (success)
            //            {
            //                return true;
            //            }
            //        }
            //        Swap(ref list[depth], ref list[i]);
            //    }
            //}

            //if (depth > 0)
            //{
            //    for (int i = depth - 1; i < arrayEnd; i++)
            //    {
            //        //if (list[depth] != list[i])
            //        {
            //            Swap(ref list[depth], ref list[i]);
            //            success = GetPer2(list, depth - 1, arrayEnd, func);
            //            if (success)
            //            {
            //                return true;
            //            }
            //        }
            //        //Swap(ref list[depth], ref list[i]);
            //    }
            //}

            //if (depth > 0)
            //{
            //    for (int i = depth; i < arrayEnd; i++)
            //    {
            //        if (list[depth] != list[i])
            //        {
            //            Swap(ref list[depth], ref list[i]);
            //            success = GetPer2(list, depth - 1, arrayEnd, func);
            //            if (success)
            //            {
            //                return true;
            //            }
            //        }
            //        //Swap(ref list[depth], ref list[i]);
            //    }
            //}

            //if (depth != arrayEnd)
            if (depth < arrayEnd)
            {
                for (int i = depth + 1; i <= arrayEnd; i++)
                {
                    if (list[depth] != list[i])
                    {
                        Swap(ref list[depth], ref list[i]);
                        success = GetPer2(list, depth + 1, arrayEnd, func);
                        if (success)
                        {
                            return true;
                        }
                        Swap(ref list[depth], ref list[i]);
                    }
                }
            }

            return success;
        }

        public static bool GetPer(char[] list, Func<char[], bool> func = null)
        {
            int x = list.Length - 1;
            return GetPer(list, 0, x, func);
        }

        private static bool GetPer(char[] list, int depth, int arrayEnd, Func<char[], bool> func = null)
        {
            bool success = false;
            if (depth == arrayEnd)
            {
                if (func != null)
                {
                    success = func(list);
                }
            }
            else
            {
                for (int i = depth; i <= arrayEnd; i++)
                {
                    Swap(ref list[depth], ref list[i]);
                    success = GetPer(list, depth + 1, arrayEnd, func);
                    if (success)
                    {
                        return true;
                    }
                    Swap(ref list[depth], ref list[i]);
                }
            }
            return success;
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
