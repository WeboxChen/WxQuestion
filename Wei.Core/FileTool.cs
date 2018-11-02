using Ionic.Zip;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Wei.Core
{
    public class FileTool
    {
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="save"></param>
        public static void Compress(string[] filename, string save, string dir = "", Encoding encoding = default(Encoding))
        {
            if (encoding == default(Encoding))
                encoding = Encoding.Default;
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(encoding))
            {
                zip.AddFiles(filename, dir);
                zip.Save(save);
            }
        }
        public static void Compress(string zipfile, Dictionary<string, string> entrys, Encoding encoding = default(Encoding))
        {
            if (encoding == default(Encoding))
                encoding = Encoding.Default;
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(encoding))
            {
                foreach (var key in entrys.Keys)
                {
                    if (File.Exists(entrys[key]))
                        zip.AddEntry(key, File.OpenRead(entrys[key]));
                }
                zip.Save(zipfile);
            }
        }

        public static void CompressDir(string dirctoryname, string save, Encoding encoding = default(Encoding))
        {
            if (encoding == default(Encoding))
                encoding = Encoding.Default;
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(encoding))
            {
                zip.AddDirectory(dirctoryname);
                zip.Save(save);
            }
        }

        public static Stream CompressOutStream(string[] files, string dir = "", Encoding encoding = default(Encoding))
        {
            if (encoding == default(Encoding))
                encoding = Encoding.Default;
            MemoryStream stream = new MemoryStream();
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(encoding))
            {
                zip.AddFiles(files, dir);
                zip.Save(stream);
                stream.Position = 0;
            }
            return stream;
        }
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="files">文件夹 => 文件集合</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static Stream CompressOutStream(Dictionary<string, List<string>> files, Encoding encoding = default(Encoding))
        {
            if (encoding == default(Encoding))
                encoding = Encoding.Default;
            MemoryStream stream = new MemoryStream();
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(encoding))
            {
                foreach(string key in files.Keys)
                {
                    zip.AddFiles(files[key], key);
                }
                zip.Save(stream);
                stream.Position = 0;
            }
            return stream;
        }
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="entrys">新文件名，支持目录 => 原始文件名</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static Stream CompressOutStream(Dictionary<string, string> entrys, Encoding encoding = default(Encoding))
        {
            if (encoding == default(Encoding))
                encoding = Encoding.Default;
            MemoryStream stream = new MemoryStream();
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(encoding))
            {
                foreach (var key in entrys.Keys)
                {
                    if (File.Exists(entrys[key]))
                        zip.AddEntry(key, File.OpenRead(entrys[key]));
                }
                zip.Save(stream);
                stream.Position = 0;
            }
            return stream;
        }
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="dir">文件夹目录</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static Stream CompressDirOutStream(string dir, Encoding encoding = default(Encoding))
        {
            if (encoding == default(Encoding))
                encoding = Encoding.Default;
            MemoryStream stream = new MemoryStream();
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(encoding))
            {
                zip.AddDirectory(dir);
                zip.Save(stream);
                stream.Position = 0;
            }
            return stream;
        }

        /// <summary>
        /// zip 文件解压
        /// </summary>
        /// <param name="zipfile">zip 文件</param>
        /// <param name="unCompressTo">解压目录</param>
        /// <param name="isOverwrite">是否覆盖源文件</param>
        public static void UnCompress(string zipfile, string unCompressTo, bool isOverwrite = true)
        {
            ExtractExistingFileAction action = isOverwrite ? ExtractExistingFileAction.OverwriteSilently : ExtractExistingFileAction.DoNotOverwrite;

            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(zipfile))
            {
                zip.ExtractAll(unCompressTo, action);
            }
        }

        /// <summary>
        /// 检查zip文件中是否包含制定类型的文件
        /// </summary>
        /// <param name="zipfile"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static bool CheckZipFiles(string zipfile, string extension)
        {
            ReadOptions opt = new ReadOptions();
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(zipfile))
            {
                return zip.Any(x => Path.GetExtension(x.FileName).ToLower() == extension);
            }
        }

        /// <summary>
        /// 判断是否是zip文件
        /// </summary>
        /// <param name="zipfile"></param>
        /// <param name="isMult"></param>
        /// <returns></returns>
        public static bool IsZipFile(Stream zipfile, bool isMult = false)
        {
            return Ionic.Zip.ZipFile.IsZipFile(zipfile, isMult);
        }

        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="fromDir"></param>
        /// <param name="toDir"></param>
        public static void CopyDir(string fromDir, string toDir)
        {
            if (!Directory.Exists(fromDir))
                return;

            if (!Directory.Exists(toDir))
            {
                Directory.CreateDirectory(toDir);
            }

            string[] files = Directory.GetFiles(fromDir);
            foreach (string formFileName in files)
            {
                string fileName = Path.GetFileName(formFileName);
                string toFileName = Path.Combine(toDir, fileName);
                if (File.Exists(toFileName))
                    File.Delete(toFileName);
                File.Copy(formFileName, toFileName);
            }
            string[] fromDirs = Directory.GetDirectories(fromDir);
            foreach (string fromDirName in fromDirs)
            {
                string dirName = Path.GetFileName(fromDirName);
                string toDirName = Path.Combine(toDir, dirName);
                CopyDir(fromDirName, toDirName);
            }
        }

        /// <summary>
        /// 移动文件夹
        /// </summary>
        /// <param name="fromDir"></param>
        /// <param name="toDir"></param>
        public static void MoveDir(string fromDir, string toDir)
        {
            if (!Directory.Exists(fromDir))
                return;

            CopyDir(fromDir, toDir);
            Directory.Delete(fromDir, true);
        }
        /// <summary>
        /// 清空文件夹
        /// </summary>
        /// <param name="dir"></param>
        public static void ClearDirectory(string directory)
        {
            DirectoryInfo dinfo = new DirectoryInfo(directory);
            var dirs = dinfo.GetDirectories();
            foreach (var d in dirs)
            {
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        d.Delete(true);
                        break;
                    }
                    catch
                    {
                        System.Threading.Thread.Sleep(50);
                    }
                }
            }
            var files = dinfo.GetFiles();
            foreach (var f in files)
            {
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        f.Delete();
                        break;
                    }
                    catch
                    {
                        System.Threading.Thread.Sleep(50);
                    }
                }
            }
        }
    }
}
