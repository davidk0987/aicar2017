using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Media.Imaging;

namespace AiCar
{
    public enum System_Message_Info
    {
        notification    = 0,
        ksxt            ,
        xtxx            ,
        PowerOnSelfTest ,
        getradionum     ,
        gettran         ,
        setiohearttime  ,
        getiojx         ,
        updatefile      
    }

    class Helper
    {
       
        /*
             //
    // 摘要:
    //     指定一周的某天。
    [ComVisible(true)]
    public enum DayOfWeek
    {
        //
        // 摘要:
        //     表示星期日。
        Sunday = 0,
        //
        // 摘要:
        //     表示星期一。
        Monday = 1,
        //
        // 摘要:
        //     表示星期二。
        Tuesday = 2,
        //
        // 摘要:
        //     表示星期三。
        Wednesday = 3,
        //
        // 摘要:
        //     表示星期四。
        Thursday = 4,
        //
        // 摘要:
        //     表示星期五。
        Friday = 5,
        //
        // 摘要:
        //     表示星期六。
        Saturday = 6
    }
             
             */
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);
        public static string IniReadValue(string Section, string Key, string lpfilename)
        {
            StringBuilder temp = new StringBuilder(50000);

            //byte[] buff = new byte[5000];


            int i = GetPrivateProfileString(Section, Key, "", temp, 5000, lpfilename);

            //return System.Text.Encoding.Default.GetString(buff);
            return temp.ToString().Trim('\0');
        }


        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        /*写INI文件*/
        public static void IniWriteValue(string Section, string Key, string Value, string filePath)
        {
            WritePrivateProfileString(Section, Key, Value, filePath);
        }


        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int byte_tx_invork(int ret);

        public static int tx_invork(int dat)
        {
            try
            {
                return byte_tx_invork(dat);
            }
            catch (Exception)
            {
                return 0;
            }

        }

        

        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int byte_rx_invork(int ret);

        public static int rx_invork(int dat)
        {
            try
            {
                return byte_rx_invork(dat);
            }
            catch (Exception)
            {
                return 0;
            }

        }
        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int ByteInvers(int ret);
        public static int invers(int ret)
        {
            try
            {
                return ByteInvers(ret);
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public static void popupkeyboard()
        {
            Process.Start(Environment.GetEnvironmentVariable("systemdrive") + @"\WINDOWS\system32\osk.exe");
        }


        public static string Helper_GetDayName(DayOfWeek today)
        {
            string result = "";

            //DateTime today = DateTime.Now;
            if (today == DayOfWeek.Monday)
            {
                result = "星期一";
            }
            else if (today == DayOfWeek.Tuesday)
            {
                result = "星期二";
            }
            else if (today == DayOfWeek.Wednesday)
            {
                result = "星期三";
            }
            else if (today == DayOfWeek.Thursday)
            {
                result = "星期四";
            }
            else if (today == DayOfWeek.Friday)
            {
                result = "星期五";
            }
            else if (today == DayOfWeek.Saturday)
            {
                result = "星期六";
            }
            else if (today == DayOfWeek.Sunday)
            {
                result = "星期日";
            }

            return result;
        }

        public static byte[] getbyte(string codestr)//二进制字符串转16进制
        {
            byte[] encodebyte = new byte[codestr.Length / 8];
            for (int i = 0; i < codestr.Length / 8; i++)
            {
                encodebyte[i] = Convert.ToByte(codestr.Substring(i * 8, 8), 2);
            }
            return encodebyte;
        }

        public static string Helper_GetGpsStatus(string gpsstatus)
        {
            if(gpsstatus=="0")
            {
                return "无效";
            }
            else if (gpsstatus == "1")
            {
                return "单点";
            }
            else if (gpsstatus == "2")
            {
                return "浮点";
            }
            else if (gpsstatus == "3")
            {
                return "固定";
            }

            return gpsstatus;
        }

        //public static BitmapImage Helper_BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        //    BitmapImage bit3 = new BitmapImage();
        //    bit3.BeginInit();
        //    bit3.StreamSource = ms;
        //    bit3.EndInit();
        //    return bit3;
        //}

        public static string RepairZero(string text, int limitedLength)
        {
            //补足0的字符串
            string temp = "";

            //补足0
            for (int i = 0; i < limitedLength - text.Length; i++)
            {
                temp += "0";
            }

            //连接text
            temp += text;

            //返回补足0的字符串
            return temp;
        }

        public static byte[] int16ToBytes(int value)
        {
            byte[] src = new byte[2];
            src[1] = (byte)((value >> 8) & 0xFF);
            src[0] = (byte)(value & 0xFF);
            return src;
        }

        public static byte[] intToBytes(int value)
        {
            byte[] src = new byte[4];
            src[3] = (byte)((value >> 24) & 0xFF);
            src[2] = (byte)((value >> 16) & 0xFF);
            src[1] = (byte)((value >> 8) & 0xFF);
            src[0] = (byte)(value & 0xFF);
            return src;
        }

        public static int Helper_XoryanzhenBTstring(byte[] Bytes)//0183格式 校验码计算
        {
            int i, result;
            for (result = Bytes[0], i = 1; i < Bytes.Length; i++)
            {
                result ^= Bytes[i];
            }
            return result;
        }

        public static string Helper_StrToMD5(string str)
        {
            byte[] data = Encoding.GetEncoding("GB2312").GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] OutBytes = md5.ComputeHash(data);

            string OutString = "";
            for (int i = 0; i < OutBytes.Length; i++)
            {
                OutString += OutBytes[i].ToString("x2");
            }
            // return OutString.ToUpper();
            return OutString.ToLower();
        }


        public static DataTable JsonToDataTable(string json)
        {
            json = "[" + json + "]";
            DataTable dataTable = new DataTable();  //实例化

            DataTable result;

            try
            {

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值

                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);

                if (arrayList.Count > 0)
                {

                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {

                        if (dictionary.Keys.Count == 0)
                        {

                            result = dataTable;

                            return result;

                        }

                        if (dataTable.Columns.Count == 0)
                        {

                            foreach (string current in dictionary.Keys)
                            {

                                dataTable.Columns.Add(current, dictionary[current].GetType());

                            }

                        }

                        DataRow dataRow = dataTable.NewRow();

                        foreach (string current in dictionary.Keys)
                        {

                            dataRow[current] = dictionary[current];

                        }



                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中

                    }

                }

            }

            catch
            {

            }

            result = dataTable;

            return result;

        }

        public static string GetMD5(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.Unicode.GetBytes(myString);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("X2");
            }

            return byte2String;
        }

        public static void PlayerText2MP3(string strText)
        {
            if (strText == "")
            {
                return;
            }

            string strfilepath = "\\sound\\" + GetMD5(strText) + ".wav";

            try
            {
                strfilepath = AppDomain.CurrentDomain.BaseDirectory + strfilepath;
                if (File.Exists(strfilepath) == false)
                {
                    //如果文件不存在,创建文件
                    XFYY.XFYY_Api.XFYY_Api_SaveVoice2Locla(strText, strfilepath);
                }
                else
                {
                    System.Media.SoundPlayer sp = new System.Media.SoundPlayer();
                    sp.SoundLocation = /*AppDomain.CurrentDomain.BaseDirectory +*/ strfilepath;
                    //sp.PlaySync();//同步
                    sp.Play();//异步
                }

                

            }
            catch { }
        }


        #region
        /// <summary>
        /// 围栏计算(点是否在围栏内)
        /// </summary>
        /// <param name="latlon">单点坐标</param>
        /// <param name="APoints">坐标集合</param>
        /// <returns></returns>
        public static bool MBR(Gps latlon, List<Gps> APoints)
        {
            if (MBR_zerOne(latlon, APoints))
            {
                return IsPtInPoly(latlon, APoints);//内判断
            }
            else
            {
                return false;//外判断
            }
        }
        /// <summary>
        /// 最小外包法
        /// </summary>
        /// <param name="latlon"></param>
        /// <param name="APoints"></param>
        /// <returns></returns>
        private static bool MBR_zerOne(Gps latlon, List<Gps> APoints)
        {
            double max_lon = APoints.Max(x => x.getWgLon());
            double max_lat = APoints.Max(x => x.getWgLat());
            double min_lon = APoints.Min(x => x.getWgLon());
            double min_lat = APoints.Min(x => x.getWgLat());
            double aLon = latlon.getWgLon();
            double aLat = latlon.getWgLat();
            if (aLon >= max_lon || aLon <= min_lon || aLat >= max_lat || aLat <= min_lat)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 点在围栏内（数学计算方法）
        /// </summary>
        /// <param name="latlon">Gps坐标</param>
        /// <param name="APoints">Gps坐标点集合</param>
        /// <returns></returns>
        private static bool IsPtInPoly(Gps latlon, List<Gps> APoints)
        {
            int iSum = 0, iCount;
            double dLon1, dLon2, dLat1, dLat2, dLon;
            double ALat = latlon.getWgLat();
            double ALon = latlon.getWgLon();
            if (APoints.Count < 3)
                return false;
            iCount = APoints.Count;
            for (int i = 0; i < iCount; i++)
            {
                if (i == iCount - 1)
                {
                    dLon1 = APoints[i].getWgLon();
                    dLat1 = APoints[i].getWgLat();
                    dLon2 = APoints[0].getWgLon();
                    dLat2 = APoints[0].getWgLat();
                }
                else
                {
                    dLon1 = APoints[i].getWgLon();
                    dLat1 = APoints[i].getWgLat();
                    dLon2 = APoints[i + 1].getWgLon();
                    dLat2 = APoints[i + 1].getWgLat();
                }
                //以下语句判断A点是否在边的两端点的水平平行线之间，在则可能有交点，开始判断交点是否在左射线上
                if (((ALat >= dLat1) && (ALat < dLat2)) || ((ALat >= dLat2) && (ALat < dLat1)))
                {
                    if (Math.Abs(dLat1 - dLat2) > 0)
                    {
                        //得到 A点向左射线与边的交点的x坐标：
                        dLon = dLon1 - ((dLon1 - dLon2) * (dLat1 - ALat)) / (dLat1 - dLat2);

                        // 如果交点在A点左侧（说明是做射线与 边的交点），则射线与边的全部交点数加一：
                        if (dLon < ALon)
                            iSum++;
                    }
                }
            }
            if (iSum % 2 != 0)
                return true;
            return false;
        }
        #endregion



    }
}
