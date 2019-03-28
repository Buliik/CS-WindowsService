using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Globalization;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace downloader
{
    public class Download
    {
        public static String DownloadData(String uri)
        {
            String data;
            using (WebClient client = new WebClient())
            {
                byte[] myDataBuffer = client.DownloadData(uri);
                data = Encoding.ASCII.GetString(myDataBuffer);
            }

            return data;
        }

        public static void DrawData(List<double> currencies)
        {
            using (Bitmap bmp = new Bitmap(400, 300))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.FillRectangle(new SolidBrush(Color.White), 0, 0, 400, 300);

                    g.DrawLine(new Pen(Color.Black), 50, 50, 50, 250);
                    g.DrawString("Exchange rate", new Font(FontFamily.GenericMonospace, 10),
                        new SolidBrush(Color.Black), 0, 30);
                    g.DrawLine(new Pen(Color.Black), 50, 250, 250, 250);
                    g.DrawString("Currency", new Font(FontFamily.GenericMonospace, 10),
                        new SolidBrush(Color.Black), 255, 260);


                    g.DrawString("EUR", new Font(FontFamily.GenericMonospace, 10),
                        new SolidBrush(Color.Black), 85, 260);

                    g.DrawString(currencies[0].ToString(), new Font(FontFamily.GenericMonospace, 10),
                        new SolidBrush(Color.Black), 80, (int)(220 - 5 * currencies[0]));

                    g.DrawLine(new Pen(Color.Blue), 100, 250, 100, (int)(250 - 5 * currencies[0]));


                    g.DrawString("USD", new Font(FontFamily.GenericMonospace, 10),
                        new SolidBrush(Color.Black), 135, 260);

                    g.DrawString(currencies[1].ToString(), new Font(FontFamily.GenericMonospace, 10),
                        new SolidBrush(Color.Black), 130, (int)(220 - 5 * currencies[1]));

                    g.DrawLine(new Pen(Color.Red), 150, 250, 150, (int)(250 - 5 * currencies[1]));


                    g.DrawString("GBP", new Font(FontFamily.GenericMonospace, 10),
                        new SolidBrush(Color.Black), 185, 260);

                    g.DrawString(currencies[2].ToString(), new Font(FontFamily.GenericMonospace, 10),
                        new SolidBrush(Color.Black), 180, (int)(220 - 5 * currencies[2]));

                    g.DrawLine(new Pen(Color.Green), 200, 250, 200, (int)(250 - 5 * currencies[2]));

                }

                bmp.Save("Graph.png");
            }
        }

        public static void SendMail(string email)
        {
            var now = DateTime.Now;
            using (var mail = new MailMessage())
            {
                mail.From = new MailAddress("a007n007@gmail.com");
                mail.To.Add(new MailAddress(email));
                mail.Subject = $"Daily currency rates";

                Attachment data = new Attachment("Graph.png", MediaTypeNames.Application.Octet);
                ContentDisposition disposition = data.ContentDisposition;
                disposition.CreationDate = System.IO.File.GetCreationTime("Graph.png");
                disposition.ModificationDate = System.IO.File.GetLastWriteTime("Graph.png");
                disposition.ReadDate = System.IO.File.GetLastAccessTime("Graph.png");

                mail.Attachments.Add(data);

                using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("a007n007@gmail.com", "01822fd971d");
                    client.EnableSsl = true;

                    client.Send(mail);
                }
            }          
        }

        public static double ParseRate(string txt, string code)
        {
            string[] rows = txt.Split(new char[] { '\n' });
            char[] colSplitChars = new char[] { '|' };
            foreach (string row in rows)
            {
                string[] cols = row.Split(colSplitChars);
                if (cols.Length < 3)
                {
                    continue;
                }
                if (cols[3] == code)
                {
                    return double.Parse(cols[4], CultureInfo.InvariantCulture);
                }
            }
            return 0;
        }
    }
}
