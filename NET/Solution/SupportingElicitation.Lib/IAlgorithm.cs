using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Analysis;

namespace SupportingElicitation.Lib
{
    public interface IAlgorithm
    {
        public void SetUp(DataGrid inputData);

        public string GetNextTemplateIDToSuggest();


    }
}
