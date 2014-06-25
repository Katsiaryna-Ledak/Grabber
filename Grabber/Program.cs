using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net;

namespace Grabber
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int movie = 0; movie < 500; movie++)
            {
                string link = URLGenerator.GetLinkForMovie();
                IMDBGrabber.pageGrabber(link);
                URLGenerator.FilmNumber++;
            }
            Console.ReadLine();   
        }
    }
}
