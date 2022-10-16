# Analysis penalty project
# How to use
# Place this script in a folder
# In the same folder, there has to be a folder called "data" that contains all json files from the experiment
# Use Ctrl + Enter to execute the code line by line

# Required packages -------------------------------------------------------
# These two lines need to be run only once!
# install.packages("rjson")
# install.packages("rstudioapi")
# install.packages("dplyr")

# Load workspace ----------------------------------------------------------
library(rjson)
library(rstudioapi)
library(sjPlot)
library(dplyr)

# Clear environment variables
rm(list = ls())
# Clear console commands
cat("\014")
# Set working directory to current script location
setwd(dirname(rstudioapi::getActiveDocumentContext()$path))
getwd()

# Reference to functions ---------------------------------------------------
source("src/read_data.R")
source("src/clean_data.R")

# Load data ---------------------------------------------------------------
folders_haider_joost <- list.dirs("../data/HaiderJoost/")[-1]
df_raw <- read_all_json(folders_haider_joost)

# Clean up data -----------------------------------------------------------
df <- clean_data(df_raw)

# Descriptive summary -----------------------------------------------------
names(df)

# Influence of ads:
df %>%
  group_by(advertisement) %>%
  summarize(
    n = n(),
    mean_direction = mean(end_x),
    sd_direction = sd(end_x)
  )

# Compare direction of shots based on goalkeeper position
# Not really any effect
df %>%
  group_by(goalkeeper_position) %>%
  summarize(
    n = n(),
    mean_direction = mean(end_x),
    sd_direction = sd(end_x)
  )


# SHOT DIRECTION ----------------------------------------------------------- 
# Run fixed effects model
m1.2 <- glm(
  direction ~ advertisement,
  data = df,
  family = binomial(link = "logit")
)
tab_model(m1.2)
plot_model(m1.2, show.values = TRUE, vline.color = "black")
plot_model(m1.2, type = "pred")

# Run multilevel on direction ----------------------------------------------
m2.2 <- lme4::glmer(
  direction ~ advertisement + (1 | participant_name),
  family = binomial,
  data = df
)
tab_model(m2.2)
plot_model(m2.2, show.values = TRUE, vline.color = "black")


# Per participant varying intercepts noise


# tab_model(m2.3, transform = NULL, show.se = TRUE, collapse.ci = TRUE, file = "plots/anniek_lars_shot_direction_table.doc")
# plot_model(m2.3, show.values = TRUE, vline.color = "black")
# plot_model(m2.3, type = "pred", show.values = TRUE, terms = c("goalkeeper_position", "advertisement"))
# ## Save plot
# ggsave("plots/anniek_lars_shot_direction_prediction.png", width = 8, height = 6)

# plot_model(m2.3, type = "res")

# # Comparing fixed level vs multilevel approach
# plot_models(
#   m1.3,
#   m2.3,
#   show.values = TRUE,
#   axis.labels = c("Interaction effect", "Goalkeeper position", "Ads moving right"),
#   m.labels = c("Fixed effect model", "Varying intercept model"),
#   vline.color = "black"
# )

# # Save plot
# ggsave("plots/anniek_lars_shot_direction_forestplot.png", width = 8, height = 5)



# SHOT PLACEMENT: ----------------------------------------------------------
# SUMMARY: No effects found for shot placement.

# # Ads only
# m3.2 <- lme4::glmer(
#   end_x ~ advertisement + (1 | participant_name),
#   family = gaussian,
#   data = df
# )
# tab_model(m3.2, transform = NULL, show.se = TRUE, collapse.ci = TRUE)

# # Interaction
# m3.3 <- lme4::lmer(
#   end_x ~ advertisement + goalkeeper_position + goalkeeper_position:advertisement + (1 | participant_name),
#   data = df
# )
# tab_model(m3.3, transform = NULL, show.se = TRUE, collapse.ci = TRUE)
# tab_model(m3.3, transform = NULL, show.se = TRUE, collapse.ci = TRUE, file = "plots/anniek_lars_shot_placement_table.doc")
# plot_model(m3.3, show.values = TRUE, vline.color = "black")
# plot_model(m3.3, type = "pred", show.values = TRUE, terms = c("goalkeeper_position", "advertisement"))
# plot_model(m3.3, type = "res")
