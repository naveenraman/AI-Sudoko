using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoko_AI
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Stopwatch stopwatch;
            TimeSpan elapsed_time;
            if (args == null || args.Length == 0)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                Console.WriteLine("Please select Folder");
                Console.WriteLine("NOTE:all the files in this folder would be processed");
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    foreach (var path in Directory.GetFiles(fbd.SelectedPath, "*.txt"))
                    {
                        Console.WriteLine("SUDUKO -  MRV Agent for file {0}\n", System.IO.Path.GetFileName(path));
                        Console.WriteLine("INPUT\n");
                        PrintInputGrid(InputGrid(path));
                        var mrv = GetGrids(path);
                        Solver mrvSolver = new Solver(mrv);
                        stopwatch = new Stopwatch();
                        stopwatch.Start();
                        mrvSolver.Run("MRV");
                        stopwatch.Stop();
                        elapsed_time = stopwatch.Elapsed;
                        Console.WriteLine("OUTPUT\n");
                        PrintOutputGrid(mrvSolver);
                        mrvSolver.PrintGridSteps();
                        Console.WriteLine("Time to solve (hh:mm:ss) : {0}\n", elapsed_time);

                        Console.WriteLine("SUDUKO -  Uninformed Agent for file {0}\n", System.IO.Path.GetFileName(path));
                        Console.WriteLine("INPUT\n");
                        PrintInputGrid(InputGrid(path));
                        var uninformed = GetGrids(path);
                        Solver uninformedSolver = new Solver(uninformed);
                        stopwatch = new Stopwatch();
                        stopwatch.Start();
                        uninformedSolver.Run("UNINFORMED");
                        stopwatch.Stop();
                        elapsed_time = stopwatch.Elapsed;
                        Console.WriteLine("OUTPUT\n");
                        PrintOutputGrid(uninformedSolver);
                        uninformedSolver.PrintGridSteps();
                        Console.WriteLine("Time to solve (hh:mm:ss) : {0}\n", elapsed_time);
                    }
                }
            }
            Console.ReadKey();
        }

        private static int[,] GetGrids(string path)
        {
            string[] lines = File.ReadAllLines(path);
            int count = lines.Count();
            int[,] sudokuArray = new int[count, count];
            if (lines.Length > 0)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    for (int j = 0; j < line.Length; j++)
                    {
                        string square = line[j].ToString();
                        int dec = GetHexDecimal(square);
                        sudokuArray[i, j] = dec;
                    }
                }
            }
            return sudokuArray;
        }

        private static string[,] InputGrid(string path)
        {
            string[] lines = File.ReadAllLines(path);
            int count = lines.Count();
            string[,] inputArray = new string[count, count];
            if (lines.Length > 0)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    for (int j = 0; j < line.Length; j++)
                    {
                        inputArray[i, j] = line[j].ToString(); ;
                    }
                }
            }
            return inputArray;
        }

        private static int GetHexDecimal(string num)
        {
            switch (num)
            {
                case "-":
                    return 0;
                case "0":
                    return 0;
                case "1":
                    return 1;
                case "2":
                    return 2;
                case "3":
                    return 3;
                case "4":
                    return 4;
                case "5":
                    return 5;
                case "6":
                    return 6;
                case "7":
                    return 7;
                case "8":
                    return 8;
                case "9":
                    return 9;
                case "A":
                    return 10;
                case "B":
                    return 11;
                case "C":
                    return 12;
                case "D":
                    return 13;
                case "E":
                    return 14;
                case "F":
                    return 15;
            }
            return -1;
        }

        public static string[,] ConvertToDecimal(Solver solver)
        {
            string[,] gridData = new string[solver.gridSize, solver.gridSize];
            for (int i = 0; i < solver.gridSize; i++)
            {
                for (int j = 0; j < solver.gridSize; j++)
                {
                    var num = solver.grid[i, j];
                    gridData[i, j] = GetDecimal(num);
                }
            }
            return gridData;
        }

        private static string GetDecimal(int num)
        {
            switch (num)
            {
                case 0:
                    return "0";
                case 1:
                    return "1";
                case 2:
                    return "2";
                case 3:
                    return "3";
                case 4:
                    return "4";
                case 5:
                    return "5";
                case 6:
                    return "6";
                case 7:
                    return "7";
                case 8:
                    return "8";
                case 9:
                    return "9";
                case 10:
                    return "A";
                case 11:
                    return "B";
                case 12:
                    return "C";
                case 13:
                    return "D";
                case 14:
                    return "E";
                case 15:
                    return "F";
                case 16:
                    return "0";

            }
            return "-1";
        }

        public static void PrintInputGrid(string[,] inputGrid)
        {
            for (int i = 0; i < inputGrid.GetLength(0); i++)
            {
                for (int j = 0; j < inputGrid.GetLength(0); j++)
                {
                    if (inputGrid[i, j].Length > 0)
                    {
                        Console.Write(inputGrid[i, j]);
                    }
                    else
                    {
                        Console.Write('-');
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public static void PrintOutputGrid(Solver solver)
        {
            var printGrid = ConvertToDecimal(solver);
            for (int i = 0; i < solver.gridSize; i++)
            {
                for (int j = 0; j < solver.gridSize; j++)
                {
                    if (printGrid[i, j].Length > 0)
                    {
                        Console.Write(printGrid[i, j]);
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
