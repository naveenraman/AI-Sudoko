using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoko_AI
{
    public class Solver : Sudoku
    {
        private Dictionary<string, List<int>> domains;
        private int nbStep = 0;
        public Solver(int[,] newGrid) : base(newGrid) { }

        public int Run(string name)
        {
            domains = new Dictionary<string, List<int>>();
            GenerateDomains();
            int res = RecursiveBacktracking(name);
            return res;
        }

        private string MRV()
        {
            int minValues = gridSize + 1;

            List<int> possibleValues;

            int vari = 0;
            int varj = 0;

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (grid[i, j] == 0)
                    {
                        possibleValues = new List<int>(valuesEnum);

                        string coordStr = IjToCoord(i, j);

                        List<string> constrainElem = constraints[coordStr];

                        foreach (string elem in constrainElem)
                        {
                            List<int> coordIj = CoordToIj(elem);

                            if (possibleValues.Contains(grid[coordIj[0], coordIj[1]]) == true)
                            {
                                possibleValues.Remove(grid[coordIj[0], coordIj[1]]);
                            }
                        }

                        if (possibleValues.Count < minValues)
                        {
                            minValues = possibleValues.Count;
                            vari = i;
                            varj = j;
                        }
                    }
                }
            }

            return IjToCoord(vari, varj);
        }

        private int RecursiveBacktracking(string name)
        {
            if (AssigmentComplete() == true) { return 0; }

            Dictionary<string, List<int>> oldDomains = CopyDomains(domains);
            string var=null;
            if (name == "MRV")
            {
                var = MRV();
            }
            else if(name == "UNINFORMED")
            {
                var = Uninformed();
            }
            List<int> values = new List<int>(domains[var]);
            foreach (int value in values)
            {
                if (CheckConstraints(var, value) == true)
                {
                    List<int> coordIj = CoordToIj(var);
                    int i = coordIj[0];
                    int j = coordIj[1];
                    grid[i, j] = value;
                    List<int> domain = new List<int>(domains[var]);
                    foreach (int val in domain)
                    {
                        if (val != value)
                        {
                            domains[var].Remove(val);
                        }
                    }
                    nbStep++;
                    int result = RecursiveBacktracking(name);
                    if (result == 0) { return result; }
                    grid[i, j] = 0;
                    domains = CopyDomains(oldDomains);
                }
            }
            return 1;
        }

        private bool AssigmentComplete()
        {
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (grid[i, j] == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        private bool CheckConstraints(string var, int val)
        {
            List<string> constrainElem = constraints[var];

            foreach (string elem in constrainElem)
            {
                List<int> coordIj = CoordToIj(elem);

                if (grid[coordIj[0], coordIj[1]] == val)
                {
                    return false;
                }
            }

            return true;
        }

        private Dictionary<string, List<int>> CopyDomains(Dictionary<string, List<int>> oldDomains)
        {
            Dictionary<string, List<int>> newDomains = new Dictionary<string, List<int>>();

            foreach (KeyValuePair<string, List<int>> item in oldDomains)
            {
                List<int> values = new List<int>(item.Value);
                newDomains.Add(item.Key, values);
            }

            return newDomains;
        }

        private void GenerateDomains()
        {
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (grid[i, j] == 0)
                    {
                        domains.Add(IjToCoord(i, j), new List<int>(valuesEnum));
                    }
                    else
                    {
                        domains.Add(IjToCoord(i, j), new List<int> { grid[i, j] });
                    }
                }
            }
        }

        private string Uninformed()
        {
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (grid[i, j] == 0)
                    {
                        return IjToCoord(i, j);
                    }
                }
            }

            return null;
        }

        public void PrintGridSteps()
        {
            Console.WriteLine();
            Console.WriteLine("Num of steps : {0}", nbStep);
        }
    }
}
