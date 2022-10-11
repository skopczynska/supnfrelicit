using System.Data;
using SupportingElicitation.Lib.Math;

namespace SupportingElicitation.Lib
{
    public class EliciationStatistics
    {
        public DataGrid CompletnessOfReqs { get; private set; }
        public DataGrid PrecisionOfTemplates { get; private set; }
        public DataGrid CompletnessOfTemplates { get; private set; }
        private int bucketPercSize = 10;
        private int maxNumOfTemplatesPercPerBucket = 0;


        public EliciationStatistics(int bucketPercSize, int maxNumOfTemplatesPercPerBucket)
        {
            CompletnessOfReqs = new DataGrid();
            PrecisionOfTemplates = new DataGrid();
            CompletnessOfTemplates = new DataGrid();
            CompletnessOfReqsPerBucket = new DataGrid();
            CompletnessOfTemplatesPerBucket = new DataGrid();
            PrecisionOfTemplatesPerBucket = new DataGrid();

            this.bucketPercSize = bucketPercSize;
            this.maxNumOfTemplatesPercPerBucket = maxNumOfTemplatesPercPerBucket;
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
        public DataGrid CompletnessOfReqsPerBucket { get; private set; }
        public DataGrid CompletnessOfTemplatesPerBucket { get; private set; }
        public DataGrid PrecisionOfTemplatesPerBucket { get; private set;}
        internal void CalculateFinalStatistics(int projectID, StatMeasuresCollector statMeasuresCollector)
        {
            //int bucketSize = (((int)statMeasuresCollector.TotalNumberOfTemplates) * BucketPercSize) / 100; // Number of templates per bucket
            //bucketSize = bucketSize == 0 ? 1 : bucketSize;
            //int numberOfBuckets = ((int) statMeasuresCollector.TotalNumberOfTemplates) / bucketSize;
            Dictionary<int, List<int>> buckets = MathStat.GetBuckets((int)statMeasuresCollector.TotalNumberOfTemplates, bucketPercSize, maxNumOfTemplatesPercPerBucket);
            ///Console.WriteLine(String.Format("P: {0}, bucket Size: {1} numOfBuckets: {2}, temp: {3}", projectID, bucketSize, numberOfBuckets, statMeasuresCollector.TotalNumberOfTemplates));

            foreach (var bucket in buckets)
			{
                List<double> valuesOfCompletnessReqsPerBucket = new List<double>();
                List<double> valuesOfCompletnessTemplPerBucket = new List<double>();
                List<double>valuesOfpresicionOfTemplatesPerBucket = new List<double>();

                foreach (var templateNum in bucket.Value)
                //for (int j = (i-1)*bucketSize +1; j <= bucketSize*i; j++)
                {
                    valuesOfCompletnessReqsPerBucket.Add(CompletnessOfReqs.GetValue(templateNum.ToString(), projectID));
                    valuesOfCompletnessTemplPerBucket.Add(CompletnessOfTemplates.GetValue(templateNum.ToString(), projectID));
                    valuesOfpresicionOfTemplatesPerBucket.Add(PrecisionOfTemplates.GetValue(templateNum.ToString(), projectID));
                }

                if (valuesOfCompletnessReqsPerBucket.Count() == 0)
                {
                    if (bucket.Key == BucketPercSize) // First item add 0
                    {
                        CompletnessOfReqsPerBucket.AddNewValue(bucket.Key.ToString(), projectID, 0);
                        CompletnessOfTemplatesPerBucket.AddNewValue(bucket.Key.ToString(), projectID, 0);
                        PrecisionOfTemplatesPerBucket.AddNewValue(bucket.Key.ToString(), projectID, 0);
                    }
                    else // other items add the previous value
                    {
                        CompletnessOfReqsPerBucket.AddNewValue(bucket.Key.ToString(), projectID,
                            CompletnessOfReqsPerBucket.GetValue((bucket.Key - BucketPercSize).ToString(), projectID));
                        CompletnessOfTemplatesPerBucket.AddNewValue(bucket.Key.ToString(), projectID,
                            CompletnessOfTemplatesPerBucket.GetValue((bucket.Key - BucketPercSize).ToString(), projectID));
                        PrecisionOfTemplatesPerBucket.AddNewValue(bucket.Key.ToString(), projectID,
                            PrecisionOfTemplatesPerBucket.GetValue((bucket.Key - BucketPercSize).ToString(), projectID));


                    }
                }
                else
                {
                    CompletnessOfReqsPerBucket.AddNewValue(bucket.Key.ToString(), projectID, MathStat.Median(valuesOfCompletnessReqsPerBucket));
                    CompletnessOfTemplatesPerBucket.AddNewValue(bucket.Key.ToString(), projectID, MathStat.Median(valuesOfCompletnessTemplPerBucket));
                    PrecisionOfTemplatesPerBucket.AddNewValue(bucket.Key.ToString(), projectID, MathStat.Median(valuesOfpresicionOfTemplatesPerBucket));
                }
			}
        }
        

        internal void Join(EliciationStatistics currentEliciationSessionResult)
        {
            CompletnessOfReqs.Join(currentEliciationSessionResult.CompletnessOfReqs);
            CompletnessOfTemplates.Join(currentEliciationSessionResult.CompletnessOfTemplates);
            PrecisionOfTemplates.Join(currentEliciationSessionResult.PrecisionOfTemplates);

            CompletnessOfReqsPerBucket.Join(currentEliciationSessionResult.CompletnessOfReqsPerBucket);
            CompletnessOfTemplatesPerBucket.Join(currentEliciationSessionResult.CompletnessOfTemplatesPerBucket);
            PrecisionOfTemplatesPerBucket.Join(currentEliciationSessionResult.PrecisionOfTemplatesPerBucket);

        }
    }
}