using DotblogsBackup.Classes;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace DotblogsBackup.BackupTools
{
    public class BackupPicture : Backup
    {
        public override void ClearOld()
        {
            try
            {
                if (Directory.Exists(Directory.GetCurrentDirectory()))
                {
                    if (Directory.Exists(Directory.GetCurrentDirectory() + @"\images"))
                    {
                        Directory.Delete(Directory.GetCurrentDirectory() + @"\images", true);
                        Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\images");
                    }
                    else
                    {
                        Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\images");
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        public override void GetPictureList()
        {
            int count = 0;
            Console.WriteLine("總共 " + allArticle.Count + " 篇文章");
            foreach (DotblogIndex pictureCollection in allArticle)
            {
                try
                {
                    count++;
                    HttpWebRequest myDefaultReq = HttpWebRequest.CreateHttp(pictureCollection.url);

                    WebResponse myRes = myDefaultReq.GetResponse();
                    StreamReader aReader = new StreamReader(myRes.GetResponseStream(), Encoding.UTF8);
                    String sPage = aReader.ReadToEnd();
                    aReader.Close();

                    //掃描圖片清單只 focus 在文章裡
                    string articleContent = "";
                    Match match = Regex.Match(sPage, @"\r\n\s+<div class\=""article__content\"">(.*)</div>\r\n\s+<div class\=""article__tags"">", RegexOptions.Singleline);
                    if (match.Success)
                    {
                        articleContent = match.Groups[1].Value;
                    }
                    else
                    {
                        throw new Exception("ERROR-" + pictureCollection.title + " : " + pictureCollection.url + " focus 不到文章內容");
                    }

                    //掃描並儲存圖片清單
                    MatchCollection pictures = Regex.Matches(articleContent, @"src=""(https://az787680.vo.msecnd.net/user/.+?png)""", RegexOptions.Multiline);
                    DotblogIndex aDotblogPicture = allArticle.Find(x => x.title == pictureCollection.title);
                    aDotblogPicture.pictures = pictures;

                    Thread.Sleep(100);
                    Console.WriteLine(string.Format("{0:0000}", count) + " 儲存圖片清單：" + pictureCollection.title);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR-" + pictureCollection.title + " : " + pictureCollection.url);
                    throw ex;
                }
            }
        }

        public override void Save()
        {
            Console.WriteLine("總共 " + allArticle.Count + " 篇文章");
            int count = 0;
            foreach (DotblogIndex pictureCollection in allArticle)
            {
                Boolean errorResult = false;
                do
                {
                    try
                    {
                        Console.WriteLine(string.Format("{0:0000}", count + 1) + " 正在處理：" + pictureCollection.title + " : " + pictureCollection.url);
                        GetData(count + 1, pictureCollection);
                        Thread.Sleep(500);
                        errorResult = false;
                    }
                    catch (PathTooLongException ex)
                    {
                        if (Directory.Exists(Directory.GetCurrentDirectory() + @"\images\" + string.Format("{0:0000}", count + 1) + "_" + pictureCollection.title) == true)
                        {
                            Directory.Delete(Directory.GetCurrentDirectory() + @"\images\" + string.Format("{0:0000}", count + 1) + "_" + pictureCollection.title);
                        }

                        pictureCollection.title = pictureCollection.title.Substring(0, 100);
                        GetData(count + 1, pictureCollection);
                        Thread.Sleep(500);
                        errorResult = false;
#if DEBUG
                        Console.WriteLine(ex.ToString());
#endif
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        errorResult = true;
                    }
                } while (errorResult);
                count++;
            }
        }

        private void GetData(int count, DotblogIndex pictureCollection)
        {
            if (Directory.Exists(Directory.GetCurrentDirectory() + @"\images\" + string.Format("{0:0000}", count) + "_" + pictureCollection.title) == false)
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\images\" + string.Format("{0:0000}", count) + "_" + pictureCollection.title);
            }

            foreach (Match url in pictureCollection.pictures)
            {
                WebRequest request = WebRequest.Create(url.Groups[1].Value);
                WebResponse response = request.GetResponse();
                Stream reader = response.GetResponseStream();

                string path = Directory.GetCurrentDirectory() + @"\images\";
                string directory = string.Format("{0:0000}", count) + "_" + pictureCollection.title + @"\";
                string needToRemovedString = Regex.Match(url.Groups[1].Value, @"(.*/)").Value;
                string fileName = url.Groups[1].Value.Replace(needToRemovedString, "");

                FileStream writer = new FileStream(path + directory + fileName, FileMode.OpenOrCreate, FileAccess.Write);
                byte[] buff = new byte[4096];
                int c = 0;

                while ((c = reader.Read(buff, 0, buff.Length)) > 0)
                {
                    writer.Write(buff, 0, c);
                }
                writer.Close();
                writer.Dispose();
                reader.Close();
                reader.Dispose();
                response.Close();
            }
        }
    }
}