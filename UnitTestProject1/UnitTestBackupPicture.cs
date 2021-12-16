using DotblogsBackup.Classes;
using DotblogsBackup.BackupTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTestBackupPicture
    {
        private BackupPicture BackupPicture = new BackupPicture();
        [TestMethod]
        public void CheckSinglePagePictures_Get_int()
        {
            //Arrage
            DotblogIndex aDotblog = new DotblogIndex("單元測試 Unit Test", "https://dotblogs.com.tw/brooke/2015/12/16/222039", null);

            BackupPicture.allArticle.Add(aDotblog);
            BackupPicture.GetPictureList();

            //Act
            int result = CountPicture(BackupPicture.allArticle); ;

            //Assert
            Assert.AreEqual(4, result);
        }

        private int CountPicture(System.Collections.Generic.List<global::DotblogsBackup.Classes.DotblogIndex> pictureCollections)
        {
            int Counter = 0;
            foreach (var item in pictureCollections)
            {
                foreach (var item2 in item.pictures)
                {
                    Counter += 1;
                }
            }
            return Counter;
        }

        [TestMethod]
        public void IntegrationTestBackupPicture_Call_void()
        {
            BackupPicture.ClearOld();
            CheckSinglePagePictures_Get_int();
            BackupPicture.Save();
        }
    }
}
