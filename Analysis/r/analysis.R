# Analysis penalty project
# How to use
# Place this script in a folder
# In the same folder, there has to be a folder called "data" that contains all json files from the experiment
# Use Ctrl + Enter to execute the code line by line

# Required packages -------------------------------------------------------
# These two lines need to be run only once!
install.packages("rjson")
install.packages("rstudioapi")


# Load workspace ----------------------------------------------------------
library(rjson)
library(rstudioapi)

# Clear environment variables
rm(list=ls())
# Clear console commands
cat("\014")

# Set working directory to current script location
setwd(dirname(rstudioapi::getActiveDocumentContext()$path))
getwd()

# Load data ---------------------------------------------------------------
file_names <- Sys.glob("data/*.json")
file_numbers <- seq(file_names)

read_all_json <- function(filenames)
{ 
  df = data.frame()
  for (file_number in file_numbers) 
  {
    data <- fromJSON(file = file_names[file_number])
    participant_name = data$ParticipantName
    advertisement = data$AdvertisementDirection
    end_x = round(data$Events$End$EndLocation$X, 2)
    end_y = round(data$Events$End$EndLocation$Y, 2)
    goalkeeper_position = round(data$GoalkeeperDisplacement, 2)
    new_observations = cbind(participant_name, advertisement, end_x, end_y)
    df <- rbind(df, new_observations)
  }
  
  df$end_x <- as.numeric(as.character(df$end_x))
  df$end_y <- as.numeric(as.character(df$end_y))
  
  return(df)
}

# debug(read_all_json)
df <- read_all_json(fileNames)

# Setup variables
df$advertisement <- factor(df$advertisement)

# Visualizations ----------------------------------------------------------
hist(df$end_x, breaks = 20)
hist(df$end_x[df$participant_name=="David Mann"], breaks=20)

plot(
  df$end_x, df$end_y, 
  xlim = c(-5, 5), 
  ylim = c(0, 3),
  main= "Nice title",
  xlab = "Shot placement lateral [m]",
  ylab = "Shot height [m]",
  asp = 1
  
)
segments(-3.66, 0, -3.66, 2.7, lwd=10)
segments(3.66, 0, 3.66, 2.7, lwd=10)
segments(-3.66, 2.7, 3.66, 2.7, lwd=10)

# Stats ------------------------------------------------------------------
df_ttest <- df[df$advertisement == "Left" | df$advertisement == "Right",]
t.test(end_x ~ advertisement, data = df_ttest)

# Anova
results_anov <- aov(end_x ~ advertisement + goalkeeper_position + advertisement:goalkeeper_position, data = df_ttest)
summary(results_anov)

# Standardized data
df_z <- df_ttest
df_z$end_x <- (df$end_x - mean(df$end_x)) / sd(df$end_x)
df_z$end_y <- (df$end_y - mean(df$end_y)) / sd(df$end_y)
df_z$goalkeeper_position <- (df$goalkeeper_position - mean(df$goalkeeper_position)) / sd(df$goalkeeper_position)

results_regression <- lm(end_x ~ advertisement + goalkeeper_position + advertisement:goalkeeper_position, data = df_z)
summary(results_regression)



