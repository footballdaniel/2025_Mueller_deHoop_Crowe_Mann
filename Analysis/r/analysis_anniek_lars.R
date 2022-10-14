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

# Load data ---------------------------------------------------------------
folders_lars_anniek <- list.dirs("../data/LarsAnniek/")[-1]
folders_haider_joost <- list.dirs("../data/HaiderJoost/")[-1]

read_all_json <- function(participants_folders) {
  df <- data.frame()
  for (participant in participants_folders)
  {
    participant_name <- basename(participant)
    search_pattern <- paste(participant, "/*.json", sep = "")
    print(search_pattern)
    file_names <- Sys.glob(search_pattern)
    print(file_names)
    file_numbers <- seq(file_names)

    for (file_number in file_numbers)
    {
      data <- rjson::fromJSON(file = file_names[file_number])
      participant_name <- participant_name
      advertisement <- data$AdvertisementDirection
      end_x <- round(data$Events$End$EndLocation$X, 2)
      end_y <- round(data$Events$End$EndLocation$Y, 2)
      goalkeeper_position <- round(data$GoalkeeperDisplacement, 2)
      new_observations <- cbind(participant_name, advertisement, goalkeeper_position, end_x, end_y)
      df <- rbind(df, new_observations)
    }

    df$end_x <- as.numeric(as.character(df$end_x))
    df$end_y <- as.numeric(as.character(df$end_y))
    df$goalkeeper_position <- as.numeric(as.character(df$goalkeeper_position))
  }
  return(df)
}

df_raw <- read_all_json(folders_lars_anniek)
df_raw_haider_joost <- read_all_json(folders_haider_joost)

print("CHOOOSE DATASET HERE!")
cat("WTF, haider and joost do have an ads effect?!")
df_raw <- df_raw_haider_joost

# Clean up data -----------------------------------------------------------
df <- subset(df_raw, advertisement != "None")
df <- df %>% mutate(
  direction = ifelse(end_x > 0, 1, 0),
  gk_distance_from_center = abs(goalkeeper_position)
)

# Label the factors (for plotting)
df$advertisement <- factor(df$advertisement, levels = c("Left", "Right"), labels = c("Advertisement moving left", "Advertisement moving right"))
df$direction <- factor(df$direction, levels = c(0, 1), labels = c("Left", "Right"))

# Descriptive summary -----------------------------------------------------
names(df)

# Influence of ads:
# When the ads go to the right, the mean distribution shifts ever so slightly right.
# However, SD is huge
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
# Run fixed effects model --------------------------------------------------
# Model 1: Direction of shot based on goalkeeper position
# When gk position alone, a small significant effect
# When gk is on the right, people shoot more to the left. However, people generally shoot to the right.
m1.1 <- glm(
  direction ~ goalkeeper_position,
  family = binomial(link = "logit"),
  data = df
)
tab_model(m1.1)
plot_model(m1.1, show.values = TRUE, vline.color = "black")
plot_model(m1.1, type = "pred")

# No effect
m1.2 <- glm(
  direction ~ advertisement,
  data = df,
  family = binomial(link = "logit")
)
tab_model(m1.2)
plot_model(m1.2, show.values = TRUE, vline.color = "black")
plot_model(m1.2, type = "pred")

# Ads and goalkeeper
# Non-significant interaction effect. when ads go to the right, the effect of gk is bigger.
m1.3 <- glm(
  direction ~ advertisement + goalkeeper_position + goalkeeper_position:advertisement,
  family = binomial(link = "logit"),
  data = df
)

tab_model(m1.3)
plot_model(m1.3, show.values = TRUE, vline.color = "black")
plot_model(m1.3, type = "pred", show.values = TRUE, terms = c("goalkeeper_position", "advertisement"), )
plot_model(m1.3, type = "res")

# Run multilevel on direction ----------------------------------------------
# Goalkeeper only. Stronger effect now!
m2.1 <- lme4::glmer(
  direction ~ goalkeeper_position + (1 | participant_name),
  family = binomial,
  data = df
)

# Tab model with log odds
tab_model(m2.1)
plot_model(m2.1, show.values = TRUE, vline.color = "black")

# Compare with 1.1
# Same effect as fixed model
plot_models(
  m1.1,
  m2.1,
  show.values = TRUE,
  m.labels = c("Fixed effect model", "Varying intercept model"),
  axis.labels = c("Goalkeeper position"),
  vline.color = "black"
)

# Ads only
m2.2 <- lme4::glmer(
  direction ~ advertisement + (1 | participant_name),
  family = binomial,
  data = df
)
tab_model(m2.2)
plot_model(m2.2, show.values = TRUE, vline.color = "black")

# Interaction
m2.3 <- lme4::lmer(
  direction ~ advertisement + goalkeeper_position + goalkeeper_position:advertisement + (1 | participant_name),
  family = binomial,
  data = df
)


tab_model(m2.3, transform = NULL, show.se = TRUE, collapse.ci = TRUE, file = "plots/anniek_lars_shot_direction_table.doc")
plot_model(m2.3, show.values = TRUE, vline.color = "black")
plot_model(m2.3, type = "pred", show.values = TRUE, terms = c("goalkeeper_position", "advertisement"))
## Save plot
ggsave("plots/anniek_lars_shot_direction_prediction.png", width = 8, height = 6)

plot_model(m2.3, type = "res")

# Comparing fixed level vs multilevel approach
plot_models(
  m1.3,
  m2.3,
  show.values = TRUE,
  axis.labels = c("Interaction effect", "Goalkeeper position", "Ads moving right"),
  m.labels = c("Fixed effect model", "Varying intercept model"),
  vline.color = "black"
)

# Save plot
ggsave("plots/anniek_lars_shot_direction_forestplot.png", width = 8, height = 5)



# SHOT PLACEMENT: ----------------------------------------------------------
# SUMMARY: No effects found for shot placement.

# Ads only
m3.2 <- lme4::glmer(
  end_x ~ advertisement + (1 | participant_name),
  family = gaussian,
  data = df
)
tab_model(m3.2, transform = NULL, show.se = TRUE, collapse.ci = TRUE)

# Interaction
m3.3 <- lme4::lmer(
  end_x ~ advertisement + goalkeeper_position + goalkeeper_position:advertisement + (1 | participant_name),
  data = df
)
tab_model(m3.3, transform = NULL, show.se = TRUE, collapse.ci = TRUE)
tab_model(m3.3, transform = NULL, show.se = TRUE, collapse.ci = TRUE, file = "plots/anniek_lars_shot_placement_table.doc")
plot_model(m3.3, show.values = TRUE, vline.color = "black")
plot_model(m3.3, type = "pred", show.values = TRUE, terms = c("goalkeeper_position", "advertisement"))
plot_model(m3.3, type = "res")
