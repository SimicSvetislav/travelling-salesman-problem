using System;

namespace TSP
{
    class Path : MyList
    {
        private double distanceTravelled;
        public double DistanceTravelled
        {
            get
            {
                return distanceTravelled;
            }
        }
        public Path ()
        {
            distanceTravelled = 0;
        }
        public double CalculatePathDistance()
        {
            distanceTravelled = 0;

            for (int i = 0; i < List.Count; ++i)
            {
                if (i != List.Count - 1)
                {
                    distanceTravelled += CalculateDistance(this[i], this[i + 1]);
                }
                else
                {
                    distanceTravelled += CalculateDistance(this[i], this[0]);
                }
            }
            return distanceTravelled;
        }
        private double CalculateDistance(City start, City finish)
        {
            return Math.Sqrt(Math.Pow(start.CoorX - finish.CoorX, 2) + Math.Pow(start.CoorY - finish.CoorY, 2));
        }

        public override string ToString()
        {
            string retVal = "Order of visits :\n";

            retVal += base.ToString();

            retVal += "\nPath : \n";

            for (int i = 0; i < List.Count; ++i)
            {
                if (i < List.Count - 1)
                {
                    retVal += $"({List[i].Id}) --> ";
                }
                else
                {
                    retVal += $"({List[i].Id})\n";
                }
            }

            retVal += $"Distance travelled : {DistanceTravelled:F3}\n";

            return retVal;
        }

        public void DisplayPath ()
        {
            Console.WriteLine("Path : ");

            for ( int i = 0; i < List.Count; ++i)
            {
                if (i == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"({List[i].Id})");
                    Console.ResetColor();
                    Console.Write(" --> ");
                }
                else if (i == List.Count-1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"({List[i].Id})");
                    Console.ResetColor();
                    Console.WriteLine();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"({List[i].Id})");
                    Console.ResetColor();
                    Console.Write(" --> ");
                }

                if ((i + 1) % 21==0)
                {
                    Console.WriteLine();
                    Console.Write("--> ");
                }
            }

            Console.WriteLine("Total distance travelled : {0:F3}", this.distanceTravelled);

        }
    }
}
