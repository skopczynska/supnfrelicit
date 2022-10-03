using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportingElicitation.Lib.Algorithms
{
    public class Random : IAlgorithm
    {
        private System.Random random;
        private int maxNumberOfTemplatesToSuggest;
        private List<string> listOfTemplatesToSuggest;
        public string Name => "Random";

        public string GetNextTemplateIDToSuggest()
        {
            if (listOfTemplatesToSuggest != null && listOfTemplatesToSuggest.Count > 0)
            {
                int randomIndexOfTemplate = random.Next(listOfTemplatesToSuggest.Count);
                string templateIDToReturn = listOfTemplatesToSuggest[randomIndexOfTemplate];
                listOfTemplatesToSuggest.RemoveAt(randomIndexOfTemplate);
                return templateIDToReturn;
            }
            return String.Empty;
        }

        public void SetUp(DataGrid inputData)
        {
            random = new System.Random();
            listOfTemplatesToSuggest = inputData.GetFirstColumnAndColumnValues(1).Keys.ToList();
        }
    }
}
