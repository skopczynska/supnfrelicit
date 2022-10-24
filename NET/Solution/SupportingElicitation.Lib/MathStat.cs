using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportingElicitation.Lib.Math
{

	public class MathStat
	{

		public static double Median(IEnumerable<double> listOfValues)
		{
			if (listOfValues.Count() == 0)
				return 0;
			
			int numberOfItems = listOfValues.Count();
			int halfIndex = listOfValues.Count() / 2;
			var sortedValues = listOfValues.OrderBy(n => n);
			double median;

			if ((numberOfItems % 2) == 0)
			{
				median = ((sortedValues.ElementAt(halfIndex) +
					sortedValues.ElementAt(halfIndex - 1)) / 2);
			}
			else
			{
				median = sortedValues.ElementAt(halfIndex);
			}

			
			return median;
		}
		//private const int MaxPerctangeOfProposedTemplates = 500; //How many more templates (in perc) the algorithm shall consider
		public static Dictionary<int, List<int>> GetBuckets(int numberOfTemplates, int numberOfProposedTemplates, int bucketSizePerc, int maxPerctangeOfProposedTemplates)
		{
			Dictionary<int, List<int>> buckets = new Dictionary<int, List<int>>();
			int numOfBuckets = maxPerctangeOfProposedTemplates / bucketSizePerc;
			numOfBuckets = maxPerctangeOfProposedTemplates % bucketSizePerc !=0 ? numOfBuckets + 1 : numOfBuckets;
			for (int i = 0; i < numOfBuckets; i++)
			{
				buckets.Add((i+1)* bucketSizePerc , new List<int>());
			}

			for (int i = 1; i <= numberOfProposedTemplates; i++)
			{
				int templatePerc = (int) (((double) i / numberOfTemplates) * 100); //100 because it is percentage
				int templateBucketPerc = GetBucketPerc(templatePerc, bucketSizePerc, numOfBuckets, maxPerctangeOfProposedTemplates);
				if (templateBucketPerc > maxPerctangeOfProposedTemplates)
					break;
				buckets[templateBucketPerc].Add(i);
			}
			
			return buckets;
		}

		private static int GetBucketPerc(int templatePrec, int bucketSizePrec, int numOfBuckets, int maxPerctangeOfProposedTemplates)
		{

			int currentPerc = maxPerctangeOfProposedTemplates;
			int currentBucket = numOfBuckets;

			while (!(templatePrec <= currentPerc && templatePrec > currentPerc - bucketSizePrec))
			{
				currentPerc -= bucketSizePrec;
                currentBucket--;
			}

			return currentPerc;

		}
	}
}