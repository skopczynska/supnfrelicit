// See https://aka.ms/new-console-template for more information
using Microsoft.Data.Analysis;
using SupportingElicitation.Lib;


InputDataProvider.ReadDataFromFile(@"D:\SVNs\supnfrelicit\input\FreqMatrix.csv");
DataFrame inputData = InputDataProvider.FrequencyMatrix;
var inputData2 = InputDataProvider.FrequencyMatrixDic;

MostTemplates mT;
int numOfProjects = inputData2.GetNumOfColumns();

DataGrid reqCompletness = new DataGrid();
DataGrid templateCompletness = new DataGrid();
DataGrid precision = new DataGrid();

Dictionary<int, double> numOfTemplates = inputData2.GetFrequencyOfColumns();
Dictionary<int, double> numOfRequirements = inputData2.GetSumOfColumns();

for (int i = 1; i <= numOfProjects; i++)
{
    mT = new MostTemplates();
    DataGrid inputDataWithOutOneProject = inputData2.Copy();
    inputDataWithOutOneProject.RemoveColumn(i);
    
    mT.SetUp(inputDataWithOutOneProject);
    double goodTemplates = 0;
    double goodReqs = 0;
    for (int j = 1; j < 11; j++)
    {
        reqCompletness.AddRow(j.ToString());
        templateCompletness.AddRow(j.ToString());
        precision.AddRow(j.ToString());

        string templateID = mT.GetNextTemplateIDToSuggest();
        double numOfReqsFromTemplate = inputData2.GetValue(templateID, i);
        
        if (numOfReqsFromTemplate > 0)
        {
            goodReqs += numOfReqsFromTemplate;
            goodTemplates += 1;
            
        }
        double prec = goodReqs / numOfRequirements[i];
        reqCompletness.AddNewValue(j.ToString(), i, prec);
        templateCompletness.AddNewValue(j.ToString(), i, goodTemplates / numOfTemplates[i]);
        precision.AddNewValue(j.ToString(), i, goodTemplates / ((double) j));
    }
}

CsvFileWriter.SaveToCsv(reqCompletness, "reqCompletness.csv");
CsvFileWriter.SaveToCsv(templateCompletness, "templateCompletness.csv");
CsvFileWriter.SaveToCsv(precision, "precision.csv");

Console.WriteLine(inputData2.GetColumnValues(1));

Console.ReadLine();



