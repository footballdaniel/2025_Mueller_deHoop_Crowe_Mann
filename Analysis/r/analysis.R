# Analysis penalty project
# How to use
# Place this script in a folder
# In the same folder, there has to be a folder called "data" that contains all json files from the experiment
# Use Ctrl + Enter to execute the code line by line

# Required packages -------------------------------------------------------
# These two lines need to be run only once!
# install.packages("rjson")
# install.packages("rstudioapi")


# Load workspace ----------------------------------------------------------
library(rjson)
library(rstudioapi)
library(sjPlot)

# Clear environment variables
rm(list=ls())
# Clear console commands
cat("\014")

# Set working directory to current script location
setwd(dirname(rstudioapi::getActiveDocumentContext()$path))
getwd()

# Load data ---------------------------------------------------------------
participants_folders = list.dirs("../data/LarsAnniek/")[-1]

read_all_json <- function(participants_folders)
{
  df = data.frame()
  for (participant in participants_folders)
  {
    participant_name = basename(participant)
    search_pattern = paste(participant,"/*.json", sep="")
    print(search_pattern)
    file_names <- Sys.glob(search_pattern)
    print(file_names)
    file_numbers <- seq(file_names)
  
    for (file_number in file_numbers)
      {
        data <- fromJSON(file = file_names[file_number])
        participant_name = participant_name
        advertisement = data$AdvertisementDirection
        end_x = round(data$Events$End$EndLocation$X, 2)
        end_y = round(data$Events$End$EndLocation$Y, 2)
        goalkeeper_position = round(data$GoalkeeperDisplacement, 2)
        new_observations = cbind(participant_name, advertisement, goalkeeper_position, end_x, end_y)
        df <- rbind(df, new_observations)
      }
  
    df$end_x <- as.numeric(as.character(df$end_x))
    df$end_y <- as.numeric(as.character(df$end_y))
    df$goalkeeper_position <- as.numeric(as.character(df$goalkeeper_position))
  }
  return(df)
}
  
df_raw = read_all_json(participants_folders)


# Clean up data -----------------------------------------------------------
library(dplyr)
df = subset(df_raw, advertisement!="None")
df = df %>% mutate(
  direction = ifelse(end_x > 0, 1, 0),
  gk_distance_from_center = abs(goalkeeper_position)
)



# Looking at when the ads where going to the right only -------------------
df_ads_to_right = df[df$advertisement == "Right", ]
formula_m1 <- "end_x ~ goalkeeper_position + (1 | participant_name)"
m1 <- lme4::lmer(formula_m1, data=df_ads_to_right)
tab_model(m1)


# Run multilevel ----------------------------------------------------------
formula_m1 <- "end_x ~ goalkeeper_position + advertisement + goalkeeper_position:advertisement + (1 | participant_name)"
m1 <- lme4::lmer(formula_m1, data=df)

# Results
tab_model(m1)
summary(m1)
coef(summary(m1))
plot_model(m1)


# Run multilevel on Direction ---------------------------------------------
formula_m1 <- "direction ~ goalkeeper_position + advertisement + goalkeeper_position:advertisement + (1 | participant_name)"
m1 <- lme4::glmer(formula_m1, data=df, family = binomial(link = "logit"))

# Results
tab_model(m1)
summary(m1)
coef(summary(m1))
plot_model(m1)

# Predictions
plot_model(
  m1, 
  type = "pred",
  terms = c("advertisement", "goalkeeper_position")
)
