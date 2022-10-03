using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportingElicitation.Lib
{
    public class DataGrid : IEnumerable<KeyValuePair<string, Dictionary<int, double>>>
    {
        private Dictionary<string, Dictionary<int, double>> data;

        public DataGrid()
        {
            data = new Dictionary<string,Dictionary<int, double>>();
        }

        public DataGrid(Dictionary<string, Dictionary<int, double>> inputData)
        {
            data = inputData;   
        }

        public Dictionary<int, double> GetSumOfColumns()
        {
            Dictionary<int, double> sumOfColumns = new Dictionary<int, double>();
            int numOfColumns = GetNumOfColumns();
            for(int i=1; i<= numOfColumns; i++)
            {
                sumOfColumns.Add(i, 0);
            }
            foreach (var row in data)
            {
                for (int i = 1; i <= numOfColumns; i++)
                {
                    sumOfColumns[i] = sumOfColumns[i] + row.Value[i];
                }

            }
            return sumOfColumns;
        }

        public void AddRow(string j)
        {
            if (!data.ContainsKey(j))
            {
                data.Add(j, new Dictionary<int, double>());
            }
        }

        public void AddNewValue(string row, int column, double value)
        {
            if (!data.ContainsKey(row))
            {
                data.Add(row, new Dictionary<int, double>());
            }
            data[row].Add(column, value);
        }

       

        public Dictionary<int, double> GetFrequencyOfColumns()
        {
            Dictionary<int, double> sumOfColumns = new Dictionary<int, double>();
            int numOfColumns = GetNumOfColumns();
            for (int i = 1; i <= numOfColumns; i++)
            {
                sumOfColumns.Add(i, 0);
            }
            foreach (var row in data)
            {
                for (int i = 1; i <= numOfColumns; i++)
                {
                    if (row.Value[i] > 0)
                    {
                        sumOfColumns[i] = sumOfColumns[i] + 1;
                    }
                }

            }
            return sumOfColumns;
        }

        public double GetValue(string row, int column)
        {
            return data[row][column];
        }

        public List<double> GetColumnValues(int column)
        {
            List<double> list = new List<double>();
            foreach (var row in data)
            {
                list.Add(row.Value[column]);
            }

            return list;
        }

        public Dictionary<string, double> GetFirstColumnAndColumnValues(int column)
        {
            Dictionary<string, double> columnValues = new();
            foreach (var row in data)
            {
                columnValues.Add(row.Key, row.Value[column]);
            }

            return columnValues;
        }

        public List<string> GetRowNames()
        {
            return data.Keys.ToList<string>();
        }

        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<string, Dictionary<int, double>>> GetEnumerator()
        {
            return data.GetEnumerator();

            //return new GridEnumerator<double>(data);
        }

        public void RemoveColumn(int columnNum)
        {
            foreach (var row in data)
            {
                row.Value.Remove(columnNum);
            }
        }

        public int GetNumOfColumns()
        {
            return data.ElementAtOrDefault(0).Value.Count;
        }

        public DataGrid Copy()
        {
            var newData = new Dictionary<string, Dictionary<int, double>>();
            foreach (var row in data)
            {
                Dictionary<int, double> newColumn = new Dictionary<int, double>();
                foreach (var col in row.Value)
                {
                    newColumn.Add(col.Key, col.Value);
                }
                newData.Add(row.Key.ToString(), newColumn);
             }
            
            DataGrid newDG = new DataGrid(newData);
            return newDG;
        }

       

        internal void Join(DataGrid dataGridToBeAdded)
        {
            foreach(var row in dataGridToBeAdded)
            {
                
                foreach (var columnValuePair in row.Value)
                {
                    int nextColumnID = data.ContainsKey(row.Key) ? data[row.Key].Keys.Max() + 1 : 1;
                    this.AddNewValue(row.Key, nextColumnID, columnValuePair.Value);
                }
            }
        }
    }

   
}
