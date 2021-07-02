using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;

namespace ComCorpAssessment.Configs
{
    static class Utilities
    {
        // this variable for logging to the file
        static ILog s_log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //
        // Summary:
        //     Convert and trim the array of strings to the list of strings
        //
        // Parameters:
        //   stringList:
        //     An array of strings to be converted.
        //
        // Returns:
        //     The list of strings.
        //
        internal  static List<string> ConvertStringArrayToStringArrayList(string[] stringList)
        {
            List<string> _response = new List<string>();
            char[] _charsToTrim = { '*', ' ', '\'', '.', '?', '!', '\"', ',', ':', '[', ']', '#' };


            try
            {
                for (int i = 0; i < stringList.Length; i++)
                {
                    string[] _words = stringList[i].Split(' ');

                    List<string> wordsList = _words.ToList();
                    _response.AddRange(wordsList);
                }


                //Trimming each words
                for (int i = 0; i < _response.Count; i++)
                {
                    _response[i] = _response[i].Trim(_charsToTrim);
                }

            }
            catch (Exception ex)
            {
                s_log.Error(ex.Message);
                _response = new List<string>();
            }

            return _response;
        }


        //
        // Summary:
        //     Count the occurence of words and save them into a dictionary
        //
        // Parameters:
        //   stringList:
        //     An array of strings to be counted.
        //
        // Returns:
        //     The dictionary of unique words and their corresponding occurences.
        //
        internal static Dictionary<string, int> GetWordsCount(List<string> wordsList)
        {
            Dictionary<string, int> response = new Dictionary<string, int>();
            int wordCount = 0;

            try
            {
                Parallel.For(0, wordsList.Count, x =>
                {
                    Task.Run(() =>
                    {
                        wordCount = wordsList.Where(v => v.Equals(wordsList[x])).Count();
                        if (!response.ContainsKey(wordsList[x]))
                        {
                            response.Add(wordsList[x], wordCount);
                        }
                    });
                });
            }
            catch (Exception ex)
            {
                s_log.Error(ex.Message);
                response = new Dictionary<string, int>();
            }

            return response;
        }
    }
}
