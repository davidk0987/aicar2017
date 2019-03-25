using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AiCar
{
    public class ByteBuffer
    {
        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr CreatePtr();

        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void DeletePtr(IntPtr _point);
        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int ConvSignal(IntPtr _point, byte[] data, int len, byte[] data_0101, ref int len_0101, byte[] data_0806, ref int len_0806);
        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static bool ReadConfig(IntPtr _point, byte[] strcfgpath);



        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void ClearBuffer(IntPtr _point);

        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetBufferLen(IntPtr _point);

        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static byte GetAt(IntPtr _point, int index);

        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static bool Append(IntPtr _point, byte[] data, int len);

        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int Delete(IntPtr _point, int nSize);

        [DllImport("ByteBuffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int Read(IntPtr _point, byte[] data, int nSize);

        private IntPtr _iPtrPoint = IntPtr.Zero;
        public ByteBuffer()
        {
            _iPtrPoint = CreatePtr();
        }

        ~ByteBuffer()
        {
            if(_iPtrPoint!=IntPtr.Zero)
            {
                DeletePtr(_iPtrPoint);
                _iPtrPoint = IntPtr.Zero;
            }
        }

        public bool ReadConfig_bool()
        {
            if (_iPtrPoint != IntPtr.Zero)
            {
                string strcfgpath = Environment.CurrentDirectory + "\\config\\sensor.ini";
                return ReadConfig(_iPtrPoint, System.Text.ASCIIEncoding.Default.GetBytes(strcfgpath));
            }
            return false;
        }
        public int ConvSignal_Int(byte[] data, byte[] data_0101, ref int len_0101, byte[] data_0806, ref int len_0806)
        {
            if (_iPtrPoint != IntPtr.Zero)
            {
                return ConvSignal(_iPtrPoint, data, data.Length,  data_0101, ref len_0101,  data_0806, ref len_0806);
            }

            return -100;
        }

        public bool Buff_Append(byte[] data)
        {
            if (_iPtrPoint != IntPtr.Zero)
            {
                return Append(_iPtrPoint, data, data.Length);
            }
            return false;
        }

        public int Buff_GetBufferLen()
        {
            if (_iPtrPoint != IntPtr.Zero)
            {
                return GetBufferLen(_iPtrPoint);
            }
            return -1;
        }

        public byte Buff_GetAt(int index)
        {
            if (_iPtrPoint != IntPtr.Zero)
            {
                return GetAt(_iPtrPoint, index);
            }
            return 0;
        }

        public int Buff_Delete(int nSize)
        {
            if (_iPtrPoint != IntPtr.Zero)
            {
                return Delete(_iPtrPoint, nSize);
            }
            return 0;
        }

        public int Buff_Read(byte[] data, int nSize)
        {
            if (_iPtrPoint != IntPtr.Zero)
            {
                return Read(_iPtrPoint, data, nSize);
            }
            return 0;
        }

        public void Buff_ClearBuffer()
        {
            if (_iPtrPoint != IntPtr.Zero)
            {
                ClearBuffer(_iPtrPoint);
            }
        }
    }
}
