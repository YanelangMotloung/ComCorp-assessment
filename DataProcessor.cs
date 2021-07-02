using System.Collections.Generic;

namespace ComCorpAssessment
{

    public abstract class DataProcessor
    {
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
        public abstract void PrepareData(string[] data);
        
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
        public abstract void SanitizeData();

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
        public abstract void CountWords();

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
        public abstract IEnumerable<KeyValuePair<string, int>> ProcessWords(int top, int length);

        //
        // Summary:
        //     This is a Template Method that calls the PrepareData, SanitizeData, CountWords and ProcessWords functions.
        //     This functions is the only function the user calls.
        //
        // Parameters:
        //   data:
        //     An array of strings to be counted.
        //   top:
        //     The number of words to be populated in thee dictionary.
        //   length:
        //     The minimum length of words to be returned
        //   
        //
        // Returns:
        //     The dictionary of unique words and their corresponding occurences.
        //
        public IEnumerable<KeyValuePair<string, int>> ProcessData(string[] data, int top, int length = 0)
        {
            PrepareData(data);
            SanitizeData();
            CountWords();
            return ProcessWords(top, length);
        }
    }
}