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
            for (int i = 0; i < data.GetNumOfColumns(); i++) //for each project (projects are columns)
            {
                DataGrid dataExceptOneProject = data.Copy();
                dataExceptOneProject.RemoveColumn(i);

                Dictionary<string, double> projectReqs = data.GetFirstColumnAndColumnValues(i);

                ElicitNFRsForProject(i, projectReqs, dataExceptOneProject, ref eliciationStatisticsForOneSimulation);
            }

            return eliciationStatisticsForOneSimulation;
        }

        private void ElicitNFRsForProject(int projectID, Dictionary<string, double> projectReqs, DataGrid dataExceptOneProject, ref EliciationStatistics eliciationStatistics)
        {
            algorithm.SetUp(data);
            StatMeasuresCollector statMeasuresCollector = new(projectReqs);

            string template = algorithm.GetNextTemplateIDToSuggest();
            while (template != String.Empty)
            {
                if (Oracle.IsProjectUsingTemplate(projectReqs, template))
                {
                    statMeasuresCollector.AddNumberOfGoodReqsForTemplate(Oracle.GetNumberOfReqsForTemplate(projectReqs, template));
                }
                else
                {
                    statMeasuresCollector.AddNumberOfBadReqsForTemplate(Oracle.GetNumberOfReqsForTemplate(projectReqs, template));
                }
                statMeasuresCollector.AddOneTemplateProposed();
                eliciationStatistics.Update(projectID, statMeasuresCollector);
                template = algorithm.GetNextTemplateIDToSuggest();
            }
            eliciationStatistics.CalculateFinalStatistics(projectID, statMeasuresCollector);
        }
    }
}
