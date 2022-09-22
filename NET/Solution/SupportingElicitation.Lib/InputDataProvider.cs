using Microsoft.Data.Analysis;

namespace SupportingElicitation.Lib
{
    public static class InputDataProvider
    {
        
        public static DataFrame FrequencyMatrix
        {
            get;

            private set;
        }

        public static DataGrid FrequencyMatrixDic { get; private set; }


        public static bool ReadDataFromFile(string filePath)
        {
            FrequencyMatrix = DataFrame.LoadCsv(filePath, header: true, separator:';', guessRows:600);

            Dictionary<string, Dictionary<int, double>> frequencyMatrixDicTemp = new Dictionary<string, Dictionary<int, double>>();
            Dictionary<int, double> templatesPerProject = null;
            using (var reader = new StreamReader(filePath))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    templatesPerProject = new Dictionary<int, double>();
                    for (int i=1; i < values.Length; i++)
                    {
                        templatesPerProject.Add(i, int.Parse(values[i]));
                    }
                    frequencyMatrixDicTemp.Add(values[0], templatesPerProject);
                }
                FrequencyMatrixDic = new DataGrid(frequencyMatrixDicTemp);
            }

            return true;
        }

        
    }
}