using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections;

namespace Grabber
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.DefaultConnectionLimit = 100;
            URLGenerator gen = new URLGenerator();
            IMDBGrabber grabber = new IMDBGrabber();

            foreach (string myLink in gen.GetLinkForMovie(1))
            {
                // throttling
               Task t = grabber.pageGrabberAsync(myLink, gen.FilmNumber);
            }

            Console.ReadLine();
        }
    }
}
