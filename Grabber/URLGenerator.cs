using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace Grabber
{
    class URLGenerator
    {
        static int filmNumber = 1;
        static string pageNumber;

        public static int FilmNumber
        {
            get { return filmNumber; }
            internal set { filmNumber = value; }
        }

        public static string PageNumber
        {
            get { return pageNumber; }
            internal set { pageNumber = value; }
        }

        //получить первую страницу с отзывами в .html
        static string GetFirstPageOfMovie()
        {
            string url = MakeURL("0"); // Получаем ссылку на первую страницу с отзывами (нулевую)

            HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            myHttpWebRequest.KeepAlive = true;
            myHttpWebRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-GB; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4";
            myHttpWebRequest.Method = "GET";
            HttpWebResponse myHttpWebResponse;
            try
            {
                myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            }
            catch (WebException ex)
            {
                Console.WriteLine(MakeURL("0") + ex.Message);
                return string.Empty;
            }

            string number = String.Format("{0:0000000}", filmNumber.ToString());
            string path = @"D:\movie = " + number + "page0.html";

            using (FileStream file = File.OpenWrite(path))
            {
                myHttpWebResponse.GetResponseStream().CopyTo(file);
            }
            return path;
        }

        //получить ссылку на страницу с отзывом у конкретного фильма
        static string MakeURL(string pageNumber)
        {
            string url = "http://www.imdb.com/title/tt";  // Адрес страницы без индекса фильма
            string number = String.Format("{0:0000000}", FilmNumber); // Номер фильма на IMDB
            string newUrl = url + number + "/reviews?start=" + pageNumber;
            return newUrl;
        }

        static int GetIndexFromFirstPage()
        {
            string path = GetFirstPageOfMovie();
            if (string.IsNullOrEmpty(path))
            {
                return 0;
            }
            string html = File.ReadAllText(path);
            int index = 0;

            Regex r = new Regex("([0-9]+\\s(reviews in total))");
            Match m = r.Match(html);
            if (!m.Success)
            {
                return 0;
            }
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

        public static List<string> GetListOfLinksForEachMovie()
        {
            List<string> links = new List<string>();
            int index = GetIndexFromFirstPage();
            for (int pageNumber = 0; pageNumber < (index + 1) * 10; pageNumber = pageNumber + 10)
            {
                links.Add(MakeURL(pageNumber.ToString()));
            }
       
            return links;
        }
    }
}
