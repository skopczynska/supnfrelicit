using Microsoft.Data.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportingElicitation.Lib.Algorithms
{
    public class MostTemplates : IAlgorithm
    {
        List<string> rankingOfTemplates;
        int indexOfLastSuggestedTemplate = -1;

        public string Name =>  "MostTemplates";

        public string GetNextTemplateIDToSuggest()
        {
            indexOfLastSuggestedTemplate++;
            if (indexOfLastSuggestedTemplate < rankingOfTemplates.Count)
                return rankingOfTemplates[indexOfLastSuggestedTemplate];
            else
                return string.Empty;

        }

        public void SetUp(DataGrid inputData)
        {
            if (rankingOfTemplates == null)
            {
                CreateRankingOfTemplates(inputData);
            }
            indexOfLastSuggestedTemplate = -1;

        }

        private void CreateRankingOfTemplates(DataGrid inputData)
        {
            Dictionary<string, double> freqOfTemplates = new Dictionary<string, double>();

            foreach (var item in inputData)
            {
                double frequency = GetFrequency(item.Value.Values);
                freqOfTemplates.Add(item.Key, frequency);
            }

            freqOfTemplates = freqOfTemplates.OrderByDescending(a => a.Value).ToDictionary(x => x.Key, x => x.Value);
            rankingOfTemplates = freqOfTemplates.Keys.ToList();
        }

        private double GetFrequency(Dictionary<int, double>.ValueCollection values)
        {
            double frequency = 0;
            foreach (var item in values)
            {
                if (item > 0)
                {
                    frequency++;
                }
            }
            return frequency;
        }
    }
}
