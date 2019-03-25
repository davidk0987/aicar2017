using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AiCar
{
    public class TcpServer
    {
        #region 引用库
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void TcpServerDllcallBack(int nBufLen, IntPtr _lpInfo);

        [DllImport("TcpServer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr CreatePtr();

        [DllImport("TcpServer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void DeletePtr(IntPtr _point);

        //设置回调地址
        //extern "C" void __declspec(dllexport)SetCallBack(void *_point,DWORD ,DWORD );
        [DllImport("TcpServer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetCallBack(IntPtr _point, TcpServerDllcallBack funptr, IntPtr classptr);

        //开始启动监听
        //extern "C" BOOL __declspec(dllexport)OnStart(void* _point, UINT )
        [DllImport("TcpServer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static bool OnStart(IntPtr _point, int iport);

        //停止监听
        //extern "C" BOOL __declspec(dllexport)OnUnit(void* _point);
        [DllImport("TcpServer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static bool OnUnit(IntPtr _point);

        //设置最大连接数，最大允许10000个连接
        //extern "C" void __declspec(dllexport)SetConnectCount(void* _point, UINT  )
        [DllImport("TcpServer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetConnectCount(IntPtr _point, int iconnectcount);

        //基于CSocket类的发送数据
        //extern "C" BOOL __declspec(dllexport)OnSendData(void* _point, char*, int , UINT  )
        [DllImport("TcpServer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static bool OnSendData(IntPtr _point, byte[] date, int len, int socket);

        //向所有客户端发送数据
        //extern "C" BOOL __declspec(dllexport)OnSendClientAll(void* _point, char*, int  )
        [DllImport("TcpServer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static bool OnSendClientAll(IntPtr _point, byte[] date, int len);

        //断开指定连接
        //extern "C" BOOL __declspec(dllexport)OnDeleteClient(void* _point, UINT  );

        [DllImport("TcpServer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static bool OnDeleteClient(IntPtr _point, int sokcet);

        //断开所有连接
        //extern "C" void __declspec(dllexport)OnDeleteAll(void* _point);
        [DllImport("TcpServer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void OnDeleteAll(IntPtr _point);

        //心跳计数
        //extern "C" void __declspec(dllexport)OnTime(void* _point, int _iTimeOut);
        [DllImport("TcpServer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void OnTime(IntPtr _point, int itimeout);

        //获取连接数
        //extern "C" int __declspec(dllexport)OnGetConnectCount(void* _point);
        [DllImport("TcpServer.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int OnGetConnectCount(IntPtr _point);

        #endregion


        private IntPtr _iPtrPoint = IntPtr.Zero;

        public TcpServer()
        {
            _iPtrPoint = CreatePtr();
            if (_iPtrPoint!=IntPtr.Zero) SetConnectCount(_iPtrPoint,100);
        }

        ~TcpServer()
        {
            if (_iPtrPoint != IntPtr.Zero)
            {
                try
                {
                    OnUnit(_iPtrPoint);
                    DeletePtr(_iPtrPoint);
                }
                catch { }

            }
        }

        public void TcpServer_SetCallBack(TcpServerDllcallBack funptr, IntPtr classptr)
        {
            if (_iPtrPoint != IntPtr.Zero)
            {
                SetCallBack(_iPtrPoint, funptr, classptr);
            }
        }

        public bool TcpServer_OnStart(int iport)
        {
            if (_iPtrPoint != IntPtr.Zero)
            {
                return OnStart(_iPtrPoint, iport);
            }

            return false;
        }

        public bool TcpServer_OnUnit()
        {
            if (_iPtrPoint != IntPtr.Zero)
            {
                return OnUnit(_iPtrPoint);
            }

            return false;
        }

        public void TcpServer_SetConnectCount(int iconnectcount)
        {
            if (_iPtrPoint != IntPtr.Zero)
            {
                SetConnectCount(_iPtrPoint, iconnectcount);
            }
        }

        public bool TcpServer_OnSendData(byte[] date, int socket)
        {
            if (_iPtrPoint != IntPtr.Zero)
            {
                return OnSendData(_iPtrPoint, date, date.Length, socket);
            }
            return false;
        }

        public bool TcpServer_OnSendClientAll(byte[] date,int len)
        {
            if (_iPtrPoint != IntPtr.Zero && len>0)
            {
                return OnSendClientAll(_iPtrPoint, date, len);
            }
            return false;
        }


        public bool TcpServer_OnDeleteClient(int sokcet)
        {
            if (_iPtrPoint != IntPtr.Zero)
            {
                return OnDeleteClient(_iPtrPoint, sokcet);
            }
            return false;
        }


        public void TcpServer_OnDeleteAll(IntPtr _point)
        {
            if (_iPtrPoint != IntPtr.Zero)
            {
                OnDeleteAll(_iPtrPoint);
            }
        }

        public void TcpServer_OnTime(int itimeout)
        {
            if (_iPtrPoint != IntPtr.Zero)
            {
                OnTime(_iPtrPoint, itimeout);
            }
        }

        public int TcpServer_OnGetConnectCount(IntPtr _point)
        {
            if (_iPtrPoint != IntPtr.Zero)
            {
                return OnGetConnectCount(_iPtrPoint);
            }
            return 0;
        }




        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
