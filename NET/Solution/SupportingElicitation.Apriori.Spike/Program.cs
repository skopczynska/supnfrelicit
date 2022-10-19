using Accord.MachineLearning.Rules;
using Microsoft.Data.Analysis;
using SupportingElicitation.Lib;
using SupportingElicitation.Lib.Algorithms;
using System.Linq;

internal class Program
{

    public static List<int> getTemplatesToCompare(List<AssociationRule<int>> rules, List<int> elementsConsidered)
    {
        List<int> templatesToConsider = new List<int>();
        foreach (var rule in rules)
        {
            templatesToConsider.AddRange(rule.X.Where(item => !elementsConsidered.Contains(item) && !templatesToConsider.Contains(item)));
            templatesToConsider.AddRange(rule.Y.Where(item => !elementsConsidered.Contains(item) && !templatesToConsider.Contains(item)));
        }

        return templatesToConsider;
    }

    private static void Main(string[] args)
    {
        int bucketSize = 10;
        int maxNumOfTemplatesPercToConsider = 500;

        DataGrid inputDataFM = InputDataProvider.ReadDataFromFile(@"D:\SVNs\supnfrelicit\input\FreqMatrix.csv");


        MostTemplates alg = new MostTemplates();
        alg.SetUp(inputDataFM.Copy());

        string startTemplate = "10";

        List<int> projectsOfTemplate = inputDataFM.GetRowValues(startTemplate).Where(r => r.Value > 0).Select(rs => rs.Key).ToList();
        int[][] learningSet = new int[projectsOfTemplate.Count][];
        int i = 0;
        foreach (int project in projectsOfTemplate)
        {
            List<double> templateOccurance = inputDataFM.GetColumnValues(project);
            List<int> templatesUsed = new List<int>();
            for (int j = 0; j < templateOccurance.Count; j++)
            {
                if (templateOccurance[j] > 0)
                    templatesUsed.Add(j);
            }

            learningSet[i] = templatesUsed.ToArray();
            i++;
        }

        Apriori apriori = new Apriori(learningSet.Length / 3, 0.5);
        var asM = apriori.Learn(learningSet);
        var freq = apriori.Frequent;

        var rules = asM.Rules;

        var rule = asM.GetHigestRule(new List<int>() { 10 });
        var templatesToCompare = getTemplatesToCompare(rule, new List<int>() { 10 });
        var bestTemplate = getBestTemplate(templatesToCompare, alg);







        // See https://aka.ms/new-console-template for more information
        Console.WriteLine("Hello, World Apriori!");
    }

    private static int getBestTemplate(List<int> templatesToCompare, MostTemplates alg)
    {
        int bestTemplate = templatesToCompare[0];
        int rankOfBestTemplate = alg.GetRankOfTemplate(bestTemplate.ToString());
        foreach (var template in templatesToCompare)
        {
            if (alg.GetRankOfTemplate(template.ToString()) > rankOfBestTemplate)
            {
                bestTemplate = template;
                rankOfBestTemplate = alg.GetRankOfTemplate(template.ToString());
            }
        }

        return bestTemplate;
    }
}