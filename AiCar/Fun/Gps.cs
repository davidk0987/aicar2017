using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCar
{
    class Gps
    {
        /// <summary>
        /// 纬度
        /// </summary>
        private double wgLat;
        /// <summary>
        /// 经度
        /// </summary>
        private double wgLon;

        public Gps()
        {
            wgLat = 0;
            wgLon = 0;
        }
        public Gps(double wgLat, double wgLon)
        {
            setWgLat(wgLat);
            setWgLon(wgLon);
        }
        public double getWgLat()
        {
            return wgLat;
        }
        public void setWgLat(double wgLat)
        {
            this.wgLat = wgLat;
        }
        public double getWgLon()
        {
            return wgLon;
        }
        public void setWgLon(double wgLon)
        {
            this.wgLon = wgLon;
        }
        public String toString()
        {
            return wgLat + "," + wgLon;
        }
    }
}
