using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP
{
    class MyList
    {
        public List<City> List { get; set; }

        public MyList ()
        {
            List = new List<City>();
        }

        public City this[int index]
        {
            get
            {
                return List[index];
            }

            set
            {
                List[index] = value;
            }
        }

        public void Add (City c)
        {
            List.Add(c);
        }

        public bool Exists(int x, int y)
        {
            bool retVal = false;

            foreach (City c in this.List)
            {
                if (retVal = (c.CoorX==x) && (c.CoorY==y))
                {
                    break;
                }
            }

            return retVal;
        }
        public bool Exists(City city)
        {
            bool retVal = false;

            foreach (City c in this.List)
            {
                if (c.Id == city.Id)
                {
                    retVal = true;
                    break;
                }
            }

            return retVal;
        }
        public int NumOfCities()
        {
            return List.Count;
        }
        public override String ToString()
        {
            string retVal = "";

            if (List.Count() > 0)
            {
                foreach (var i in List)
                {
                    retVal += i.ToString();
                }
            }
            else
            {
                retVal += "**********************\nEmpty list!\n**********************\n";
            }

            return retVal;
        }
    }
}
