library(psych)
library(Lahman)
require(plyr)
library(dplyr)
library(MASS)
library(effsize)
library(reshape2)
library(ggplot2)
library(Rmisc)
library(tidyr)
digPrec<-4 

fileName <-"complentess"
dataMT <-read.csv("..\\output\\completnessReqs-310-2345-1-MostTemplates.csv", sep=";", header=TRUE, encoding = "UTF-8", dec=",")
dataRandom <-read.csv("..\\output\\completnessReqsPerBucket-1110-1859-1-Random.csv", sep=";", header=TRUE, encoding = "UTF-8", dec=",")




prepareData <- function(inputData){
  preparedData <- inputData[-c(1)]
  preparedDataT <- as.data.frame(t(preparedData))
  rownames(preparedDataT) <-colnames(preparedData)
  colnames(preparedDataT) <- rownames(preparedData)
  
  preparedData_long_df <- gather(preparedDataT)
  colnames(preparedData_long_df) <- c('Group', 'Value')
  preparedData_long_df <- transform(preparedData_long_df, Group=as.numeric(Group))
  
  return(preparedData_long_df)
}

dataMT <- prepareData(dataMT)
dataRandom <- prepareData(dataRandom)


createPlot <- function(dataToPlot, xlab, ylab, title) {
  p <- ggplot(data = dataToPlot) # +  aes(x=ID, y=data[,values]))
  p <- p + geom_boxplot(aes(x=Group, y=Value, group=Group), outlier.shape = NA, color='grey', fill='white')
  p <- p + xlab(xlab) + ylab(ylab) + ggtitle(title)
  return(p)
}

plots <- list(createPlot(dataMT, "NumberOfTemplates","Completeness","Most Templates"),
          createPlot(dataRandom, "NumberOfTemplates","Completeness","Random"))


pdf(paste("..\\output\\", fileName, ".pdf", sep=''), width = 30, height=20)
multiplot(plotlist = plots, cols=2)
dev.off()

