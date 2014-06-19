using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Net;


namespace Grabber
{
    class Program
    {
        static void Main(string[] args)
        {
            UnicodeEncoding uniEncode = new UnicodeEncoding();
            // Адрес страницы без индекса фильма
            string url = "http://www.imdb.com/title/tt";
            // индекс названия фильма
            string movieName = "0814314";
            // первая страница отзывов
            string page1 = "0";

            // Формируем адрес сайта
            // http://www.imdb.com/title/tt0814314/reviews?start=0
            string newUrl = url + movieName + "/reviews?start=" + page1;
            
            // Отправляем GET запрос и получаем в ответ HTML-код сайта
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(newUrl);
            myHttpWebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            StreamReader myStreamReader = new StreamReader(myHttpWebResponse.GetResponseStream());

            string path = @"C:\Users\Katsiaryna_Ledak\Documents\Visual Studio 2013\Projects\Grabber\AllMovies.gz";
            FileStream file = File.OpenWrite(path);
            StreamWriter myStreamWriter = new StreamWriter(file);
            myStreamWriter.Write(myStreamReader.ReadToEnd());
            
            /*byte[] bytesToCompress = uniEncode.GetBytes(myStreamReader.ReadToEnd());
            string path = @"C:\Users\Katsiaryna_Ledak\Documents\Visual Studio 2013\Projects\Grabber\AllMovies.gz";
            using (FileStream fileToCompress = File.Create(path))
            {
                using (GZipStream compressionStream = new GZipStream(fileToCompress, CompressionMode.Compress))
                {
                    compressionStream.Write(bytesToCompress, 0, bytesToCompress.Length);
                }
            }

            byte[] decompressedBytes = new byte[bytesToCompress.Length];
            using (FileStream fileToDecompress = File.Open(path, FileMode.Open))
            {
                using (GZipStream decompressionStream = new GZipStream(file, CompressionMode.Decompress))
                {
                    decompressionStream.Read(decompressedBytes, 0, bytesToCompress.Length);
                }
            }
            */
            myStreamReader.Close();
            myStreamWriter.Close();

            Console.ReadLine();
        }
    }
}
