using Microsoft.Data.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportingElicitation.Lib
{
    public class RandomAlgorithm : IAlgorithm
    {
        List<string> rankingOfTemplates;
        int indexOfLastSuggestedTemplate = -1;
        Random random = new Random();

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
            Dictionary<string, double> freqOfTemplates = new Dictionary<string, double>();
            
            foreach (var item in inputData)
            {
                double randomPlace = random.NextDouble();
                while(freqOfTemplates.Values.Contains(randomPlace))
                {
                    randomPlace = random.NextDouble();
                }
                
                freqOfTemplates.Add(item.Key, randomPlace);
            }

            freqOfTemplates = freqOfTemplates.OrderByDescending(a => a.Value).ToDictionary(x=> x.Key, x=>x.Value);
            rankingOfTemplates = freqOfTemplates.Keys.ToList();
        }

        
    }
}
