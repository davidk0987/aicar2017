using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KSMain
{
    public class Buffer
    {
        [DllImport("Buffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr CreatePtr();

        [DllImport("Buffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void DeletePtr(IntPtr _point);
        [DllImport("Buffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int ConvSignal(IntPtr _point, byte[] data, int len, byte[] data_0101, ref int len_0101, byte[] data_0806, ref int len_0806);

        [DllImport("Buffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void ClearBuffer(IntPtr _point);

        [DllImport("Buffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int GetBufferLen(IntPtr _point);

        [DllImport("Buffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static byte GetAt(IntPtr _point, int index);

        [DllImport("Buffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static bool Append(IntPtr _point, byte[] data, int len);

        [DllImport("Buffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int Delete(IntPtr _point, int nSize);

        [DllImport("Buffer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int Read(IntPtr _point, byte[] data, int nSize);

        private IntPtr _iPtrPoint = IntPtr.Zero;
        public Buffer()
        {
            _iPtrPoint = CreatePtr();
        }

        ~Buffer()
        {
            if(_iPtrPoint!=IntPtr.Zero)
            {
                DeletePtr(_iPtrPoint);
                _iPtrPoint = IntPtr.Zero;
            }
        }

        public int ConvSignal_Int(byte[] data, int len, byte[] data_0101, ref int len_0101, byte[] data_0806, ref int len_0806)
        {
            if (_iPtrPoint != IntPtr.Zero)
            {
                return ConvSignal(_iPtrPoint, data, len,  data_0101, ref len_0101,  data_0806, ref len_0806);
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
    }
}
