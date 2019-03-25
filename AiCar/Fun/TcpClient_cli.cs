using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace AiCar
{
    public class TcpClient_cli
    {


        #region 引用库

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void TcpClientDllcallBack(int nBufLen, IntPtr _lpInfo);


        [DllImport("TcpClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr CreatePtr();

        [DllImport("TcpClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void DeletePtr(IntPtr _point);

        //设置回调地址
        //extern "C" void __declspec(dllexport)SetCallBack(void *_point,DWORD ,DWORD );
        [DllImport("TcpClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void SetCallBack(IntPtr _point, TcpClientDllcallBack funptr, IntPtr classptr);

        //基于CSocket类的客户端连接
        //extern "C" BOOL __declspec(dllexport)OnCSocketConnect(void *_point,LPSTR ,UINT );
        [DllImport("TcpClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int OnCSocketConnect(IntPtr _point, string sIP, int iPort);


        //基于CSocket类的关闭客户端连接
        //extern "C" void __declspec(dllexport)UnInit(void *_point);
        [DllImport("TcpClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static void UnInit(IntPtr _point);

        //基于CSocket类的发送数据
        //extern "C" BOOL __declspec(dllexport)OnCSocketSendData(void *_point,char *,int );
        [DllImport("TcpClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static bool OnCSocketSendData(IntPtr _point,byte[] data,int len);

        //判断是否已经开启
        //extern "C" BOOL __declspec(dllexport)GetOnStart(void *_point);
        [DllImport("TcpClient.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static bool GetOnStart(IntPtr _point);
        #endregion



        private IntPtr _iPtrPoint = IntPtr.Zero;
        private string _sdomain = "";
        public TcpClient_cli()
        {
            _iPtrPoint = CreatePtr();
        }

        ~TcpClient_cli()
        {
            if(_iPtrPoint != IntPtr.Zero)
            {
                try
                {
                    UnInit(_iPtrPoint);
                    DeletePtr(_iPtrPoint);
                }
                catch { }
                
            }            
        }

        public void TcpClient_SetCallBack(TcpClientDllcallBack funptr, IntPtr classptr)
        {
            if (_iPtrPoint != IntPtr.Zero)
            {
                 SetCallBack(_iPtrPoint,funptr,classptr);
            }
        }


        public string TcpClient_GetDomain()
        {
            return _sdomain;
        }



        public int TcpClient_Connect(string sIP, int iPort)
        {
            if (_iPtrPoint != IntPtr.Zero)
            {
                _sdomain = sIP;
                IPAddress ipadd;
                if(System.Net.IPAddress.TryParse(sIP,out ipadd)==false)
                {
                    //输入的是域名
                    try
                    {
                        IPHostEntry IPinfo = Dns.GetHostEntry(sIP);
                        if (IPinfo.AddressList.Count() > 0)
                        {
                            sIP = IPinfo.AddressList[0].ToString();
                        }
                        else
                        {
                            return -101;
                        }
                    }
                    catch { return -102; }

                    
                }

                return OnCSocketConnect(_iPtrPoint, sIP, iPort);
            }

            return -100;
        }

        public void TcpClient_DisConnect()
        {
            if (_iPtrPoint != IntPtr.Zero)
            {
                UnInit(_iPtrPoint);
            }
        }

        public bool TcpClient_OnCSocketSendData(byte[] data,int len)
        {
            if (_iPtrPoint != IntPtr.Zero && data!=null && len > 0 && TcpClient_IsConnect())
            {
                return OnCSocketSendData(_iPtrPoint, data, len);
            }

            return false;
        }

        public bool TcpClient_OnCSocketSendData(byte[] data)
        {
            if (_iPtrPoint != IntPtr.Zero && data != null && data.Length > 0 && TcpClient_IsConnect())
            {
                return OnCSocketSendData(_iPtrPoint, data, data.Length);
            }

            return false;
        }

        public bool TcpClient_IsConnect()
        {
            if (_iPtrPoint != IntPtr.Zero)
            {
                return GetOnStart(_iPtrPoint);
            }

            return false;
        }



        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
