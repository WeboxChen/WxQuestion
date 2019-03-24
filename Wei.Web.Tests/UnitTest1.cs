using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Wei.Web.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string file = @"E:\work\WXQuestions\WXQuestion\Wei.Web\efnjb8pksl1rU7He2jxZXETjCd7qMYvGxNULsAKbfPCJjJi3dsC53gJQdHzyuGVN.amr";
            FileInfo finfo = new FileInfo(file);
            var stream = finfo.OpenRead();

            byte[] bytes = bytes = new byte[stream.Length];

            stream.Read(bytes, 0, bytes.Length);
            string voice = Convert.ToBase64String(bytes);
            Console.WriteLine(voice);
        }
    }
}
