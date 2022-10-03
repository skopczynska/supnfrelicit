 // See https://aka.ms/new-console-template for more information
using Microsoft.Data.Analysis;
using SupportingElicitation.Lib;
using SupportingElicitation.Lib.Algorithms;

DataGrid inputDataFM = InputDataProvider.ReadDataFromFile(@"D:\SVNs\supnfrelicit\input\FreqMatrix.csv");

IAlgorithm algorithm = new MostTemplates();
Simulator simulator = new Simulator(inputDataFM, algorithm);

int simulationNum = 1;
EliciationStatistics result = simulator.RunSimulations(simulationNum);

string resultFileEnding = string.Format("{0}{1}-{2}{3}-{5}-{4}.csv", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Hour,DateTime.Now.Minute, algorithm.Name, simulationNum);
CsvFileWriter.SaveToCsv(result.CompletnessOfReqs, string.Format("completnessReqs-{0}", resultFileEnding));
CsvFileWriter.SaveToCsv(result.CompletnessOfTemplates, string.Format("completnessTempl-{0}", resultFileEnding));
CsvFileWriter.SaveToCsv(result.PrecisionOfTemplates, string.Format("precisionTempl-{0}", resultFileEnding));



Console.ReadLine();



