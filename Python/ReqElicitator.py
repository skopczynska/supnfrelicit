import pandas as pd


def isTemplateUsed(inputData, projectID, templateID):
    if (inputData[[projectID, templateID]] > 0):
        return True
    else:
        return False

def projectUsesTemplate(inputData, projectID, templateID):
    for row in inputData.iloc[ : , : 1].iterrows():
        if (row[1]['NORTID'] == templateID):
            if (inputData[projectID].iloc[row[0]] > 0):
                return True
            else:
                return False

def getNumberOfReqsFromTemplate(inputData, projectID, templateID):
    for row in inputData.iloc[ : , : 1].iterrows():
        if (row[1]['NORTID'] == templateID):
            return inputData[projectID].iloc[row[0]]
    return 0            