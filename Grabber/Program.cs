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
        static int p = 0;
        static void Main(string[] args)
        {
            int kol = 0; //количество страниц в архиве
            int index = 50; //количество страниц с отзывами об одном фильме

            GetIndexFromReviewPage();

            //цикл по всем страницам в одном фильме
            for (int pageNumber = 0; pageNumber < index; pageNumber = pageNumber + 10)
            {
                // Отправляем GET запрос и получаем в ответ HTML-код сайта
                string str = GetMovieNumber();
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(MakeURL(str, pageNumber));
                myHttpWebRequest.KeepAlive = false;
                myHttpWebRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-GB; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4";
                myHttpWebRequest.Method = "GET";
                myHttpWebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                //формируем название архива
                string path = @"D:\movie = " + str + "review" + pageNumber + ".gz";

                using (FileStream file = File.OpenWrite(path))
                {
                    myHttpWebResponse.GetResponseStream().CopyTo(file);
                }
                Console.WriteLine("{0}", path);
                kol++;
            }
            Console.WriteLine("number of pages = {0}", kol);
            /* когда kol = 100 (например), засунуть их в большой архив*/


            string directoryPath = @"D:\AllReviews0";
            DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);

            Console.ReadLine();
        }

        //получить ссылку на страницу с отзывом у конкретного фильма
        public static string MakeURL (string movieNumber, int pageNum)
        {
            string url = "http://www.imdb.com/title/tt";  // Адрес страницы без индекса фильма
            string newUrl = url + movieNumber + "/reviews?start=" + pageNum.ToString();
            return newUrl;
        }

        //вычислить номер фильма
        public static string GetMovieNumber()
        {
            string num = "000000";
            p++;
            return num + p.ToString();
        }

        //получить первую страницу с отзывами в .html
        public static string GetFirstPageOfMovie()
        {
            string mn = GetMovieNumber();
            string url = "http://www.imdb.com/title/tt" + mn + "/reviews?start=0";

            HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            myHttpWebRequest.KeepAlive = false;
            myHttpWebRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-GB; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4";
            myHttpWebRequest.Method = "GET";
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

            string path = @"D:\movie = " + mn + "page0.html";

            using (FileStream file = File.OpenWrite(path))
            {
                myHttpWebResponse.GetResponseStream().CopyTo(file);
            }
            return path;
        }

        public static void GetIndexFromReviewPage()
        {
            GetFirstPageOfMovie();
            string html = File.ReadAllText(GetFirstPageOfMovie());

            string sPattern = "Index [0-9] reviews in total";

            Regex r = new Regex(sPattern, RegexOptions.IgnoreCase);
            Match m = r.Match(html);
            /*искать совпадения и вытаскивать оттуда число index*/
        }

    }
}
