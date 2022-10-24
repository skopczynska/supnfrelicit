using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportingElicitation.Lib
{
    internal class StatMeasuresCollector
    {
        
        internal double TotalNumberOfRequirements { get; private set; }
        internal double TotalNumberOfTemplates { get; private set; }
        internal double NumberOfProposedTemplates { get; private set; }
        internal double NumberOfGoodRequirements { get; private set; }
        internal double NumberOfBadRequirements { get; private set; }

        internal double NumberOfGoodTemplates { get; private set; }
        internal double NumberOfBadTemplates { get; private set; }  


        public StatMeasuresCollector(Dictionary<string, double> project)
        {
            TotalNumberOfRequirements =  project.Sum(a => a.Value);
            TotalNumberOfTemplates =  project.Count(a => a.Value > 0);
        }

        internal void AddNumberOfBadReqsForTemplate(double numberOfReqs)
        {
            NumberOfBadRequirements += numberOfReqs;
            NumberOfBadTemplates++;
        }

        internal void AddNumberOfGoodReqsForTemplate(double numberOfReqs)
        {
            NumberOfGoodRequirements +=  numberOfReqs ;
            NumberOfGoodTemplates++;
            
        }

        internal void AddOneTemplateProposed()
        {
            NumberOfProposedTemplates++;
        }

        internal bool AreAllTemplatesSuggested()
        {
            return TotalNumberOfTemplates == NumberOfGoodTemplates;
        }
    }
}
