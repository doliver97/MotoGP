using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Net;

namespace MotoGP
{
    /// <summary>
    /// Interaction logic for News.xaml
    /// </summary>
    public partial class News : Window
    {
        //Copied from here: https://stackoverflow.com/questions/10709821/find-text-in-string-with-c-sharp
        public static string GetBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        public News()
        {
            InitializeComponent();

            //The feed is NOT a valid xml file, it cannot be processed automatically
            //See: https://validator.w3.org/feed/check.cgi?url=http%3A%2F%2Fwww.motogp.com%2Fen%2Fnews%2Frss

            //string url = "http://www.motogp.com/en/news/rss";
            //XmlReader reader = XmlReader.Create(url);
            //SyndicationFeed feed = SyndicationFeed.Load(reader);
            //reader.Close();

            //Have to do this manually:
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            string rssText = webClient.DownloadString("http://www.motogp.com/en/news/rss");
            string[] rssTextLines = rssText.Split('\n');

            //Get titles
            List<string> titles = new List<string>();
            for (int i = 0; i < rssTextLines.Length; i++)
            {
                string title = GetBetween(rssTextLines[i],"<title>","</title>");
                if(title!="")
                {
                    titles.Add(title);
                }
            }

            //Get descriptions
            List<string> descriptions = new List<string>();
            for (int i = 0; i < rssTextLines.Length; i++)
            {
                string description = GetBetween(rssTextLines[i], "<description>", "</description>");
                if (description != "")
                {
                    descriptions.Add(description);
                }
            }

            //Add to list
            for (int i = 1; i < titles.Count; i++)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Content = titles[i];
                lvi.Style = (Style)(Resources["Title"]);
                NewsList.Items.Add(lvi);

                ListViewItem lvi2 = new ListViewItem();
                lvi2.Content = descriptions[i];
                lvi2.Style = (Style)(Resources["Description"]);
                NewsList.Items.Add(lvi2);
            }
        }
    }
}
