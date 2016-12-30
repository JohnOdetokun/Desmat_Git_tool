using LibGit2Sharp;
using System;
using System.Collections.Generic;

namespace GitVersionTool
{
    class WriteCommitsToFile
    {
        string fileWriteName;
        string fileWritePath;

        public WriteCommitsToFile(string[] args)
        {
            fileWriteName = args[1];
            fileWritePath = args[2];
            ValidateArgs();
        }

        public void Write(string contentToWrite, string extension)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileWritePath + "\\" + fileWriteName + '.' + extension))
                {
                    file.Write(contentToWrite);
                }
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
                Console.Read();
            }
        }

        public void WriteMultipleFileTypes(IBuildOutputString<Commit>[] fileTypes, List<Commit> filteredList)
        {
            Console.WriteLine("Writing to chosen files...");
            foreach(IBuildOutputString<Commit> type in fileTypes)
            {
                string content = type.BuildString(filteredList);
                string extention = type.Extension();
                Write(content, extention);
            }
            Console.WriteLine("Done writing to path \"" + fileWritePath + "\"; file name is " + fileWriteName + "\nPRESS ENTER TO EXIT");
            Console.ReadLine();
        }

        public void ValidateArgs()
        {
            if (fileWriteName == null || fileWritePath == null)
            {
                throw new ArgumentNullException("Incorrect/Insufficient args");
            }
        }
    } 
}
