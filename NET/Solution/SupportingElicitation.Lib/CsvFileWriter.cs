using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportingElicitation.Lib
{
    public class CsvFileWriter
    {
        public static void SaveToCsv(DataGrid dataGridToSave, string filePath)
        {
            using(StreamWriter sw = new StreamWriter(filePath))
            {
                double numOfColumns = dataGridToSave.GetNumOfColumns();
                string line = "ID";
                for (int i=1; i<=numOfColumns; i++)
                {
                    line = string.Format("{0};{1}", line, i);
                }
                sw.WriteLine(line);
                foreach (var row in dataGridToSave)
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
