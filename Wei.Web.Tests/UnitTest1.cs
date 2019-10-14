using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wei.Core;

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

        [TestMethod]
        public void TestMethod2()
        {
            var source = "八hello三一OO";
            var str = DataConvertHelper.ChineseNumberAnalysis(source);
            Console.WriteLine($"source: {source}");
            Console.WriteLine($"result: {str}");
        }

        [TestMethod]
        public void TestMethod3()
        {
            decimal num  = decimal.Floor(7.31m + 1);
            Console.WriteLine(num);
        }
    }
}
