from itertools import groupby
import ReadInputFile
import MostTemplates
import ReqElicitator
import pandas as pd
import matplotlib.pyplot as plt


inputData = ReadInputFile.getInputData()
templatesToSuggest = MostTemplates.getTemplatesToElicit(inputData)

numReqsInProject = inputData.sum(axis=0)

tempData = inputData.iloc[ :, 1:].astype('int64')
numTemplsInProject = tempData.where(tempData != '0', '1').sum(axis=0)

numElicitedReqsForProject = []
numElicitedTemplatesForProject = [] #those that apply to the project

for x in range(len(inputData.columns)):
    numElicitedReqsForProject.append(0)
    numElicitedTemplatesForProject.append(0)
    

reqsCompletness = pd.DataFrame()
templatesCompletness = pd.DataFrame()
precision = pd.DataFrame()

reqsCompletness['ID'] = range(len(inputData.columns))
templatesCompletness['ID'] = range(len(inputData.columns))
precision['ID'] = range(len(inputData.columns))
for x in range(1, len(templatesToSuggest.index)+1):
    reqsCompletness[x] = 0
    templatesCompletness[x] = 0
    precision[x] = 0

print(templatesToSuggest.index)

for (projectID, columnData) in inputData.iloc[ :, 1:].iteritems():
    templatesToSuggest = MostTemplates.getTempatesToSuggest(inputData, projectID)
    numTemplSuggested = 1
    for template in templatesToSuggest.iloc[:,:1].iterrows():
        templateID = template[1]['NORTID']
        if (ReqElicitator.projectUsesTemplate(inputData, projectID, templateID)):
            numElicitedTemplatesForProject[projectID] = numElicitedTemplatesForProject[projectID] + 1
            numElicitedReqsForProject[projectID] = numElicitedReqsForProject[projectID] + ReqElicitator.getNumberOfReqsFromTemplate(inputData, projectID, templateID)
        reqsCompletness[numTemplSuggested].iloc[projectID] = numElicitedReqsForProject[projectID]/numReqsInProject[projectID]
        templatesCompletness[numTemplSuggested].iloc[projectID] = numElicitedTemplatesForProject[projectID] /numTemplsInProject[projectID]
        precision[numTemplSuggested].iloc[projectID] = numElicitedTemplatesForProject[projectID] / numTemplSuggested
        numTemplSuggested +=1 
    print(projectID)

reqsCompletness.to_excel("output\\reqsCompletness.xlsx", index=False)
templatesCompletness.to_excel("output\\templCompletness.xlsx", index=False)
precision.to_excel("output\\precision.xlsx", index=False)



