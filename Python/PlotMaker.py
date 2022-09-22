from itertools import groupby
import ReadInputFile
import MostTemplates
import ReqElicitator
import pandas as pd
import matplotlib.pyplot as plt
import numpy as np


excel_data = pd.read_excel('output\\reqsCompletness.xlsx')
# Read the values of the file in the dataframe
data = pd.DataFrame(excel_data)

myFig = plt.figure()
reqsCompletnessBar = data.drop(axis=0, labels=0).iloc[ :, 1:].boxplot(showfliers=False)
plt.xticks(np.arange(1,693,50))
plt.show()
myFig.savefig("reqCompletness.png", format = "png")