using System;
using System.Collections.Generic;
using log4net;
using System.IO;
using ComCorpAssessment.Configs;

namespace ComCorpAssessment.Models
{
    class FileHandler
    {
        public List<string> WordsList { get; set; }
        ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


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
        public FileHandler(string filePath)
        {
            try
            {
                if (filePath is null)
                {
                    _log.Error("The provided filepath is empty");
                    throw new NullReferenceException(" The provided filepath is empty");

                }
                string[] lines = File.ReadAllLines(filePath);

                if (lines.Length > 0)
                {
                    WordsList = Utilities.ConvertStringArrayToStringArrayList(lines);
                }
                else
                {
                    _log.Error("The input specied file is empty");
                    throw new IOException("The input specied file is empty");
                }
            }
            catch (FileNotFoundException e)
            {
                _log.Error(e.Message);
                throw new Exception("The file or directory cannot be found.");
            }
            catch (DirectoryNotFoundException e)
            {
                _log.Error(e.Message);
                throw new Exception("The file or directory cannot be found.");
            }
            catch (DriveNotFoundException e)
            {
                _log.Error(e.Message);
                throw new Exception("The drive specified in 'path' is invalid.");
            }
            catch (PathTooLongException e)
            {
                _log.Error(e.Message);
                throw new Exception("'path' exceeds the maxium supported path length.");
            }
            catch (UnauthorizedAccessException e)
            {
                _log.Error(e.Message);
                throw new Exception("You do not have permission to create this file.");
            }
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32)
            {
                _log.Error(e.Message);
                throw new Exception("There is a sharing violation.");
            }
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 80)
            {
                _log.Error(e.Message);
                throw new Exception("The file already exists.");
            }
            catch (IOException e)
            {
                _log.Error($"An exception occurred:\nError code: " + $"{e.HResult & 0x0000FFFF}\nMessage: {e.Message}");
                throw new Exception($"An exception occurred:\nError code: " + $"{e.HResult & 0x0000FFFF}\nMessage: {e.Message}");
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
                throw e;
            }

        }

    }
    
}
