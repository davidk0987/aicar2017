using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSMain
{
    class Program
    {
        static void Main(string[] args)
        {
            Buffer _buffer = new Buffer();

            byte[] data = System.Text.ASCIIEncoding.Default.GetBytes("$KSXT,20190106065841.80,113.49058882,23.44375600,22.5517,0.00,0.00,80.92,0.036,,3,0,0,17,-0.986,1470.338,-14.213,0.036,0.006,0.072,00000000000000000000000000000,00000000000000000000000000000,11111000011101111100000000001,0,0,0,LOCK/OFF,0.000,Z02B11836100001932D,KSXT_V1.6_201901011651,20190210,C002,113.49059847,23.43047988,36.59481000,FD979E6B71437CBF73C5FE0C69DDA94E,2.463,0.904,0.014,0.024,1,0.000,0.000,0.000,R01B11900600000042A,20210105,0001*36");
            byte[] data_0101 = new byte[1024];
            byte[] data_0806 = new byte[1024];
            int len_0101 = 0, len_0806 = 0;
            _buffer.ConvSignal_Int(data, data.Length, data_0101,ref len_0101, data_0806,ref len_0806);


            string str = string.Format("0101:{0}   0806:{1}", len_0101, len_0806);
            Console.WriteLine(str);
        }
    }
}
