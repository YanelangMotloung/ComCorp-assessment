using System;
using System.Collections.Generic;
using System.Linq;
using ComCorpAssessment.Configs;
using ComCorpAssessment.Models;
using log4net;



namespace ComCorpAssessment.Controllers
{
    internal class FileController
    {
        ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private FileHandler _fileHandler { get; set; }
        public List<string> _words { get; set; }

        //
        // Summary:
        //     Opens an existing file for reading, extract and store all the words into the list of strings.
        //
        // Parameters:
        //   path:
        //     The file to be opened for reading.
        //
        // Returns:
        //     void (It's a constructor).
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     path is a zero-length string, contains only white space, or contains one or more
        //     invalid characters as defined by System.IO.Path.InvalidPathChars.
        //
        //   T:System.ArgumentNullException:
        //     path is null.
        //
        //   T:System.IO.PathTooLongException:
        //     The specified path, file name, or both exceed the system-defined maximum length.
        //     For example, on Windows-based platforms, paths must be less than 248 characters,
        //     and file names must be less than 260 characters.
        //
        //   T:System.IO.DirectoryNotFoundException:
        //     The specified path is invalid, (for example, it is on an unmapped drive).
        //
        //   T:System.UnauthorizedAccessException:
        //     path specified a directory.-or- The caller does not have the required permission.
        //
        //   T:System.IO.FileNotFoundException:
        //     The file specified in path was not found.
        //
        //   T:System.NotSupportedException:
        //     path is in an invalid format.
        //
        //   T:System.IO.IOException:
        //     An I/O error occurred while opening the file.
        internal FileController(string filePath)
        {
            try
            {
                _fileHandler = new FileHandler(filePath);
                _words = _fileHandler.WordsList;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                throw ex;
            }

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
        internal Dictionary<string,int> CountWords(List<string> words)
        {
            Dictionary<string, int> _response = new Dictionary<string, int>();
            try
            {
                _response = Utilities.GetWordsCount(words);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }

            return _response;
        }

        //
        // Summary: Get the specified number of pair elements from a Dictionary
        //    
        //
        // Parameters:
        //   words:
        //     An array of strings to be counted.
        //   number:
        //      a n number of objects.
        //
        // Returns:
        //     The dictionary of unique words and their corresponding occurences.
        //

        internal IEnumerable<KeyValuePair<string, int>> GetSpecifiedNumberOfbjects(Dictionary<string, int> words, int number)
        {
            IEnumerable<KeyValuePair<string, int>> _response = new Dictionary<string, int>();

            try
            {
                _response = (from entry in words
                                                      orderby entry.Value
                                                      descending
                                                      select entry).ToDictionary
                           (
                            pair => pair.Key,
                            pair => pair.Value
                           ).Take(number);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }

            return _response;

        }

        
        //
        // Summary: Get the specified number of pair elements from a Dictionary
        //    
        //
        // Parameters:
        //   words:
        //     An array of strings to be counted.
        //   number:
        //      a n number of objects.
        //   length:
        //      the required minimun length per each word
        //
        // Returns:
        //     The dictionary of unique words and their corresponding occurences.
        //
        internal IEnumerable<KeyValuePair<string, int>> GetSpecifiedNumberOfbjects(Dictionary<string, int> words, int number,int length)
        {
            IEnumerable<KeyValuePair<string, int>> _response = new Dictionary<string, int>();

            try
            {
                _response = (from entry in words
                             orderby entry.Value descending
                             where entry.Key.Length > length
                             select entry
                               ).ToDictionary
                           (
                            pair => pair.Key,
                            pair => pair.Value
                           ).Take(number);

            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }

            return _response;

        }
    }
}
