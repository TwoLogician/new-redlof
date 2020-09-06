using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Softveloper.IO;

namespace NewRedlof.Cli
{
    class Program
    {
        /// <summary>
        /// New Redlof
        /// </summary>
        /// <param name="source">The path of the file or directory to move.</param>
        /// <param name="dest">The path to the new location for sourceDirName . If sourceDirName is a file, then destDirName must also be a file name.</param>
        static void Main(string source, string dest)
        {
            try
            {
                if (string.IsNullOrEmpty(source))
                {
                    Console.WriteLine("Where is --source?");
                    return;
                }
                if (string.IsNullOrEmpty(dest))
                {
                    Console.WriteLine("Where is --dest?");
                    return;
                }
                var directories = GetDirectories(source);
                var files = Directory.GetFiles(source).ToList();
                files.AddRange(directories.SelectMany(x => Directory.GetFiles(x)));
                files.ForEach(x =>
                {
                    try
                    {
                        var file = new FileInfo(x);
                        var lastWriteTime = file.LastWriteTime;
                        var year = lastWriteTime.Year;

                        var path = Path.Combine(dest, year.ToString(), $"{year}-{lastWriteTime.Month}");
                        Filerectory.CreateDirectory(path);
                        File.Move(x, Path.Combine(path, file.Name));

                        Console.Write($"\r{files.IndexOf(x) + 1}/{files.Count()}");
                    }
                    catch (Exception ex)
                    {
                        Catch(ex);
                    }
                });
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        private static void Catch(Exception ex)
        {
            var foregroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex);
            Console.ForegroundColor = foregroundColor;
        }

        private static List<string> GetDirectories(string source)
        {
            var allDirectories = new List<string>();
            var directories = Directory.GetDirectories(source).ToList();
            allDirectories.AddRange(directories);
            allDirectories.AddRange(directories.SelectMany(x => GetDirectories(x)));
            return allDirectories;
        }
    }
}
