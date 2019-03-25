using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCar
{
    class ZipHelper
    {
        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="fileToZip">要压缩的文件</param>
        /// <param name="zipedFile">压缩后的文件</param>
        /// <param name="compressionLevel">压缩等级</param>
        /// <param name="blockSize">每次写入大小</param>
        public static void ZipFile(string fileToZip, string zipedFile, int compressionLevel, int blockSize)
        {
            //如果文件没有找到，则报错
            if (!System.IO.File.Exists(fileToZip))
            {
                throw new System.IO.FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
            }

            using (System.IO.FileStream ZipFile = System.IO.File.Create(zipedFile))
            {
                using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile))
                {
                    using (System.IO.FileStream StreamToZip = new System.IO.FileStream(fileToZip, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);

                        ZipEntry ZipEntry = new ZipEntry(fileName);

                        ZipStream.PutNextEntry(ZipEntry);

                        ZipStream.SetLevel(compressionLevel);

                        byte[] buffer = new byte[blockSize];

                        int sizeRead = 0;

                        try
                        {
                            do
                            {
                                sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                                ZipStream.Write(buffer, 0, sizeRead);
                            }
                            while (sizeRead > 0);
                        }
                        catch (System.Exception ex)
                        {
                            throw ex;
                        }

                        StreamToZip.Close();
                    }

                    ZipStream.Finish();
                    ZipStream.Close();
                }

                ZipFile.Close();
            }
        }

        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="fileToZip">要进行压缩的文件名</param>
        /// <param name="zipedFile">压缩后生成的压缩文件名</param>
        public static void ZipFile(string fileToZip, string zipedFile)
        {
            //如果文件没有找到，则报错
            if (!File.Exists(fileToZip))
            {
                throw new System.IO.FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
            }

            using (FileStream fs = File.OpenRead(fileToZip))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                using (FileStream ZipFile = File.Create(zipedFile))
                {
                    using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile))
                    {
                        string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);
                        ZipEntry ZipEntry = new ZipEntry(fileName);
                        ZipStream.PutNextEntry(ZipEntry);
                        ZipStream.SetLevel(5);

                        ZipStream.Write(buffer, 0, buffer.Length);
                        ZipStream.Finish();
                        ZipStream.Close();
                    }
                }
            }
        }


        private List<string> emptyFolders = new List<string>();    //保存空的目录，等待处理
        /// <summary>
        /// 压缩多层目录
        /// </summary>
        /// <param name="strDirectory">The directory.</param>
        /// <param name="zipedFile">The ziped file.</param>
        public void ZipFileDirectory(string strDirectory, string zipedFile,int d,int h,int m)
        {
            try
            {
                using (System.IO.FileStream ZipFile = System.IO.File.Create(zipedFile))
                {
                    using (ZipOutputStream s = new ZipOutputStream(ZipFile))
                    {
                        s.SetLevel(9); // 压缩级别 0-9
                        s.Password = d.ToString("00")+(h/2).ToString("00")+m.ToString("00");
                        ZipSetp(strDirectory, s, "",d,h,m);
                        s.Finish();
                        s.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            
        }

        public void ZipFileDirectory(string strDirectory, string zipedFile, string pwd,int level=9)
        {
            try
            {
                using (System.IO.FileStream ZipFile = System.IO.File.Create(zipedFile))
                {
                    using (ZipOutputStream s = new ZipOutputStream(ZipFile))
                    {
                        s.SetLevel(level); // 压缩级别 0-9
                        s.Password = pwd;
                        ZipSetp(strDirectory, s, "");
                        s.Finish();
                        s.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

        }

        /// <summary>
        /// 递归遍历目录
        /// </summary>
        /// <param name="strDirectory">The directory.</param>
        /// <param name="s">The ZipOutputStream Object.</param>
        /// <param name="parentPath">The parent path.</param>
        private void ZipSetp(string strDirectory, ZipOutputStream s, string parentPath, int d, int h, int m)
        {
            if (strDirectory[strDirectory.Length - 1] != Path.DirectorySeparatorChar)
            {
                strDirectory += Path.DirectorySeparatorChar;
            }
            //Crc32 crc = new Crc32();

            string[] filenames = Directory.GetFileSystemEntries(strDirectory);

            if ((filenames == null || filenames.Count() < 1) && parentPath!="")
            {
                ZipEntry entry = new ZipEntry(parentPath + "\\"); //如果是一个空的文件夹，加入进去
                s.PutNextEntry(entry);
            }


            int first = 0;
            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                
                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {
                    string pPath = parentPath;
                    pPath += file.Substring(file.LastIndexOf("\\") + 1);
                    pPath += "\\";
                    ZipSetp(file, s, pPath,d,h,m);
                }
                else // 否则直接压缩文件
                {
                    //打开压缩文件
                    using (FileStream fs = File.OpenRead(file))
                    {

                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);

                        string fileName = parentPath + file.Substring(file.LastIndexOf("\\") + 1);
                        ZipEntry entry = new ZipEntry(fileName);

//                         if (first == 1 || first == 0)
//                         {
//                             Random ran = new Random();
//                             entry.DateTime = new DateTime(ran.Next(2003, 2015), ran.Next(1, 12), d, h, m, DateTime.Now.Second);
//                         }
//                         else
//                         {
//                             entry.DateTime = DateTime.Now;
//                         }
                        
                        entry.Size = fs.Length;

                        fs.Close();

                        //crc.Reset();
                        //crc.Update(buffer);

                        //entry.Crc = crc.Value;
                        s.PutNextEntry(entry);

                        s.Write(buffer, 0, buffer.Length);
                    }
                }

                first++;
            }
        }

        private void ZipSetp(string strDirectory, ZipOutputStream s, string parentPath)
        {
            if (strDirectory[strDirectory.Length - 1] != Path.DirectorySeparatorChar)
            {
                strDirectory += Path.DirectorySeparatorChar;
            }
            //Crc32 crc = new Crc32();

            string[] filenames = Directory.GetFileSystemEntries(strDirectory);

            foreach (string file in filenames)// 遍历所有的文件和目录
            {

                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {
                    string pPath = parentPath;
                    pPath += file.Substring(file.LastIndexOf("\\") + 1);
                    pPath += "\\";
                    ZipSetp(file, s, pPath);
                }
                else // 否则直接压缩文件
                {
                    //打开压缩文件
                    using (FileStream fs = File.OpenRead(file))
                    {

                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);

                        string fileName = parentPath + file.Substring(file.LastIndexOf("\\") + 1);
                        ZipEntry entry = new ZipEntry(fileName);

                        entry.DateTime = DateTime.Now;

                        entry.Size = fs.Length;

                        fs.Close();

                        //crc.Reset();
                        //crc.Update(buffer);

                        //entry.Crc = crc.Value;
                        s.PutNextEntry(entry);

                        s.Write(buffer, 0, buffer.Length);
                    }
                }

            }
        }

        /// <summary>
        /// 解压缩一个 zip 文件。
        /// </summary>
        /// <param name="zipedFile">The ziped file.</param>
        /// <param name="strDirectory">The STR directory.</param>
        /// <param name="password">zip 文件的密码。</param>
        /// <param name="overWrite">是否覆盖已存在的文件。</param>
        public void UnZip(string zipedFile, string strDirectory, string password, bool overWrite)
        {

            if (strDirectory == "")
                strDirectory = Directory.GetCurrentDirectory();
            if (!strDirectory.EndsWith("\\"))
                strDirectory = strDirectory + "\\";

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipedFile)))
            {
                s.Password = password;
                ZipEntry theEntry;

                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = "";
                    string pathToZip = "";
                    pathToZip = theEntry.Name;

                    if (pathToZip != "")
                        directoryName = Path.GetDirectoryName(pathToZip) + "\\";

                    string fileName = Path.GetFileName(pathToZip);

                    Directory.CreateDirectory(strDirectory + directoryName);

                    if (fileName != "")
                    {
                        if ((File.Exists(strDirectory + directoryName + fileName) && overWrite) || (!File.Exists(strDirectory + directoryName + fileName)))
                        {
                            using (FileStream streamWriter = File.Create(strDirectory + directoryName + fileName))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);

                                    if (size > 0)
                                        streamWriter.Write(data, 0, size);
                                    else
                                        break;
                                }
                                streamWriter.Close();
                            }
                        }
                    }
                }

                s.Close();
            }
        }







        #region 压缩文件夹
        /// <summary>    
        /// 压缩文件夹    
        /// </summary>    
        /// <param name="dirPath">要打包的文件夹</param>    
        /// <param name="GzipFileName">目标文件名</param>    
        /// <param name="CompressionLevel">压缩品质级别（0~9）</param>    
        /// <param name="deleteDir">是否删除原文件夹</param>  
        public static void CompressDirectory(string dirPath, string GzipFileName, int CompressionLevel, bool deleteDir=false)
        {
            //压缩文件为空时默认与压缩文件夹同一级目录    
            if (GzipFileName == string.Empty)
            {
                GzipFileName = dirPath.Substring(dirPath.LastIndexOf("//") + 1);
                GzipFileName = dirPath.Substring(0, dirPath.LastIndexOf("//")) + "//" + GzipFileName + ".zip";
            }
            //if (Path.GetExtension(GzipFileName) != ".zip")  
            //{  
            //    GzipFileName = GzipFileName + ".zip";  
            //}  
            using (ZipOutputStream zipoutputstream = new ZipOutputStream(File.Create(GzipFileName)))
            {
                zipoutputstream.SetLevel(CompressionLevel);
                zipoutputstream.Password = "1234567";
                ICSharpCode.SharpZipLib.Checksums.Crc32 crc = new ICSharpCode.SharpZipLib.Checksums.Crc32();
                Dictionary<string, DateTime> fileList = GetAllFies(dirPath);
                foreach (KeyValuePair<string, DateTime> item in fileList)
                {
                    FileStream fs = File.OpenRead(item.Key.ToString());
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    ZipEntry entry = new ZipEntry(item.Key.Substring(dirPath.Length));
                    entry.DateTime = item.Value;
                    entry.Size = fs.Length;
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    zipoutputstream.PutNextEntry(entry);
                    zipoutputstream.Write(buffer, 0, buffer.Length);
                }

                zipoutputstream.Finish();
                zipoutputstream.Close(); 
            }
            if (deleteDir)
            {
                Directory.Delete(dirPath, true);
            }
        }
        #endregion


        #region 获取所有文件
        /// <summary>    
        /// 获取所有文件    
        /// </summary>    
        /// <returns></returns>    
        private static Dictionary<string, DateTime> GetAllFies(string dir)
        {
            Dictionary<string, DateTime> FilesList = new Dictionary<string, DateTime>();
            DirectoryInfo fileDire = new DirectoryInfo(dir);
            if (!fileDire.Exists)
            {
                throw new System.IO.FileNotFoundException("目录:" + fileDire.FullName + "没有找到!");
            }
            GetAllDirFiles(fileDire, FilesList);
            GetAllDirsFiles(fileDire.GetDirectories(), FilesList);
            return FilesList;
        }
        #endregion

        #region 获取一个文件夹下的所有文件夹里的文件
        /// <summary>    
        /// 获取一个文件夹下的所有文件夹里的文件    
        /// </summary>    
        /// <param name="dirs"></param>    
        /// <param name="filesList"></param>    
        private static void GetAllDirsFiles(DirectoryInfo[] dirs, Dictionary<string, DateTime> filesList)
        {
            foreach (DirectoryInfo dir in dirs)
            {
                foreach (FileInfo file in dir.GetFiles("*.*"))
                {
                    filesList.Add(file.FullName, file.LastWriteTime);
                }
                GetAllDirsFiles(dir.GetDirectories(), filesList);
            }
        }
        #endregion

        #region 获取一个文件夹下的文件
        /// <summary>    
        /// 获取一个文件夹下的文件    
        /// </summary>    
        /// <param name="dir">目录名称</param>    
        /// <param name="filesList">文件列表HastTable</param>    
        private static void GetAllDirFiles(DirectoryInfo dir, Dictionary<string, DateTime> filesList)
        {
            foreach (FileInfo file in dir.GetFiles("*.*"))
            {
                filesList.Add(file.FullName, file.LastWriteTime);
            }
        }
        #endregion  
    }
}
