using SupportingElicitation.Lib.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportingElicitation.Lib
{
    public class Simulator
    {
        private DataGrid data;
        private IAlgorithm algorithm;

        public Simulator(DataGrid data, IAlgorithm algorithm)
        {
            this.data = data;
            this.algorithm = algorithm;
        }


        public EliciationStatistics RunSimulations(int numberOfSimulations, int bucketPercSize, int maxNumOfTemplatesPercToConsider)
        {
            EliciationStatistics allEliciationSessionsResults = new EliciationStatistics(bucketPercSize, maxNumOfTemplatesPercToConsider);
            for (int i = 0; i < numberOfSimulations; i++)
            {
                EliciationStatistics currentEliciationSessionResult = RunSimulationForCatalog(bucketPercSize, maxNumOfTemplatesPercToConsider);
                allEliciationSessionsResults.Join(currentEliciationSessionResult);
            }

            return allEliciationSessionsResults;
        }

        private EliciationStatistics RunSimulationForCatalog(int bucketPercSize, int maxNumOfTemplatesPercToConsider)
        {
            EliciationStatistics eliciationStatisticsForOneSimulation = new EliciationStatistics(bucketPercSize, maxNumOfTemplatesPercToConsider);
            for (int i = 1; i <= data.GetNumOfColumns(); i++) //for each project (projects are columns)
            {
                DataGrid dataExceptOneProject = data.Copy();
                dataExceptOneProject.RemoveColumn(i);
                dataExceptOneProject.RemoveRowsWithOnlyZeroValues();

                Dictionary<string, double> projectReqs = data.GetFirstColumnAndColumnValues(i);

                ElicitNFRsForProject(i, projectReqs, dataExceptOneProject, ref eliciationStatisticsForOneSimulation);
            }

            return eliciationStatisticsForOneSimulation;
        }

        private void ElicitNFRsForProject(int projectID, Dictionary<string, double> projectReqs, DataGrid dataExceptOneProject, ref EliciationStatistics eliciationStatistics)
        {
            algorithm.SetUp(dataExceptOneProject);
            if (algorithm is IInteractiveAlgorithm)
                ((IInteractiveAlgorithm)algorithm).SetStartTemplate(GetRandomGoodTemplate(projectReqs));

            StatMeasuresCollector statMeasuresCollector = new(projectReqs);
            
            string template = algorithm.GetNextTemplateIDToSuggest();

            while (template != String.Empty && !statMeasuresCollector.AreAllTemplatesSuggested())
            {
                bool isProjectUsingTemplate = Oracle.IsProjectUsingTemplate(projectReqs, template);
                if (isProjectUsingTemplate)
                {
                    statMeasuresCollector.AddNumberOfGoodReqsForTemplate(Oracle.GetNumberOfReqsForTemplate(projectReqs, template));
                }
                else
                {
                    statMeasuresCollector.AddNumberOfBadReqsForTemplate(Oracle.GetNumberOfReqsForTemplate(projectReqs, template));
                }
                
                if (algorithm is IInteractiveAlgorithm)
                    ((IInteractiveAlgorithm)algorithm).ProvideFeedbackAboutTemplate(template, isProjectUsingTemplate);

                statMeasuresCollector.AddOneTemplateProposed();
                eliciationStatistics.Update(projectID, statMeasuresCollector);
                template = algorithm.GetNextTemplateIDToSuggest();
                Console.WriteLine($"{statMeasuresCollector.NumberOfGoodTemplates} / {statMeasuresCollector.TotalNumberOfTemplates}");
            }
            eliciationStatistics.CalculateFinalStatistics(projectID, statMeasuresCollector);
            Console.WriteLine($"Elicited for project {projectID}");
        }

        private string GetRandomGoodTemplate(Dictionary<string, double> projectReqs)
        {
            System.Random rand = new System.Random();
            var reqsOfProject = projectReqs.Where(pR => pR.Value > 0);
            return reqsOfProject.ElementAt(rand.Next(reqsOfProject.Count())).Key;
        }
    }
}
