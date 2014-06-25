using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Grabber
{
    class IMDBGrabber
    {
        public static void pageGrabber(string link)
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(link);
            myHttpWebRequest.KeepAlive = true;
            myHttpWebRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-GB; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4";
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");
            HttpWebResponse myHttpWebResponse;

            // Фильм с указанным номером может не существовать, тогда страница не найдена
            try
            {
                myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            //формируем название архива
            string number = String.Format("{0:0000000}", URLGenerator.FilmNumber);
            string path = @"D:\movie = " + number + ".gz";

            using (FileStream file = File.OpenWrite(path))
            {
                myHttpWebResponse.GetResponseStream().CopyTo(file);
            }
            Console.WriteLine("{0}", path);
        }                             
    }
}
