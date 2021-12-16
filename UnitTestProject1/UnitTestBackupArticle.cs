using DotblogsBackup.Classes;
using DotblogsBackup.BackupTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTestBackupArticle
    {
        [TestMethod]
        public void ReadAllPage_Compare_Boolean()
        {
            //Arrange
            string dotblogs = "https://dotblogs.com.tw/";
            string userName = "brooke";

            Backup.SetDotblogsUrl(dotblogs, userName);

            BackupArticle BackupArticle = new BackupArticle();
            BackupArticle.ClearOld();
            BackupArticle.GetPageList();
            BackupArticle.Save();

            //Act
            Boolean result = CheckSum(BackupArticle.allArticle);

            //Assert
            Assert.IsTrue(result);

        }

        public Boolean CheckSum(List<DotblogIndex> page)
        {
            Boolean result = true;
            string[] sDir = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.html", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < sDir.Length; i++)
            {
                sDir[i] = sDir[i].Substring(sDir[i].LastIndexOf("\\") + 1);
                sDir[i] = sDir[i].Substring(0, sDir[i].LastIndexOf("."));
            }

            foreach (string d in sDir)
            {
                Boolean not_in_pageList = true;
                foreach (DotblogIndex pl in page)
                {
                    if (d == pl.title)
                    {
                        not_in_pageList = false;
                        result = result && not_in_pageList;
                    }
                }
                if (not_in_pageList) { Console.WriteLine(d + "not in pageList"); }
            }

            foreach (DotblogIndex pl in page)
            {
                Boolean not_in_DirPath = true;
                foreach (var d in sDir)
                {
                    if (pl.title == d)
                    {
                        not_in_DirPath = false;
                        result = result && not_in_DirPath;
                    }
                }
                if (not_in_DirPath) { Console.WriteLine(pl.title + "not in DirPath"); }
            }

            return result;
        }
    }
}
