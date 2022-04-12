# Analysis penalty project


# Required packages -------------------------------------------------------
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

# Stats ------------------------------------------------------------------
df_ttest <- df[df$advertisement == "Left" | df$advertisement == "Right",]
t.test(end_x ~ advertisement, data = df_ttest)

# Anova
results_anov <- aov(end_x ~ advertisement, data = df_ttest)
summary(results_anov)

# Visualizations ----------------------------------------------------------
hist(df$end_x, breaks = 20)
hist(df$end_x[df$participant_name=="David Mann"], breaks=20)

plot(
  df$end_x, df$end_y, 
  xlim = c(-5, 5), 
  ylim = c(0, 3),
  main= "Nice title",
  xlab = "Shot placement lateral [m]",
  ylab = "Shot height [m]"
  
)
segments(-3.66, 0, -3.66, 2.7, lwd=10)
segments(3.66, 0, 3.66, 2.7, lwd=10)
segments(-3.66, 2.7, 3.66, 2.7, lwd=10)




