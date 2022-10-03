using Microsoft.Data.Analysis;

namespace SupportingElicitation.Lib
{
    public class InputDataProvider
    {
        
       public static DataGrid FrequencyMatrix { get; private set; }


        public static DataGrid ReadDataFromFile(string filePath)
        {
            FrequencyMatrix = new DataGrid();
            
            using (var reader = new StreamReader(filePath))
            {
                reader.ReadLine(); // Omit header in the file
                int rowNumber = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    
                    for (int i= 1; i < values.Length; i++) //Starts from 1 to omit the first column in the file
                    {
                        FrequencyMatrix.AddNewValue(rowNumber.ToString(), i-1, double.Parse(values[i]));
                    }

                    rowNumber++;
                    
                }
            }

            return FrequencyMatrix;
        }

        
    }
}