using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Raccoom.Xml.Atom;

namespace Raccoom.Xml.Demo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RssReader());
            //CreateBasicFeed();
        }

        public static void CreateBasicFeed()
        {
            Raccoom.Xml.Atom.AtomFeed feed = new AtomFeed();
            //
            AtomContentConstruct link = feed.CreateLink();
            link.Type = AtomContentType.Text;
            link.Value = "Link1";
            feed.Links.Add(link);
            //
            link = feed.CreateLink();
            link.Type = AtomContentType.Text;
            link.Value = "Link2";
            feed.Links.Add(link);
            //
            AtomPersonConstruct author = feed.CreateAuthor();
            author.XmlLanguage = System.Globalization.CultureInfo.CurrentCulture;
            author.Name = "Christoph Richner";
            author.Email = "support@raccoom.net";
            Uri uri = new Uri("http://www.raccoom.net");
            author.Uri = uri;
            feed.Authors.Add(author);
            //
            feed.Updated = DateTime.Now;
            feed.Generator.Uri = uri.ToString();
            feed.Logo = uri;
            feed.Icon = uri;
            //
            AtomEntry entry = feed.CreateEntry();
            entry.Contributors.Add(author);
            entry.Authors.Add(author);
            entry.Id = uri;
            entry.Links.Add(link);
            entry.Title.Type = AtomContentType.Text;
            entry.Title.Value = "First entry title goes here";
            entry.Updated = DateTime.Now;
            entry.Summary.Value = @"<font class=""maintitle""><b>Hautkrebs-Verdacht auch bei Jugendlichen</b></font><br><br><font class=""font""><b>Am Montag konnte man sich in der Zentralschweiz gratis auf Hautkrebs untersuchen lassen. Der Andrang war riesig – doch nicht alle konnten die Praxis mit Erleichterung verlassen.</b><br><br>«Mit so einem Aufmarsch haben wir nicht gerechnet, wir mussten viele der über 100 Leute wieder nach Hause schicken», fasst der Zuger Hautarzt Urs Hasse den nationalen Hautkrebstag zusammen. <br><br>Am Montag konnte sich jedermann seine Haut gratis auf bösartige Melanome untersuchen lassen – nicht immer mit angenehmer Diagnose. «Bei 20 teils ganz jungen Personen stellte ich Muttermale fest, die genauer untersucht werden müssen», so Hasse.<br><br>Eine düsterere Bilanz zieht Bettina Schlagenhauf, Hautärztin in Küssnacht: «Zwei Patienten wiesen einen Verdacht auf Hautkrebs auf. Ihre Melanome müssten in diesem Fall operativ entfernt werden.» Dazu kämen mehrere Vorstufenfälle von Hautkrebs. Immerhin konnte Schlagenhauf mit ihrer Praxispartnerin trotz Wartezeit von bis zu einer Stunde alle 180 Patienten untersuchen.<br><br>Barbara Iseli von der Krebsliga Schweiz möchte, dass sich nächstes Jahr alle Interessierten untersuchen lassen können: «In der Zent-ralschweiz konnten wir diesmal nur fünf Hautärzte für diese freiwillige Aktion gewinnen.»<br><br>Mario Stübi<br><br></font>""";
            entry.Summary.Type = AtomContentType.Html;
            //
            feed.Entries.Add(entry);
            //
            System.Xml.Serialization.XmlSerializer ser;
            try
            {
                ser = new System.Xml.Serialization.XmlSerializerFactory().CreateSerializer(feed.GetType());
                using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create("atom.xml"))
                {
                    System.Xml.Serialization.XmlSerializerNamespaces ns = new System.Xml.Serialization.XmlSerializerNamespaces();
                    //ns.Add("atom", "http://www.w3.org/2005/Atom");
                    ser.Serialize(writer, feed, ns);
                }
                //
                AtomParser parser = new AtomParser();
                using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create("atom.xml"))
                {
                    parser.Parse(reader);
                }

            }
            catch (Exception ex)
            {
                Exception e = ex.GetBaseException();
            }
        }
        class AtomParser : Raccoom.Xml.ComponentModel.SyndicationObjectParser
        {
            internal Raccoom.Xml.Atom.AtomFeed Parse(System.Xml.XmlReader reader)
            {
                AtomFeed feed = new AtomFeed();
                base.Parse(feed, reader);
                return feed;
            }
        }
    }
}