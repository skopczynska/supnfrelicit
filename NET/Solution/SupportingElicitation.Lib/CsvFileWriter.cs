using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportingElicitation.Lib
{
    public static class CsvFileWriter
    {
        public static void SaveToCsv(DataGrid dataToSave, string filePath)
        {
            using(StreamWriter sw = new StreamWriter(filePath))
            {
                double numOfColumns = dataToSave.GetNumOfColumns();
                string line = "ID";
                for (int i=1; i<=numOfColumns; i++)
                {
                    line = string.Format("{0};{1}", line, i);
                }
                sw.WriteLine(line);
                foreach (var row in dataToSave)
                {
                    line = string.Format("{0}", row.Key);
                    foreach (var column in row.Value.Values)
                    {
                        line = string.Format("{0};{1}", line, column);
                    }
                    sw.WriteLine(line);
                }
            }

        }
    }
}
