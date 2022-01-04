using MediaToolkit;
using MediaToolkit.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace punkMediaInfo.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            var converts = new List<string>();

            converts.AddRange(ProcessFiles("\\films\\"));

            File.WriteAllLines("files.txt", converts);
        }

        public static IEnumerable<string> ProcessFiles(string path, string extension = ".mkv", string videoCodec = "hevc")
        {
            var converts = new List<string>();

            using (var engine = new Engine())
                foreach (var file in Directory
                    .EnumerateFiles(path, "*", SearchOption.AllDirectories)?
                    .Where(x => x.EndsWith(extension)))
                {
                    try
                    {
                        var media = new MediaFile($"{file}");
                        engine.GetMetadata(media);
                        if (media.Metadata.VideoData.Format.Contains(videoCodec))
                        {
                            converts.Add(media.Filename);
                            System.Console.ForegroundColor = ConsoleColor.Green;
                        }

                        System.Console.WriteLine($"{media.Filename} - {media.Metadata.VideoData.Format}");
                    }
                    catch (Exception ex)
                    {
                        System.Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.WriteLine(ex);
                    }

                    System.Console.ResetColor();
                }

            return converts;
        }
    }
}
