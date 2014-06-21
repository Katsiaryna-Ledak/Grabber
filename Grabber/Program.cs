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
        static int p = 1;
        static void Main(string[] args)
        {
            GetOneMovieReviews();
            Console.ReadLine();
        }

        //получить ссылку на страницу с отзывом у конкретного фильма
        public static string MakeURL (int pageNum)
        {
            string url = "http://www.imdb.com/title/tt";  // Адрес страницы без индекса фильма
            string number = String.Format("{0:0000000}", p); // Номер фильма на IMDB
            string newUrl = url + number + "/reviews?start=" + pageNum.ToString();
            //p++;
            return newUrl;
        }

        //получить первую страницу с отзывами в .html
        public static string GetFirstPageOfMovie()
        {
            string url = MakeURL(0); // Получаем ссылку на первую страницу с отзывами (нулевую)

            HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            myHttpWebRequest.KeepAlive = false;
            myHttpWebRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-GB; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4";
            myHttpWebRequest.Method = "GET";
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

            string number = String.Format("{0:0000000}", p);
            string path = @"D:\movie = " + number + "page0.html";

            using (FileStream file = File.OpenWrite(path))
            {
                myHttpWebResponse.GetResponseStream().CopyTo(file);
            }
            return path;
        }

        public static int GetIndexFromFirstPage()
        {
            string html = File.ReadAllText(GetFirstPageOfMovie());
            int index = 0;

            Regex r = new Regex("([0-9]+\\s(reviews in total))");
            Match m = r.Match(html);
            if (!m.Success)
            {
                return 0;
            }
            else
            {
                string str = m.Value.Split(' ')[0];

                if (!Int32.TryParse(str, out index))
                {
                    return 0;
                }
                if ((index == 0) || (index == 10))
                {
                    return 0;
                }
                else
                {
                    return index / 10;
                }
            }
        }

        public static void GetOneMovieReviews()
        {
            int kol = 0; //количество страниц в архиве
            int index = GetIndexFromFirstPage(); //количество страниц с отзывами об одном фильме

            //цикл по всем страницам в одном фильме
            for (int pageNumber = 0; pageNumber < index + 10; pageNumber = pageNumber + 10)
            {
                // Отправляем GET запрос и получаем в ответ HTML-код сайта
                
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(MakeURL(pageNumber));
                myHttpWebRequest.KeepAlive = false;
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
                    Console.WriteLine(MakeURL(pageNumber) + ex.Message);
                    return;
                }

                //формируем название архива
                string number = String.Format("{0:0000000}", p);
                string path = @"D:\movie = " + number + "review" + pageNumber + ".gz";

                using (FileStream file = File.OpenWrite(path))
                {
                    myHttpWebResponse.GetResponseStream().CopyTo(file);
                }
                Console.WriteLine("{0}", path);
                kol++;
            }
            if (kol > index)
            {
                p++;
                GetOneMovieReviews();
            }

            Console.WriteLine("number of pages = {0}", kol);
        }

    }
}
