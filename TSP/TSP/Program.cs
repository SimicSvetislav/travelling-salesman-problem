using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSP
{
    class Program
    {
        static void Main(string[] args)
        {
            MyList listOfCities = new MyList();
            int numOfCities = 0;
            string input = "";

            Console.WriteLine("*******************************************");
            Console.WriteLine("*****************   TSP   *****************");
            Console.WriteLine("*******************************************");
            Console.WriteLine("\nPlease choose input method : \n1. From file\n2. Random generated cities");
            Console.Write(">> ");

            input = Console.ReadLine();

            if (!(input.All(char.IsDigit)) || (input == "") || (Convert.ToInt32(input)) > 2 || (Convert.ToInt32(input) < 1))
            {
                do
                {
                    Console.WriteLine("Invalid input!");
                    Console.Write("Please type valid input (1 or 2).\n>> ");
                    input = Console.ReadLine();
                } while (!(input.All(char.IsDigit)) || (input == "") || (Convert.ToInt32(input)) > 2 || (Convert.ToInt32(input) < 1));
            }

            int inputDig = Convert.ToInt32(input);

            if (inputDig == 2)
            {
                #region Creation of cities
                
                bool validInput = false;
                Console.Write("\nHow many cities?\n>> ");

                do
                {
                    input = Console.ReadLine();
                    if ((input.All(char.IsDigit)) && !(input == "") && (Convert.ToInt32(input) > 0))
                    {
                        validInput = true;
                        numOfCities = Convert.ToInt32(input);
                    }
                    else
                    {
                        Console.Write("Input not valid!\nTry again.\n>> ");
                    }
                } while (!validInput);

                Random rndNum = new Random();

                int cX, cY;
                int maxX = 300, maxY = 200, offsetY = 40;
           

                for (int i = 0; i < numOfCities; i++) {

                    do {
                        cX = rndNum.Next(1, maxX);
                        cY = rndNum.Next(1, maxY) + offsetY;
                        bool ima;
                        ima = listOfCities.Exists(cX, cY);
                        Console.WriteLine($"{cX} {cY} : {ima}\n");
                        //Console.WriteLine($"{cX} {cY}");
                    } while (listOfCities.Exists(cX, cY));

                    //Console.WriteLine(cX + " " + cY);
                    City c1 = new City(cX, cY);
                    listOfCities.Add(c1);
                }
                #endregion
            }
            else
            { 
                #region File input

                string line;
                char delimiter = ' ';
                City c;

                string activeFile = "";

                Console.WriteLine("\nWhich file would you like to use?\n" +
                    "1. citiesSmallV.txt (10 cities)\n" +
                    "2. spanningTreeExample.txt (10 cities)\n" +
                    "3. problemC.txt (15 cities)\n" +
                    "4. smallMediumV.txt (30 cities)\n" +
                    "5. citiesMediumV.txt (50 cities)\n" +
                    "6. citiesLargeMediumV.txt (75 cities)\n" +
                    "7. citiesLargeV.txt (100 cities)\n" +
                    "8. citiesVeryLargeV.txt (150 cities)\n" +
                    "9. citiesHugeV.txt (200 cities)"
                    );

                Console.Write(">> ");

                input = Console.ReadLine();

                if (!(input.All(char.IsDigit)) || (input == "") || (Convert.ToInt32(input)) > args.Length || (Convert.ToInt32(input) < 1))
                {
                    do
                    {
                        Console.WriteLine("Invalid input!");
                        Console.Write("Please type valid input (from 1 to {0}).\n>> ", args.Length);
                        input = Console.ReadLine();
                    } while (!(input.All(char.IsDigit)) || (input == "") || (Convert.ToInt32(input)) > args.Length || (Convert.ToInt32(input) < 1));
                }

                inputDig = Convert.ToInt32(input);
                activeFile = args[inputDig - 1];

                using (StreamReader reader = new StreamReader(activeFile))
                {

                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(delimiter);
                        c = new City(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]));
                        listOfCities.Add(c);
                    }

                    reader.Close();
                    Console.WriteLine("\nCities loaded from file : {0}", activeFile);
                }
            }

            #endregion

            //Console.WriteLine($"Number of cities {listOfCities.NumOfCities()}");

            List<City> greedyOutputList = new List<City>();
            List<City> twoOutputList = new List<City>();
            List<City> BBOutputList = new List<City>();
            List<City> christofidesOutputList = new List<City>();

            double greedyDistance, twoDistance, BBDistance;
            TimeSpan greedyTimeSpan, twoTimeSpan, BBTimeSpan;

            #region Distance matrix

            double[,] distanceMatrix = new double[listOfCities.NumOfCities(), listOfCities.NumOfCities()];

            for (int i = 0; i < listOfCities.NumOfCities(); ++i)
            { 
                for (int j = 0; j < listOfCities.NumOfCities(); ++j) {
                    if (j != i)
                    {
                        distanceMatrix[i, j] = CalculateDistance(listOfCities[i], listOfCities[j]);
                    }
                    else
                    {
                        distanceMatrix[i, j] = double.PositiveInfinity;
                    }
                }
            }

            #endregion

            #region Choosing start city
            int startCityId;

            Console.Write("\nID of starting city?\n>> ");
            input = Console.ReadLine();

            if (!(input.All(char.IsDigit)) || (input == "") || (Convert.ToInt32(input) < 1) || (Convert.ToInt32(input) > listOfCities.NumOfCities()))
            {
                do
                {
                    Console.WriteLine("Invalid input!");
                    Console.WriteLine($"Valid city IDs range from 1 to {listOfCities.NumOfCities()}.");
                    Console.Write("Please type valid ID.\n>> ");
                    input = Console.ReadLine();
                } while (!(input.All(char.IsDigit)) || (input == "") || (Convert.ToInt32(input) < 1) || (Convert.ToInt32(input) > listOfCities.NumOfCities()));
            }

            startCityId = Convert.ToInt32(input);
            City startCity = listOfCities[startCityId - 1];

            //Console.WriteLine($"Starting city : {startCity}");

            #endregion

            MyList forC = new MyList();
            for (int i = 0; i < listOfCities.List.Count; ++i)
            {
                forC.Add(listOfCities.List[i]);
            }

            Console.WriteLine("\nChoose method :\n1. Greedy\n2. Greedy + 2-opt\n3. Branch and bound\n4. Christofides\n5. All");
            Console.Write(">> ");
            input = Console.ReadLine();

            if (!(input.All(char.IsDigit)) || (input == "") || (Convert.ToInt32(input) < 1) || (Convert.ToInt32(input) > 5))
            {
                do
                {
                    Console.WriteLine("Invalid input!");
                    Console.WriteLine("Valid inputs range from 1 to 5.");
                    Console.Write("Please type valid input.\n>> ");
                    input = Console.ReadLine();
                } while (!(input.All(char.IsDigit)) || (input == "") || (Convert.ToInt32(input) < 1) || (Convert.ToInt32(input) > 5));
            }

            inputDig = Convert.ToInt32(input);

            MyList notVisited = new MyList();
            Path path = new Path();

            double christofidesDistance;
            TimeSpan christofidesTimeSpan;

            notVisited.List = listOfCities.List;

            switch (inputDig)
            {
                case 1:
                    GreedyAlgoithm(startCity, listOfCities, path, notVisited, out greedyOutputList, out greedyDistance, out greedyTimeSpan);

                    Console.Write("\nTap enter for graph(s) . . . ");
                    Console.ReadLine();

                    Task mytask1 = Task.Run(() =>
                    {
                        MyForm form = new MyForm(greedyOutputList, "Greedy algorithm", greedyDistance, DisplayTimeSpan(greedyTimeSpan));
                        form.ShowDialog();
                    });

                    break;

                case 2:
                    Path temp = new Path();

                    temp = path;
                    GreedyAlgoithm(startCity, listOfCities, path, notVisited, out greedyOutputList, out greedyDistance, out greedyTimeSpan);
                    TwoOptAlgorithm(temp, out twoOutputList, out twoDistance, out twoTimeSpan);

                    Console.Write("\nTap enter for graph(s) . . . ");
                    Console.ReadLine();

                    mytask1 = Task.Run(() =>
                    {
                        MyForm form = new MyForm(greedyOutputList, "Greedy algorithm", greedyDistance, DisplayTimeSpan(greedyTimeSpan));
                        form.ShowDialog();
                    });

                    Task mytask2 = Task.Run(() =>
                    {
                        MyForm form = new MyForm(twoOutputList, "2-opt algorithm", twoDistance, DisplayTimeSpan(twoTimeSpan));
                        form.ShowDialog();
                    });

                    break;

                case 3:
                    BranchAndBound(distanceMatrix, startCityId, listOfCities, out BBOutputList, out BBDistance, out BBTimeSpan);

                    Console.Write("\nTap enter for graph(s) . . . ");
                    Console.ReadLine();

                    Task mytask3 = Task.Run(() =>
                    {
                        MyForm form = new MyForm(BBOutputList, "Branch and Bound algorithm", BBDistance, DisplayTimeSpan(BBTimeSpan));
                        form.ShowDialog();
                    });

                    break;

                case 4:
                    christofidesOutputList = Christofides(startCity, distanceMatrix, forC, out christofidesDistance, out christofidesTimeSpan);

                    Console.Write("\nTap enter for graph(s) . . . ");
                    Console.ReadLine();

                    Task mytask4 = Task.Run(() =>
                    {
                        MyForm form = new MyForm(christofidesOutputList, "Christofides algorithm", christofidesDistance, DisplayTimeSpan(christofidesTimeSpan));
                        form.ShowDialog();
                    });

                    break;

                case 5:
                    City g = new City();
                    g = startCity;
                    var d = distanceMatrix;
                    MyList pl = new MyList();
                    foreach (var i in listOfCities.List)
                    {
                        pl.Add(i);
                    }
                    GreedyAlgoithm(startCity, listOfCities, path, notVisited, out greedyOutputList, out greedyDistance, out greedyTimeSpan);
                    TwoOptAlgorithm(path, out twoOutputList, out twoDistance, out twoTimeSpan);
                    BranchAndBound(distanceMatrix, startCityId, pl, out BBOutputList, out BBDistance, out BBTimeSpan);
                    christofidesOutputList = Christofides(g, d, forC, out christofidesDistance, out christofidesTimeSpan);

                    Console.Write("\nTap enter for graph(s) . . . ");
                    Console.ReadLine();

                    mytask1 = Task.Run(() =>
                    {
                        MyForm form = new MyForm(greedyOutputList, "Greedy algorithm", greedyDistance, DisplayTimeSpan(greedyTimeSpan));
                        form.ShowDialog();
                    });

                    mytask2 = Task.Run(() =>
                    {
                        MyForm form = new MyForm(twoOutputList, "2-opt algorithm", twoDistance, DisplayTimeSpan(twoTimeSpan));
                        form.ShowDialog();
                    });

                    mytask3 = Task.Run(() =>
                    {
                        MyForm form = new MyForm(BBOutputList, "Branch and Bound algorithm", BBDistance, DisplayTimeSpan(BBTimeSpan));
                        form.ShowDialog();
                    });

                    mytask4 = Task.Run(() =>
                    {
                        MyForm form = new MyForm(christofidesOutputList, "Christofides algorithm", christofidesDistance, DisplayTimeSpan(christofidesTimeSpan));
                        form.ShowDialog();
                    });

                    break;
            }

            Console.Write("\nTap enter to end program execution . . . ");
            Console.ReadLine();
        }

        private static void TwoOptAlgorithm (Path path, out List<City> twoOutputList, out double twoDistance, out TimeSpan twoTimeSpan)
        {
            Console.Write("\n2-opt algorithm at work . . . ");
            Console.WriteLine();

            DateTime finish, begin = DateTime.Now;

            int noImproveIteration = 0;

            while (noImproveIteration < 20)
            {
                double shortestPath = path.DistanceTravelled;

                for (int i = 1; i < path.NumOfCities() - 2; ++i)
                {
                    for (int j = i + 1; j < path.NumOfCities() - 1; ++j)
                    {
                        TwoOptSwap(ref i, ref j, out Path newPath, path);

                        if (newPath.DistanceTravelled < shortestPath)
                        {
                            noImproveIteration = 0;
                            path = newPath;
                        }
                    }
                }
                noImproveIteration++;
            }

            finish = DateTime.Now;

            twoTimeSpan = finish - begin;

            twoDistance = path.CalculatePathDistance();
            path.DisplayPath();
            Console.WriteLine(DisplayTimeSpan(twoTimeSpan));

            twoOutputList = path.List;
        }

        private static void GreedyAlgoithm(City startCity, MyList listOfCities, Path path, MyList notVisited, out List<City> greedyOutputList, out double greedyDistance, out TimeSpan greedyTimeSpan)
        {
            DateTime begin, finish;

            begin = DateTime.Now;

            Console.Write("\nGreedy algorithm at work . . . ");
            Console.WriteLine();

            City currentCity = startCity;

            path.Add(startCity);
            listOfCities.List.Remove(startCity);

            while (notVisited.NumOfCities() != 0)
            {
                currentCity = FindClosestCity(currentCity, notVisited);
                path.Add(currentCity);
                notVisited.List.Remove(currentCity);
            }

            path.Add(startCity);

            finish = DateTime.Now;
            greedyTimeSpan = finish - begin;

            greedyDistance = path.CalculatePathDistance();

            greedyOutputList = path.List;
            path.DisplayPath();
            Console.WriteLine(DisplayTimeSpan(greedyTimeSpan));
        }
        private static List<City> Christofides(City startCity, double[,] distanceMatrix, MyList list, out double distance, out TimeSpan timeSpan)
        {
            MyList tree = new MyList();
            MyList oddDegree = new MyList();
            tree.Add(startCity);

            City c1 = new City();
            City c2 = new City();

            //Console.WriteLine(list.ToString());

            list.List.Remove(startCity);

            double minDistance;
            c1 = list.List[0];
            c2 = list.List[0];
            double temp = 0;

            DateTime start, finish;

            Console.Write("\nChristofides algorithm at work . . . ");
            Console.WriteLine();

            start = DateTime.Now;

            //Console.WriteLine("Out of tree :");
            while (list.NumOfCities() != 0)
            {
                minDistance = CalculateDistance(list.List[0], tree.List[0]);
                c1 = list.List[0];
                c2 = tree.List[0];
                foreach (City cout in list.List)
                {
                    foreach (City cin in tree.List)
                    {
                        if (minDistance >= (temp = CalculateDistance(cout, cin)))
                        {
                            minDistance = temp;
                            c1 = cout;
                            c2 = cin;
                        }
                    }
                }
                list.List.Remove(c1);
                tree.List.Add(c1);
                c1.Connections.Add(c2);
                c2.Connections.Add(c1);
                c1.NodDegree++;
                c2.NodDegree++;
            }
            //PrintConnections(tree.List);

            foreach (City c in tree.List)
            {
                if (c.NodDegree % 2 != 0)
                {
                    oddDegree.Add(c);
                }
            }

            //Console.WriteLine("Odd degree nods : ");
            //Console.WriteLine(oddDegree);

            while (oddDegree.List.Count != 0)
            {
                c1 = oddDegree.List[0];
                tree.List.Remove(c1);
                oddDegree.List.Remove(c1);

                c2 = FindClosestCity(c1, oddDegree);
                tree.List.Remove(c2);
                oddDegree.List.Remove(c2);

                c1.ExtraConnection = c2;
                c2.ExtraConnection = c1;

                c1.NodDegree++;
                c2.NodDegree++;

                tree.List.Add(c1);
                tree.List.Add(c2);
            }

            //PrintConnections(tree.List);
            //Console.WriteLine("Done");

            list.List = tree.List;
            MyList backup = new MyList();
            for (int i = 0; i < tree.List.Count; ++i)
            {
                backup.Add(tree.List[i]);
            }
            City currentCity = startCity;
            City nextCity;
            City preferableCity=null;
            Path pathWithDuplicates = new Path();
            pathWithDuplicates.Add(startCity);
            list.List.Remove(startCity);

            while (list.List.Count != 0)
            {
                nextCity = null;
                if (currentCity.ExtraConnection != null)
                {
                    preferableCity = new City(currentCity.ExtraConnection);
                    if (list.Exists(preferableCity))
                    {
                        for (int i = 0; i < list.List.Count; ++i)
                        {
                            if (preferableCity.Id == list.List[i].Id)
                            {
                                nextCity = new City(list.List[i]);
                                list.List.Remove(nextCity);
                                nextCity.ExtraConnection = null;
                                list.List.Add(nextCity);
                                list.List.Remove(currentCity);
                                currentCity.ExtraConnection = null;
                                list.List.Add(currentCity);
                                break;
                            }
                        }
                    }
                }

                if (nextCity == null)
                {
                    MyList t = new MyList();
                    City min = new City();
                    t.List = currentCity.Connections;
                    if (t.List.Count != 0)
                    {
                        do
                        {
                            min = FindClosestCity(currentCity, t);
                            if (list.Exists(min))
                            {
                                for (int i = 0; i < list.List.Count; ++i)
                                {
                                    if (min.Id == list.List[i].Id)
                                    {
                                        nextCity = new City(list.List[i]);
                                        list.List.Remove(nextCity);
                                        nextCity.Connections.Remove(currentCity);
                                        list.List.Add(nextCity);
                                        list.List.Remove(currentCity);
                                        currentCity.Connections.Remove(nextCity);
                                        list.List.Add(currentCity);
                                        break;
                                    }
                                }
                                break;
                            }
                            t.List.Remove(min);
                            if (t.List.Count == 0)
                            {
                                break;
                            }
                        } while (t.List.Count != 0);
                    }
                }

                if (nextCity == null)
                {
                    for (int i = 0; i < backup.List.Count; ++i)
                    {
                        if (preferableCity.Id == backup[i].Id)
                        {
                            nextCity = new City(preferableCity);
                            list.List.Remove(nextCity);
                            nextCity.Connections.Remove(currentCity);
                            list.List.Add(nextCity);
                            list.List.Remove(currentCity);
                            currentCity.Connections.Remove(nextCity);
                            list.List.Add(currentCity);
                        }
                    }
                }
                
                if (nextCity == null)
                {
                    double m = double.PositiveInfinity;
                    City d = new City();
                    foreach (City c in list.List)
                    {
                        if (CalculateDistance(currentCity, c) < m)
                        {
                            m = CalculateDistance(currentCity, c);
                            d = c;
                        }
                    }
                    if (list.List.Contains(currentCity))
                    {
                        list.List.Remove(currentCity);
                    }
                    nextCity = new City (d);
                }

                if (list.List.Count == 1)
                {
                    nextCity = list.List[0];
                    pathWithDuplicates.Add(nextCity);
                    list.List.Clear();
                }

                //list.List.Remove(nextCity);

                bool notDone = true;

                do
                {
                    notDone = false;
                    for (int i = 0; i < list.List.Count; ++i)
                    {
                        if ((list.List[i].Connections.Count == 0) && (list.List[i].ExtraConnection == null))
                        {
                            notDone = true;
                            list.List.Remove(list.List[i]);
                        }
                    }
                } while (notDone);

                pathWithDuplicates.Add(nextCity);

                if (!(list.List.Contains(nextCity)) && (list.List.Count != 0))
                {
                    nextCity = FindClosestCity(currentCity, list);
                    pathWithDuplicates.Add(nextCity);
                }

                if (list.List.Contains(nextCity))
                {
                    list.List.Remove(nextCity);
                }

                currentCity = nextCity;

                //Console.WriteLine(pathWithDuplicates);
                //Console.WriteLine(list);
                //PrintConnections(list.List);
            }

            Path path = new Path();
            
            for (int i = 0; i < pathWithDuplicates.List.Count; ++i)
            {
                if (!(path.List.Contains(pathWithDuplicates.List[i])))
                {
                    path.Add(pathWithDuplicates.List[i]);
                }
            }

            path.Add(startCity);

            finish = DateTime.Now;

            timeSpan = finish - start;

            distance = path.CalculatePathDistance();

            path.DisplayPath();
            Console.WriteLine(DisplayTimeSpan(timeSpan));

            return path.List;
        }
        private static void PrintConnections(List<City> list)
        {
            foreach (City c in list)
            {
                Console.Write($"City {c.Id} ({c.CoorX}, {c.CoorY});\tConnections : ");
                foreach (City i in c.Connections)
                {
                    Console.Write($"({i.Id}) ");
                }
                Console.Write($"\t\tNod degree : {c.NodDegree}\t");
                if (c.ExtraConnection != null)
                {
                    Console.Write($"Extra connection : {c.ExtraConnection.Id}");
                }
                Console.WriteLine(); 
            }
        }
        private static void BranchAndBound(double[,] distanceMatrix, int startCityId, MyList listOfCities, out List<City> list, out double distance, out TimeSpan timeSpan)
        {
            //simple matrices for check
            /*
            // cost - 25
            distanceMatrix = new double[,] {    { double.PositiveInfinity, 4, 12, 7 },
                                                { 5, double.PositiveInfinity, double.PositiveInfinity, 18 },
                                                { 11, double.PositiveInfinity, double.PositiveInfinity, 6 },
                                                { 10, 2, 3, double.PositiveInfinity }
            };
            */

            /*
            // cost - 34
            distanceMatrix = new double[,] {    { double.PositiveInfinity, 10, 8, 9, 7 },
                                                { 10, double.PositiveInfinity, 10, 5, 6 },
                                                { 8, 10, double.PositiveInfinity, 8, 9 },
                                                { 9, 5, 8, double.PositiveInfinity, 6 },
                                                { 7, 6, 9, 6, double.PositiveInfinity }
            };
            */

            /*
            // cost - 16
            distanceMatrix = new double[,] {    { double.PositiveInfinity, 3, 1, 5, 8 },
                                                { 3, double.PositiveInfinity, 6, 7, 9 },
                                                { 1, 6, double.PositiveInfinity, 4, 2 },
                                                { 5, 7, 4, double.PositiveInfinity, 3 },
                                                { 8, 9, 2, 3, double.PositiveInfinity }
            };
            */

            /*
            // cost - 8
            distanceMatrix = new double[,] {    { double.PositiveInfinity, 2, 1, double.PositiveInfinity },
                                                { 2, double.PositiveInfinity, 4, 3 },
                                                { 1, 4, double.PositiveInfinity, 2 },
                                                { double.PositiveInfinity, 3, 2, double.PositiveInfinity }
            };
            */

            /*
            // cost - 12
            distanceMatrix = new double[,] {    { double.PositiveInfinity, 5, 4, 3 },
                                                { 3, double.PositiveInfinity, 8, 2 },
                                                { 5, 3, double.PositiveInfinity, 9 },
                                                { 6, 4, 3, double.PositiveInfinity }
            };
            */
            /*
            // optimal cost - 100 
            distanceMatrix = new double[,] {    { double.PositiveInfinity, 11, 24, 25, 30, 29, 15, 15 },
                                                { 11, double.PositiveInfinity, 13, 20, 32, 37, 17, 17 },
                                                { 24, 13, double.PositiveInfinity, 16, 30, 39, 29, 22 },
                                                { 25, 20, 16, double.PositiveInfinity, 15, 23, 18, 12 },
                                                { 30, 32, 30, 15, double.PositiveInfinity, 9, 23, 15 },
                                                { 29, 37, 39, 23, 9, double.PositiveInfinity, 14, 21 },
                                                { 15, 17, 29, 18, 23, 14, double.PositiveInfinity, 7 },
                                                { 15, 17, 22, 12, 15, 21, 7, double.PositiveInfinity }
            };
            */
            if (distanceMatrix.GetLength(0) < 20)
            {
                DisplayMatrix(distanceMatrix);
            }

            double[,] reducedMatrix = new double[distanceMatrix.GetLength(0), distanceMatrix.GetLength(1)];
            for (int i = 0; i < reducedMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < reducedMatrix.GetLength(0); j++)
                {
                    reducedMatrix[i, j] = distanceMatrix[i, j];
                }
            }

            List<int> notVisitedId = new List<int>();

            for (int i = 1; i <= distanceMatrix.GetLength(0); ++i)
            {
                notVisitedId.Add(i);
            }

            double[] rowMin = new double[distanceMatrix.GetLength(0)];
            double[] columnMin = new double[distanceMatrix.GetLength(1)];
            double cost = 0;

            Console.Write("\nBranch and Bound algorithm at work . . . ");
            Console.WriteLine();

            DateTime start, finish;

            start = DateTime.Now;

            notVisitedId.Remove(startCityId);

            //Initial reduction

            int id1 = startCityId, id2 = id1;

            for (int i = 0; i < rowMin.Length; ++i)
            {
                rowMin[i] = FindRowMin(reducedMatrix, i);
            }

            RowReduction(rowMin, ref reducedMatrix);

            for (int i = 0; i < columnMin.Length; ++i)
            {
                columnMin[i] = FindColumnMin(reducedMatrix, i);
            }

            ColumnReduction(ref reducedMatrix, columnMin);

            CalculateCost(ref cost, Sum(rowMin), Sum(columnMin), distanceMatrix, id1 - 1, id2 - 1);

            //Initialy reduced matrix formed with initial cost

            Queue<int> path = new Queue<int>();
            int currentCityId = startCityId;

            path.Enqueue(startCityId);

            while (notVisitedId.Count != 0) {

                Dictionary<int, double> nodCost = new Dictionary<int, double>();
                Dictionary<int, double[,]> potentialNewMatrices = new Dictionary<int, double[,]>();

                foreach (int id in notVisitedId)
                {
                    nodCost.Add(id, DoWork(currentCityId, id, reducedMatrix, cost, ref potentialNewMatrices, startCityId));
                }

                //Parallel.ForEach(notVisitedId, id => nodCost.Add(id, DoWork(currentCityId, id, reducedMatrix, cost, ref potentialNewMatrices, startCityId)));

                cost = nodCost.Values.Min();
                currentCityId = FindKey(cost, nodCost);
                reducedMatrix = FindMatrix(currentCityId, potentialNewMatrices);
                path.Enqueue(currentCityId);
                notVisitedId.Remove(currentCityId);
            }

            //connect to first
            path.Enqueue(startCityId);

            finish = DateTime.Now;
            timeSpan = finish - start;

            Path pathR = new Path();
            list = new List<City>();

            foreach (int i in path)
            {
                for (int j = 0; j < listOfCities.List.Count; ++j)
                {
                    if (i == listOfCities[j].Id)
                    {
                        pathR.List.Add(listOfCities[j]);
                        list.Add(listOfCities[j]);
                    }
                }
            }

            distance = pathR.CalculatePathDistance();

            DisplayPath(path, cost, timeSpan);
            //DisplayPath(list, cost, timeSpan);
            //Console.WriteLine("Calculated distance : {0}", distance);
        }
        private static void DisplayPath(List<City> list, double cost, TimeSpan timeSpan)
        {
            int j = 0;
            Console.WriteLine("Path : ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"({list.First().Id})");
            Console.ResetColor();
            Console.Write(" --> ");

            foreach (City i in list)
            {
                j++;
                Console.ForegroundColor = ConsoleColor.Green;
                if ((i != list.First()) && (i != list.Last()))
                {
                    Console.Write($"({i.Id})");
                    Console.ResetColor();
                    Console.Write(" --> ");
                }
                if ((j + 1) % 21 == 0)
                {
                    Console.WriteLine();
                    Console.Write("--> ");
                }
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"({list.Last().Id})");
            Console.ResetColor();

            Console.WriteLine("Total cost : {0:F3}", cost);
            Console.WriteLine($"Time elapsed : {DisplayTimeSpan(timeSpan)}");
        }
        private static void DisplayPath(Queue<int> path, double cost, TimeSpan interval)
        {
            int j = 0;
            Console.WriteLine("Path : ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"({path.First()})");
            Console.ResetColor();
            Console.Write(" --> ");

            foreach (int i in path)
            {
                j++;
                Console.ForegroundColor = ConsoleColor.Green;
                if ((i != path.First()) && (i != path.Last()))
                { 
                    Console.Write($"({i})");
                    Console.ResetColor();
                    Console.Write(" --> ");
                }
                if ((j + 1) % 21 == 0)
                {
                    Console.WriteLine();
                    Console.Write("--> ");
                }
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"({path.Last()})");
            Console.ResetColor();

            Console.WriteLine("Total cost : {0:F3}", cost);
            Console.WriteLine($"Time elapsed : {DisplayTimeSpan(interval)}");

        }
        private static double[,] FindMatrix (int currentCityId, Dictionary <int, double[,]> potentialNewMatrices)
        {
            double[,] retVal = potentialNewMatrices.Values.First();

            foreach (var i in potentialNewMatrices)
            {
                if (i.Key == currentCityId)
                {
                    retVal = i.Value;
                }
            }

            return retVal;
        }
        private static int FindKey (double cost, Dictionary<int, double> nodCost)
        {
            int retVal = -1;

            foreach (var i in nodCost)
            {
                if (i.Value == cost)
                {
                    retVal = i.Key;
                    break;
                }
            }

            return retVal;
        }
        private static void DisplayCosts(Dictionary<int, double> nodCost)
        {
            Console.WriteLine("Costs : ");
            foreach (KeyValuePair<int, double> i in nodCost)
            {
                if (i.Value == double.PositiveInfinity)
                {
                    Console.WriteLine($"{i.Key} : Inf.");
                }
                else
                { 
                Console.WriteLine($"{i.Key} : {i.Value}");
                }
            }
        }   
        private static double DoWork (int currentCityId, int potentialCityId, double[,] tempMatrix, double cost, ref Dictionary<int, double[,]> potentialNewMatrices, int originCity)
        {
            double[,] newMatrix = new double[tempMatrix.GetLength(0), tempMatrix.GetLength(1)];
            for (int i = 0; i < tempMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < tempMatrix.GetLength(0); j++)
                {
                    newMatrix[i, j] = tempMatrix[i, j];
                }
            }

            for (int i = 0; i < newMatrix.GetLength(0); ++i)
            {
                newMatrix[currentCityId - 1, i] = double.PositiveInfinity;
                newMatrix[i, potentialCityId - 1] = double.PositiveInfinity;
            }

            newMatrix[potentialCityId - 1, originCity - 1] = double.PositiveInfinity;

            double[] rowMin = new double[newMatrix.GetLength(0)];
            double[] columnMin = new double[newMatrix.GetLength(1)];

            for (int i = 0; i < rowMin.Length; ++i)
            {
                rowMin[i] = FindRowMin(newMatrix, i);
            }

            RowReduction(rowMin, ref newMatrix);

            for (int i = 0; i < columnMin.Length; ++i)
            {
                columnMin[i] = FindColumnMin(newMatrix, i);
            }
            
            ColumnReduction(ref newMatrix, columnMin);

            CalculateCost(ref cost, Sum(rowMin), Sum(columnMin), tempMatrix, currentCityId - 1, potentialCityId - 1);
           
            potentialNewMatrices.Add(potentialCityId, newMatrix);

            return cost;
        }
        private static void CalculateCost(ref double cost, double sr, double sc, double[,] reducedMatrix, int currentCityInd, int potentialCityInd)
        {
            if (currentCityInd == potentialCityInd)
            {
                cost += sr + sc + 0;
            } 
            else if (reducedMatrix[currentCityInd, potentialCityInd] == double.PositiveInfinity)
            {
                cost = double.PositiveInfinity;
            }
            else
            {
                cost += sr + sc + reducedMatrix[currentCityInd, potentialCityInd];
            }
        }
        private static double Sum (double[] v)
        {
            double retVal = 0;

            foreach (double d in v)
            {
                retVal += d;
            }

            return retVal;
        }
        private static void ColumnReduction (ref double[,] reducedMatrix, double[] column)
        {
            for (int i = 0; i < reducedMatrix.GetLength(1); ++i)
            {
                if (column[i] != 0)
                    for (int j = 0; j < reducedMatrix.GetLength(0); ++j)
                    {
                        if (reducedMatrix[j, i] != double.PositiveInfinity)
                        {
                            reducedMatrix[j, i] = reducedMatrix[j, i] - column[i];
                        }
                    }
            }
        }
        private static void RowReduction(double[] row, ref double[,] reducedMatrix)
        {
            for (int i = 0; i < reducedMatrix.GetLength(0); ++i)
            {
                if (row[i] != 0)
                {
                    for (int j = 0; j < reducedMatrix.GetLength(1); ++j)
                    {
                        if (reducedMatrix[i, j] != double.PositiveInfinity)
                        {
                            reducedMatrix[i, j] = reducedMatrix[i, j] - row[i];
                        }
                    }
                }
            }
        }
        private static double FindColumnMin (double [,] m, int column)
        {
            double retVal = m[0, column];

            for (int i=0; i < m.GetLength(1); ++i)
            {
                if (retVal > m[i, column])
                {
                    retVal = m[i, column];
                }
            }

            if (retVal == double.PositiveInfinity)
            {
                retVal = 0;     //all infinities, can't reduce
            }

            return retVal; 
        }
        private static double FindRowMin (double[,] m, int row)
        {
            double retVal = m[row, 0];

            for (int i=0; i < m.GetLength(1); ++i)
            {
                if (retVal>m[row, i])
                {
                    retVal = m[row, i];
                }
            }

            if (retVal == double.PositiveInfinity)
            {
                retVal = 0;     //all infinities, can't reduce
            }

            return retVal;
        } 
        private static void DisplayMatrix(double[,] m)
        {
            int k = m.GetLength(0);

            Console.WriteLine();

            for (int i = 0; i < m.GetLength(0); ++i)
            {
                for (int j = 0; j < m.GetLength(1); ++j) { 
                    if (m[i,j] != double.PositiveInfinity)
                    {
                        if (m[i,j]==0)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write($"{m[i, j]:F3}\t");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.Write($"{m[i, j]:F3}\t");
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Inf.\t");
                        Console.ResetColor();
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        private static string DisplayTimeSpan(TimeSpan interval)
        {
            string retVal = "Time elapsed : ";

            if (interval.TotalMinutes < 1)
            {
                retVal += $"{(interval.Seconds)}s";
            }
            else if (interval.TotalHours < 1)
            {
                retVal += $"{interval.Minutes}m:{(interval.Seconds):D2}s";
            }
            else
            {
                retVal += $"{interval.Hours}h:{(interval.Minutes):D2}m:{(interval.Seconds):D2}s"; 
            }

            retVal += $"\n{(interval.TotalSeconds):F3} total seconds";

            return retVal;
        }
        private static void TwoOptSwap(ref int i, ref int j, out Path newPath, Path path)
        {
            newPath = new Path();

            for (int k = 0; k <= i - 1; ++k)
            {
                newPath.Add(path[k]);
            }

            for (int k = j; k >= i; --k)
            {
                newPath.Add(path[k]);
            }

            for (int k = j + 1; k < path.NumOfCities(); ++k)
            {
                newPath.Add(path[k]);
            }

            newPath.CalculatePathDistance();
            
        }
        private static City FindClosestCity (City currentCity, MyList list)
        {
            City nextCity;
            double minDistance = CalculateDistance(currentCity, list.List[0]);
            nextCity = list.List[0];
            double temp;

            for (int i = 1; i < list.NumOfCities(); ++i)
            {
                if (minDistance > (temp = CalculateDistance(currentCity, list.List[i])))
                {
                    nextCity = list.List[i];
                    minDistance = temp;
                }
            }

            return nextCity;
        }
        public static double CalculateDistance(City start, City finish)
        {
            if (start.Id != finish.Id)
            {
                return Math.Sqrt(Math.Pow(start.CoorX - finish.CoorX, 2) + Math.Pow(start.CoorY - finish.CoorY, 2));
            }
            else return double.PositiveInfinity;
        }
    }
}
