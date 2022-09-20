import ReadInputFile
import pandas as pd



def getTemplatesToElicit(inputData):
    tempData = inputData.iloc[ :, 1:].astype('int64')
    tempData = tempData.where(tempData != '0', '1')

    templateRanking = pd.DataFrame(inputData['NORTID'])
    templateRanking['SUM'] = tempData.sum(axis=1)
    templateRanking.sort_values('SUM', inplace=True, ascending=False)

    return templateRanking

def getTempatesToSuggest(inputData, projectID):
    tempData = inputData.iloc[ :, 1:].astype('int64')
    tempData = tempData.where(tempData != '0', '1')
    tempData = tempData.drop([projectID])

    templateRanking = pd.DataFrame(inputData['NORTID'])
    templateRanking['SUM'] = tempData.sum(axis=1)
    templateRanking.sort_values('SUM', inplace=True, ascending=False)

    return templateRanking

    # Algorithm for eliciting by suggesting top most popular templates
    #print(templateRanking)

