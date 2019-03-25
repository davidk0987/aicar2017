using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AiCar
{
    public class Device_CMD
    {
        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void Device_CMD_0x0503(byte[] outbuf1,byte[] outbuf2);
        public static byte[] Set_Device_CMD_0x0503(string strZCM)//写入注册码
        {
            string zcm = strZCM.Replace("-", "").Replace(" ", "").ToUpper();

            byte[] outbuf1 = new byte[24];
            try
            {
                Device_CMD_0x0503(outbuf1, System.Text.ASCIIEncoding.Default.GetBytes(zcm));
            }
            catch { }


            return outbuf1;
        }

        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void Device_CMD_0x0603(byte[] outbuf1, byte[] outbuf2);
        public static byte[] Set_Device_CMD_0x0603(string strIOJX)//设置IO极性
        {
           // string zcm = strZCM.Replace("-", "").Replace(" ", "").ToUpper();
            byte[] b_iojx = Helper.getbyte(strIOJX);
            byte[] outbuf1 = new byte[12];
            try
            {
                Device_CMD_0x0603(outbuf1, b_iojx);
            }
            catch { }


            return outbuf1;
        }

        public static int CheckCMDHead(byte[] b_cmd)
        {
            byte b_hi = (byte)Helper.rx_invork(b_cmd[0]);
            byte b_lo = (byte)Helper.rx_invork(b_cmd[1]);

            if (b_lo == 0x05)
            {
                if (b_hi == 0x04)
                {
                    return 1;//返回1为注册码设置成功
                }
            }

            if (b_lo == 0x03)
            {
                if (b_hi == 0x0d)
                {
                    return 2;//返回1为注册码设置成功
                }
            }

            if (b_lo == 0x05)
            {
                if (b_hi == 0x02)
                {
                    return 3;//返回读取的注册码
                }
            }

            if (b_lo == 0x03)
            {
                if (b_hi == 0x0b)
                {
                    return 4;//返回设置通道号成功
                }
            }

            if (b_lo == 0x04)
            {
                if (b_hi == 0x02)
                {
                    return 5;//返回读取基站坐标
                }
            }

            if (b_lo == 0x04)
            {
                if (b_hi == 0x04)
                {
                    return 6;//返回1为注册码设置成功
                }
            }

            if (b_lo == 0x03)
            {
                if (b_hi == 0x10)
                {
                    return 7;//返回请求升级指令
                }
            }

            if (b_lo == 0x07)
            {
                if (b_hi == 0x02)
                {
                    return 8;//返回机身号
                }
            }

            if (b_lo == 0x06)
            {
                if (b_hi == 0x02)
                {
                    return 9;//返回IO极性
                }
            }

            if (b_lo == 0x06)
            {
                if (b_hi == 0x04)
                {
                    return 10;//设置IO极性回应
                }
            }

            if (b_lo == 0x08)
            {
                if (b_hi == 0x02)
                {
                    return 11;//设置车型文件返回
                }
            }

            if (b_lo == 0x06)
            {
                if (b_hi == 0x06)
                {
                    return 12;//获取OBD启用状态
                }
            }

            if (b_lo == 0x06)
            {
                if (b_hi == 0x08)
                {
                    return 13;//设置OBD启用状态
                }
            }

            if (b_lo == 0x09)
            {
                if (b_hi == 0x04)
                {
                    return 14;//获取差分传输模式
                }
            }

            if (b_lo == 0x09)
            {
                if (b_hi == 0x06)
                {
                    return 15;//设置差分传输模式
                }
            }

            return 0;
        }


        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void Device_CMD_0x030C(byte[] ret);
        public static byte[] Get_Device_CMD_0x030C()//获取当前电台通道
        {
            byte[] outbuf1 = new byte[8];
            try
            {
                Device_CMD_0x030C(outbuf1);
            }
            catch 
            {
                
            }

            return outbuf1;
        }

        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void Device_CMD_0x030A(byte[] outbuf1, int radionum);
        public static byte[] Set_Device_CMD_0x030A(int radionum)//设置电台通道
        {
            byte[] outbuf1 = new byte[9];

            try
            {
                Device_CMD_0x030A(outbuf1, radionum);
            }
            catch { }

            return outbuf1;
        }


        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void Device_CMD_0x0903(byte[] outbuf1);
        public static byte[] Get_Device_CMD_0x0903()//获取当前数据传输模式
        {
            byte[] outbuf1 = new byte[8];

            try
            {
                Device_CMD_0x0903(outbuf1);
            }
            catch { }

            return outbuf1;
        }

        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void Device_CMD_0x0905(byte[] outbuf1, int trantype);
        public static byte[] Set_Device_CMD_0x0905(int trantype)//设置当前数据传输模式
        {
            byte[] outbuf1 = new byte[9];
            try
            {
                Device_CMD_0x0905(outbuf1, trantype);
            }
            catch { }

            return outbuf1;
        }


        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void Device_CMD_0x0601(byte[] outbuf1);
        public static byte[] Get_Device_CMD_0x0601()//获取IO极性
        {
            byte[] outbuf1 = new byte[8];

            try
            {
                Device_CMD_0x0601(outbuf1);
            }
            catch { }

            return outbuf1;
        }

        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void Device_CMD_0x0104(byte[] outbuf1);
        public static byte[] Set_Device_CMD_0x0104()//重启主机命令
        {
            byte[] outbuf1 = new byte[8];

            try
            {
                Device_CMD_0x0104(outbuf1);
            }
            catch { }

            return outbuf1;
        }


        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void Device_CMD_0x0300(byte[] outbuf1, int pAppFileCount, int pAppFileLen);
        public static byte[] Set_Device_CMD_0x0300( int pAppFileCount, int pAppFileLen)//开始升级固件，写入固件长度以及块数
        {
            byte[] b_count = Helper.intToBytes(pAppFileCount);
            byte[] b_filelen = Helper.intToBytes(pAppFileLen);

            byte[] outbuf2 = new byte[16];
            outbuf2[0] = 0xee;
            outbuf2[1] = 0xef;
            outbuf2[2] = (byte)(0x0300 & 0xff);
            outbuf2[3] = (byte)((0x0300 & 0xff00) >> 8);
            outbuf2[4] = 0;
            outbuf2[5] = 0;
            outbuf2[6] = 8;
            outbuf2[7] = 0;


            Array.Copy(b_filelen, 0, outbuf2, 8, 4);
            Array.Copy(b_count, 0, outbuf2, 12, 4);

            uint crc = 0;
            for (int i = 0; i < 8; i++)
            {
                crc = crc + outbuf2[8 + i];
            }
            outbuf2[4] = (byte)(crc & 0xff);
            outbuf2[5] = (byte)((crc & 0xff00) >> 8);

            for (int i = 0; i < 16; i++)
            {
                outbuf2[i] = (byte)Helper.tx_invork(outbuf2[i]);
            }
            return outbuf2;
            //byte[] outbuf1 = new byte[16];

            //try
            //{
            //    Device_CMD_0x0300(outbuf1, pAppFileCount, pAppFileLen);
            //}
            //catch { }

            //return outbuf1;
        }


        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void Device_CMD_0x0301(byte[] outbuf1, byte[] pAppFileData, int pAppFileCount, int pAppFileLen, int pAppFileCount_Now);
        public static byte[] Get_Device_CMD_0x0301(byte[] pAppFileData, int pAppFileCount, int pAppFileLen, int pAppFileCount_Now)//升级主机固件
        {
            byte[] outbuf2 = new byte[16 + 1024 + 1];
            byte[] outbuf3 = new byte[16 + 1024 + 1];
            outbuf2[0] = 0xee;
            outbuf2[1] = 0xef;
            outbuf2[2] = (byte)(0x0301 & 0xff);
            outbuf2[3] = (byte)((0x0301 & 0xff00) >> 8);
            outbuf2[4] = 0;
            outbuf2[5] = 0;
            outbuf2[6] = 0x04;
            outbuf2[7] = 0x08;




            int i_filelen = 0;
            if (pAppFileCount_Now < pAppFileCount)
            {
                i_filelen = 1024;
            }
            else
            {
                i_filelen = pAppFileLen - (pAppFileCount_Now - 1) * 1024;
            }


            byte[] b_count = Helper.intToBytes(pAppFileCount_Now);
            byte[] b_filelen = Helper.intToBytes(i_filelen);
            byte[] b_bodylen = Helper.int16ToBytes(i_filelen + 8);

            Array.Copy(b_bodylen, 0, outbuf2, 6, 2);
            Array.Copy(b_count, 0, outbuf2, 8, 4);
            Array.Copy(b_filelen, 0, outbuf2, 12, 4);

            Array.Copy(pAppFileData, (pAppFileCount_Now - 1) * 1024, outbuf2, 16, i_filelen);

            uint crc = 0;
            for (int i = 0; i < i_filelen + 8; i++)
            {
                crc = crc + outbuf2[8 + i];
            }
            outbuf2[4] = (byte)(crc & 0xff);
            outbuf2[5] = (byte)((crc & 0xff00) >> 8);

            for (int i = 0; i < 1040; i++)
            {
                outbuf3[i] = (byte)Helper.tx_invork(outbuf2[i]);
            }
            return outbuf3;
            //byte[] outbuf1 = new byte[8];
            //byte[] outbuf1 = new byte[16 + 1024];

            //try
            //{
            //    Device_CMD_0x0301(outbuf1, pAppFileData, pAppFileCount, pAppFileLen, pAppFileCount_Now);
            //}
            //catch { }

            //return outbuf1;
        }


        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void Device_CMD_0x0302(byte[] outbuf1);
        public static byte[] Set_Device_CMD_0x0302()//完成升级
        {
            byte[] outbuf1 = new byte[8];

            try
            {
                Device_CMD_0x0302(outbuf1);
            }
            catch { }

            return outbuf1;
        }



        //////////////////////////////////////////////////////////////////
    }
}
