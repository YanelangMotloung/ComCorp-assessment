using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComCorpAssessment.Configs;
using log4net;


namespace ComCorpAssessment
{
    public class TextProcessor : DataProcessor
    {
        #region local variables
        // logging to the file
        static ILog s_log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //  storing the words
        List<string> _wordsList = new List<string>();
        
        //storing the words and their corresponding number of occurences
        Dictionary<string, int> _countedWords = new Dictionary<string, int>();

        #endregion

        #region DataProcessor functions implementation
        //
        // Summary:
        //     iterate the array of strings and store each word in the List of strings
        //
        // Parameters:
        //   data:
        //     An array of strings to be counted.
        //
        // Returns:
        //     void.
        //
        public override void PrepareData(string[] data)
        {

            try
            {
                 for (int i = 0; i < data.Length; i++)
                {
                    string[] _words = data[i].Split(Utilities.s_linesSpliter);

                    List<string> wordsList = _words.ToList();

                    _wordsList.AddRange(wordsList);
                }
            }
            catch (Exception ex)
            {
                s_log.Error(ex.Message);
                _wordsList = new List<string>();
            }
        }

        //
        // Summary:
        //     Removes the unwanted character from the begining and the end of words List. The
        //     output is store on the input list.
        //
        // Parameters:
        //   no input parameters. 
        //
        // Returns:
        //     void.
        //
        public override void SanitizeData()
        {
            try
            {
                for (int i = 0; i < _wordsList.Count; i++)
                {
                    _wordsList[i] = _wordsList[i].Trim(Utilities.s_charsToTrim);
                }
            }
            catch (Exception ex)
            {
                s_log.Error(ex.Message);
                _wordsList = new List<string>();
            }
        }

        //
        // Summary:
        //     Count the occurence of words and save them into a dictionary of unique words and
        //     their corresponding occurences
        //
        // Parameters:
        //   stringList:
        //     An array of strings to be counted.
        //
        // Returns:
        //     The dictionary of unique words and their corresponding occurences.
        //
        public override void CountWords()
        {
            int _wordCount = 0;
            try
            {
                Parallel.For(0, _wordsList.Count, x =>
                {
                    Task.Run(() =>
                    {
                        _wordCount = _wordsList.Where(v => v.Equals(_wordsList[x])).Count();

                        if (!_countedWords.ContainsKey(_wordsList[x]))
                        {
                            _countedWords.Add(_wordsList[x], _wordCount);
                        }
                    });

                });
            }
            catch (Exception ex)
            {
                s_log.Error(ex.Message);
                _countedWords = new Dictionary<string, int>();
            }
        }

        //
        // Summary:
        //     Filter the dictionary of words and their occurences and return the dictionary 
        //     with the specified filter. The dictionary is returned in descending order as
        //     per the number of occurence and as the minimun length specified.
        //
        // Parameters:
        //   top:
        //     The number of words to be populated in thee dictionary.
        //   length:
        //     The minimum length of words to be returned
        //
        // Returns:
        //     The dictionary of unique words and their corresponding occurences.
        //
        public override IEnumerable<KeyValuePair<string, int>> ProcessWords(int top, int length)
        {
            IEnumerable<KeyValuePair<string, int>> _response = new Dictionary<string, int>();

            try
            {
                _response = (from entry in _countedWords
                             orderby entry.Value descending
                             where entry.Key.Length > length
                             select entry
                               ).ToDictionary
                           (
                            pair => pair.Key,
                            pair => pair.Value
                           ).Take(top);

            }
            catch (Exception ex)
            {
                s_log.Error(ex.Message);
                _response = new Dictionary<string, int>();
            }

            return _response;
        }
        #endregion
    }
}
