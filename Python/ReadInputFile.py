# Import pandas
import pandas as pd


def getInputData():
    # Load the xlsx file
    excel_data = pd.read_excel('..\\input\\freqmatrix.xlsx')
    # Read the values of the file in the dataframe
    data = pd.DataFrame(excel_data)
    # Print the content
    #print("The content of the file is:\n", data)
    return data

