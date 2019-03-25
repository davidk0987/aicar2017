using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XFYY
{
    class XFYY_Api
    {
        #region msc dll import

        public enum SynthStatus
        {
            MSP_TTS_FLAG_STILL_HAVE_DATA = 1,
            MSP_TTS_FLAG_DATA_END = 2,
            MSP_TTS_FLAG_CMD_CANCELED = 0
        }
        [DllImport("msc.dll", CallingConvention = CallingConvention.StdCall)]
        //[DllImport("msc.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int MSPLogin(string user, string password, string configs);

        [DllImport("msc.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int MSPLogout();

        [DllImport("msc.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr QTTSSessionBegin(string _params, ref int errorCode);

        [DllImport("msc.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int QTTSTextPut(string sessionID, string textString, uint textLen, string _params);

        [DllImport("msc.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr QTTSAudioGet(string sessionID, ref uint audioLen, ref SynthStatus synthStatus, ref int errorCode);

        [DllImport("msc.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr QTTSAudioInfo(string sessionID);

        [DllImport("msc.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int QTTSSessionEnd(string sessionID, string hints);




        /// <summary>
        /// 结构体初始化赋值
        /// </summary>
        /// <param name="data_len"></param>
        /// <returns></returns>
        private static XFYY_Api_WAVE_Header XFYY_Api_getWave_Header(int data_len)
        {
            return new XFYY_Api_WAVE_Header
            {
                RIFF_ID = 1179011410,
                File_Size = data_len + 36,
                RIFF_Type = 1163280727,
                FMT_ID = 544501094,
                FMT_Size = 16,
                FMT_Tag = 1,
                FMT_Channel = 1,
                FMT_SamplesPerSec = 16000,
                AvgBytesPerSec = 32000,
                BlockAlign = 2,
                BitsPerSample = 16,
                DATA_ID = 1635017060,
                DATA_Size = data_len
            };
        }
        /// <summary>
        /// 语音音频头
        /// </summary>
        private struct XFYY_Api_WAVE_Header
        {
            public int RIFF_ID;
            public int File_Size;
            public int RIFF_Type;
            public int FMT_ID;
            public int FMT_Size;
            public short FMT_Tag;
            public ushort FMT_Channel;
            public int FMT_SamplesPerSec;
            public int AvgBytesPerSec;
            public ushort BlockAlign;
            public ushort BitsPerSample;
            public int DATA_ID;
            public int DATA_Size;
        }


        #endregion


        public static void XFYY_Api_SaveVoice2Locla(string strtext,string strfilepath)
        {
            int ret = -1;


            if (strtext == "")
            {
                return;
            }


            int XFYY_Api_ret = -1;
            IntPtr XFYY_Api_session_ID = IntPtr.Zero;
            string XFYY_Api_strsession_ID = "";

            string login_configs = "appid = 57070687, work_dir = .";//登录参数,自己注册后获取的appid
            string _params = "engine_type = local, voice_name = xiaoyan, text_encoding = GB2312, tts_res_path = fo|res\\tts\\xiaoyan.jet;fo|res\\tts\\common.jet, sample_rate = 16000, speed = 50, volume = 70, pitch = 50, rdn = 2";



            SynthStatus synth_status = SynthStatus.MSP_TTS_FLAG_STILL_HAVE_DATA;
            if (XFYY_Api_ret != 0)//如果没有初始化，初始化函数，下次直接使用
            {
                XFYY_Api_ret = MSPLogin(string.Empty, string.Empty, login_configs);//第一个参数为用户名，第二个参数为密码，第三个参数是登录参数，用户名和密码需要在http://open.voicecloud.cn
                //MSPLogin方法返回失败
                if (XFYY_Api_ret != 0)
                {
                    return;
                }
            }

            //             XFYY_Api_ret = MSPLogin(string.Empty, string.Empty, login_configs);//第一个参数为用户名，第二个参数为密码，第三个参数是登录参数，用户名和密码需要在http://open.voicecloud.cn
            //             //MSPLogin方法返回失败
            //             if (XFYY_Api_ret != 0)
            //             {
            //                 return;
            //             }


            if (XFYY_Api_session_ID == IntPtr.Zero)
            {

                XFYY_Api_session_ID = QTTSSessionBegin(_params, ref ret);
                if (ret != 0)
                {
                    return;
                }
            }

            //             XFYY_Api_session_ID = QTTSSessionBegin(_params, ref ret);
            //             if (ret != 0)
            //             {
            //                 return;
            //             }

            if (XFYY_Api_strsession_ID == "")
            {
                XFYY_Api_strsession_ID = XFYY_Api_Ptr2Str(XFYY_Api_session_ID);
            }

            //XFYY_Api_strsession_ID = XFYY_Api_Ptr2Str(XFYY_Api_session_ID);




            uint ulen = (uint)System.Text.ASCIIEncoding.Default.GetBytes(strtext).Length;

            ret = QTTSTextPut(XFYY_Api_strsession_ID, strtext, ulen, string.Empty);
            //QTTSTextPut方法返回失败
            if (ret != 0)
            {
                return;
            }



            uint audio_len = 0;
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(new byte[44], 0, 44);
            while (true)
            {
                IntPtr source = QTTSAudioGet(XFYY_Api_strsession_ID, ref audio_len, ref synth_status, ref ret);
                byte[] array = new byte[(int)audio_len];
                if (audio_len > 0)
                {
                    Marshal.Copy(source, array, 0, (int)audio_len);
                }
                memoryStream.Write(array, 0, array.Length);
                Thread.Sleep(50);
                if (synth_status == SynthStatus.MSP_TTS_FLAG_DATA_END || ret != 0)
                    break;
            }
            XFYY_Api_WAVE_Header wave_Header = XFYY_Api_getWave_Header((int)memoryStream.Length - 44);


            //XFYY_Api_WAVE_Header wave_Header = new XFYY_Api_WAVE_Header();



            byte[] array2 = XFYY_Api_StructToBytes(wave_Header);
            memoryStream.Position = 0L;
            memoryStream.Write(array2, 0, array2.Length);
            memoryStream.Position = 0L;


            using (FileStream fs = new FileStream(strfilepath, FileMode.Create))
            {
                byte[] buff = memoryStream.ToArray();
                fs.Write(buff, 0, buff.Length);
            }


            SoundPlayer soundPlayer = new SoundPlayer(memoryStream);
            soundPlayer.Stop();
            soundPlayer.Play();
            memoryStream.Close();


            QTTSSessionEnd(XFYY_Api_strsession_ID, "");
            MSPLogout();//退出登录
        }

        public static void XFYY_Api_Text2Voice(string strtext)
        {
            int ret = -1;


            if(strtext=="")
            {
                return;
            }


            int    XFYY_Api_ret           = -1;
            IntPtr XFYY_Api_session_ID    = IntPtr.Zero;
            string XFYY_Api_strsession_ID = "";

            string login_configs = "appid = 57070687, work_dir = .";//登录参数,自己注册后获取的appid
            string _params = "engine_type = local, voice_name = xiaoyan, text_encoding = GB2312, tts_res_path = fo|res\\tts\\xiaoyan.jet;fo|res\\tts\\common.jet, sample_rate = 16000, speed = 50, volume = 50, pitch = 50, rdn = 2";



            SynthStatus synth_status = SynthStatus.MSP_TTS_FLAG_STILL_HAVE_DATA;
            if (XFYY_Api_ret != 0)//如果没有初始化，初始化函数，下次直接使用
            {
                XFYY_Api_ret = MSPLogin(string.Empty, string.Empty, login_configs);//第一个参数为用户名，第二个参数为密码，第三个参数是登录参数，用户名和密码需要在http://open.voicecloud.cn
                //MSPLogin方法返回失败
                if (XFYY_Api_ret != 0)
                {
                    return;
                }
            }

//             XFYY_Api_ret = MSPLogin(string.Empty, string.Empty, login_configs);//第一个参数为用户名，第二个参数为密码，第三个参数是登录参数，用户名和密码需要在http://open.voicecloud.cn
//             //MSPLogin方法返回失败
//             if (XFYY_Api_ret != 0)
//             {
//                 return;
//             }


            if (XFYY_Api_session_ID == IntPtr.Zero)
            {

                XFYY_Api_session_ID = QTTSSessionBegin(_params, ref ret);
                if (ret != 0)
                {
                    return;
                }
            }

//             XFYY_Api_session_ID = QTTSSessionBegin(_params, ref ret);
//             if (ret != 0)
//             {
//                 return;
//             }

            if (XFYY_Api_strsession_ID == "")
            {
                XFYY_Api_strsession_ID = XFYY_Api_Ptr2Str(XFYY_Api_session_ID);
            }

            //XFYY_Api_strsession_ID = XFYY_Api_Ptr2Str(XFYY_Api_session_ID);




            uint ulen = (uint)System.Text.ASCIIEncoding.Default.GetBytes(strtext).Length;

            ret = QTTSTextPut(XFYY_Api_strsession_ID, strtext, ulen, string.Empty);
            //QTTSTextPut方法返回失败
            if (ret != 0)
            {
                return;
            }



            uint audio_len = 0;
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(new byte[44], 0, 44);
            while (true)
            {
                IntPtr source = QTTSAudioGet(XFYY_Api_strsession_ID, ref audio_len, ref synth_status, ref ret);
                byte[] array = new byte[(int)audio_len];
                if (audio_len > 0)
                {
                    Marshal.Copy(source, array, 0, (int)audio_len);
                }
                memoryStream.Write(array, 0, array.Length);
                //Thread.Sleep(50);
                if (synth_status == SynthStatus.MSP_TTS_FLAG_DATA_END || ret != 0)
                    break;
            }
            XFYY_Api_WAVE_Header wave_Header = XFYY_Api_getWave_Header((int)memoryStream.Length - 44);


            //XFYY_Api_WAVE_Header wave_Header = new XFYY_Api_WAVE_Header();



            byte[] array2 = XFYY_Api_StructToBytes(wave_Header);

            memoryStream.Position = 0L;
            memoryStream.Write(array2, 0, array2.Length);
            memoryStream.Position = 0L;

            
            SoundPlayer soundPlayer = new SoundPlayer(memoryStream);
            soundPlayer.Stop();
            soundPlayer.Play();


            



            memoryStream.Close();


            QTTSSessionEnd(XFYY_Api_strsession_ID, "");
            MSPLogout();//退出登录
        }


        private static byte[] XFYY_Api_StructToBytes(object structure)
        {
            int num = Marshal.SizeOf(structure);
            IntPtr intPtr = Marshal.AllocHGlobal(num);
            byte[] result;
            try
            {
                Marshal.StructureToPtr(structure, intPtr, false);
                byte[] array = new byte[num];
                Marshal.Copy(intPtr, array, 0, num);
                result = array;
            }
            finally
            {
                Marshal.FreeHGlobal(intPtr);
            }
            return result;
        }
        ////////////////////////

        /// 指针转字符串
        /// </summary>
        /// <param name="p">指向非托管代码字符串的指针</param>
        /// <returns>返回指针指向的字符串</returns>
        public static string XFYY_Api_Ptr2Str(IntPtr p)
        {
            List<byte> lb = new List<byte>();
            while (Marshal.ReadByte(p) != 0)
            {
                lb.Add(Marshal.ReadByte(p));
                p = p + 1;
            }
            byte[] bs = lb.ToArray();
            return Encoding.Default.GetString(lb.ToArray());
        }
    }
}
