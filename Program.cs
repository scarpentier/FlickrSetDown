using System;
using System.IO;
using System.Linq;
using System.Net;
using FlickrNet;

namespace FlickrSetDown
{
    /// <summary>
    /// Main program class
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Flickr object from FlickrNet
        /// </summary>
        private static readonly Flickr Flickr = new Flickr("651212bcc04a1da0adc2e2b2a34860fe", "78bfd6c28af0ce8b");
                                       // Please, don't abuse my personal keys

        /// <summary>
        /// WebClient object used to download the pictures
        /// </summary>
        private static readonly WebClient WebClient = new WebClient();

        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("USAGE: FlickrSetDown [Set]");
                Console.WriteLine("Set\tSet number that you want to download");
                Console.WriteLine();
                Console.WriteLine("Ex: FlickrSetDown 72157623513691987");
                return;
            }

            var set = args[0];

            Directory.CreateDirectory(set);

            foreach (var file in Flickr.PhotosetsGetPhotos(set).Where(file => !File.Exists(set + "/" + file.Title + ".jpg") ))
            {
                try
                {
                    var url = Flickr.PhotosGetInfo(file.PhotoId).OriginalUrl;
                    var path = file.Title + ".jpg";
                    Console.Write("Downloading {0}... ", path);
                    if (path != null) WebClient.DownloadFile(url, set + "/" + path);
                    Console.WriteLine("OK");
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
            Console.WriteLine("Operation complete!");
        }
    }
}