using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Threading;

namespace Grabber
{
    class URLGenerator
    {
        int filmNumber = 1;  // Номер фильма на IMDB

        public int FilmNumber
        {
            get { return this.filmNumber; }
            internal set { this.filmNumber = value; }
        }

        //формируем ссылку для грэббера
        public IEnumerable<string> GetLinkForMovie(int mn)
        {
            string link = "";
            for (FilmNumber = mn; FilmNumber < 9999999; FilmNumber++)
            {
                string number = String.Format("{0:0000000}", FilmNumber);
                link = "http://www.imdb.com/title/tt" + number + "/reviews?count=" + Int16.MaxValue;
                Thread.Sleep(100);
                yield return link;
            }
        }
    }
}
