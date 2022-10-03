namespace SupportingElicitation.Lib
{
    public class EliciationStatistics
    {
        public DataGrid CompletnessOfReqs { get; private set; }
        public DataGrid PrecisionOfTemplates { get; private set; }
        public DataGrid CompletnessOfTemplates { get; private set; }


        public EliciationStatistics()
        {
            CompletnessOfReqs = new DataGrid();
            PrecisionOfTemplates = new DataGrid();
            CompletnessOfTemplates = new DataGrid();
        }

       
        internal void Update(int projectID, StatMeasuresCollector statMeasuresCollector)
        {
            double completnessOrReqs = statMeasuresCollector.NumberOfGoodRequirements / statMeasuresCollector.TotalNumberOfRequirements;
            CompletnessOfReqs.AddNewValue(statMeasuresCollector.NumberOfProposedTemplates.ToString(),
                projectID,
                completnessOrReqs
                );

            double completnessOfTemplates = statMeasuresCollector.NumberOfGoodTemplates / statMeasuresCollector.TotalNumberOfTemplates;
            CompletnessOfTemplates.AddNewValue(statMeasuresCollector.NumberOfProposedTemplates.ToString(),
                projectID,
                completnessOfTemplates
                );

            double precisionOfTemplates = statMeasuresCollector.NumberOfGoodTemplates / statMeasuresCollector.NumberOfProposedTemplates;
            PrecisionOfTemplates.AddNewValue(statMeasuresCollector.NumberOfProposedTemplates.ToString(),
                projectID,
                precisionOfTemplates
                );
        }

        internal void Join(EliciationStatistics currentEliciationSessionResult)
        {
            CompletnessOfReqs.Join(currentEliciationSessionResult.CompletnessOfReqs);
            CompletnessOfTemplates.Join(currentEliciationSessionResult.CompletnessOfTemplates);
            PrecisionOfTemplates.Join(currentEliciationSessionResult.PrecisionOfTemplates);

        }
    }
}