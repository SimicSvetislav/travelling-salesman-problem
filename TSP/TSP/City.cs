using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP
{
    public class City
    {
        public int CoorX { get; set; }
        public int CoorY { get; set; }
        public List<City> Connections { get; set; }
        public City ExtraConnection { get; set; }
        public int NodDegree { get; set; }
        //public List<City> MinCostMatching {get; set;}

        private readonly int id;

        public int Id
        {
            get
            {
                return id;
            }
        }

        private static int idAssing = 1;

        public City (int x, int y)
        {
            CoorX = x;
            CoorY = y;
            id = idAssing++;
            NodDegree = 0;
            Connections = new List<City>();
            ExtraConnection = null;
            //MinCostMatching = new List<City>();
        }

        public City (City c)
        {
            this.CoorX = c.CoorX;
            this.CoorY = c.CoorY;
            this.id = c.Id;
            NodDegree = c.NodDegree;
            Connections = new List<City>(c.Connections);
            ExtraConnection = c.ExtraConnection;
            //MinCostMatching = new List<City>();
        }

        public City()
        {
        }

        public override bool Equals(object obj)
        {
            bool retVal = false;

            City city = (City)obj;
            if ((this.CoorX == city.CoorX) && (this.CoorY == city.CoorY))
            {
                retVal = true;
            }

            return retVal;
        }

        public override string ToString()
        {
            return $"City ({Id})\t[{CoorX}]\t[{CoorY}]\n";
        }
        
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool AlreadyConnected (City c)
        {
            bool retVal = false;
            foreach (City i in Connections)
            {
                if (c.id == i.Id)
                {
                    retVal = true;
                    break;
                }
            }

            return retVal;
        }
    }
}
