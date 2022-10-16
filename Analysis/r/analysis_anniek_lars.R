# Analysis penalty project
# How to use
# Place this script in a folder
# In the same folder, there has to be a folder called "data" that contains all json files from the experiment
# Use Ctrl + Enter to execute the code line by line

# Load workspace ----------------------------------------------------------
library(rjson)
library(rstudioapi)
library(sjPlot)
library(dplyr)
library(ggplot2)
library(glmmTMB)
library(lme4)
library(ggridges)

# Clear environment variables
rm(list = ls())
# Clear console commands
cat("\014")
# Set working directory to current script location
setwd(dirname(rstudioapi::getActiveDocumentContext()$path))
getwd()

# Set theme for plotting
set_theme(
  base = theme_bw(),
  axis.linecolor = "black", # Black axis lines
  legend.background = element_rect(fill = "white", colour = "black")
)

# Reference to functions ---------------------------------------------------
source("src/read_data.R")
source("src/clean_data.R")

# Load data ---------------------------------------------------------------
df_raw <- read_all_json("../data/LarsAnniek/")
df <- clean_data(df_raw)

df$goalkeeper_position.f <- factor(
  df$goalkeeper_position,
  levels = c(-0.22, -0.11, -0.05, 0, 0.05, 0.11, 0.22),
  labels = c("22 cm Left", "11 cm Left", "5 cm Left", "Center", "5 cm Right", "11 cm Right", "22 cm Right")
)

# Descriptive summary -----------------------------------------------------
names(df)

# Barplot for shot direction based on ads
ggplot(df, aes(x = direction, fill = advertisement)) +
  geom_bar(position = "dodge") +
  xlab("Shot direction") +
  ylab("Number of shots") +
  scale_fill_manual(values = c("black", "white")) +
  theme(legend.background = element_rect(fill = "white", color = "black")) +
  labs(title = "Study 2: Small advertisements")
ggsave("plots/anniek_lars_direction_ads.jpg", width = 6, height = 4, dpi = 300)

# barplot for each goalkeeper position
ggplot(df, aes(x = goalkeeper_position.f, fill = direction)) +
  geom_bar(position = "dodge") +
  xlab("Goalkeeper placement") +
  ylab("Number of shots") +
  scale_fill_manual(values = c("black", "white")) +
  theme(legend.background = element_rect(fill = "white", color = "black")) +
  labs(title = "Study 2: Small advertisements")
ggsave("plots/anniek_lars_direction_goalkeeper.jpg", width = 10, height = 4, dpi = 300)


 # plot of end_x based on each level of goalkeeper
ggplot(df, aes(x = end_x, y = goalkeeper_position.f)) +
  geom_density_ridges() +
  xlab("Shot distribution [m]") +
  geom_text(aes(label = "Center of goal"), x = 0, y = 0.6, size = 3) +
  geom_vline(aes(xintercept = -3.6), df) +
  geom_vline(aes(xintercept = 3.6), df) +
  xlim(-6, 6) +
  ylab("Goalkeeper placement")
ggsave("plots/anniek_lars_direction_goalkeeper_continuous.jpg", width = 6, height = 4, dpi = 300)


# SHOT DIRECTION ----------------------------------------------------------- 
# Run fixed effects model --------------------------------------------------
# Gk only
m1.1 <- glm(
  direction ~ goalkeeper_position,
  family = binomial(link = "logit"),
  data = df
)
tab_model(m1.1)
plot_model(m1.1, show.values = TRUE, vline.color = "black")
plot_model(m1.1, type = "pred")

# Adds only
m1.2 <- glm(
  direction ~ advertisement,
  data = df,
  family = binomial(link = "logit")
)
tab_model(m1.2)
plot_model(
  m1.2,
  show.values = TRUE,
  vline.color = "black",
  colors = "bw"
)
plot_model(m1.2, type = "pred")

# Full model with interaction
# Non-significant interaction effect. when ads go to the right, the effect of gk is bigger.
m1.3 <- glm(
  direction ~ advertisement + goalkeeper_position + goalkeeper_position:advertisement,
  family = binomial(link = "logit"),
  data = df
)

tab_model(m1.3)
plot_model(
  m1.3,
  show.values = TRUE,
  vline.color = "black",
  colors = "bw")
plot_model(m1.3, type = "pred", show.values = TRUE, terms = c("goalkeeper_position", "advertisement"), )
plot_model(m1.3, type = "res", colors = "bw")

# Run multilevel on direction ----------------------------------------------
# Goalkeeper only. Stronger effect now!
m2.1 <- lme4::glmer(
  direction ~ goalkeeper_position + (1 | participant_name),
  family = binomial,
  data = df
)
tab_model(m2.1)
plot_model(
  m2.1,
  show.values = TRUE,
  vline.color = "black",
  colors = "bw")

# Ads only
m2.2 <- lme4::glmer(
  direction ~ advertisement + (1 | participant_name),
  family = binomial,
  data = df
)
tab_model(m2.2)
plot_model(
  m2.2,
  show.values = TRUE,
  vline.color = "black",
  colors = "bw"
)

# Interaction
m2.3 <- lme4::glmer(
  direction ~ advertisement + goalkeeper_position + goalkeeper_position:advertisement + (1 | participant_name),
  family = binomial,
  data = df
)
plot_model(
  m2.3, 
  show.values = TRUE, 
  vline.color = "black",
  colors = "bw")
ggsave("plots/anniek_lars_shot_direction_prediction.png", width = 8, height = 6)

plot_model(m2.3, type = "res", colors = "bw")
sjPlot::plot_model(
  m2.3,
  type = "pred",
  terms = c("goalkeeper_position", "advertisement"))

# Compare effects
tab_model(
  m1.3, m2.3,
  pred.labels = c("Intercept", "Advertisement direction", "Goalkeeper position", "Interaction"),
  dv.labels = c("Regression", "Multilevel regression"),
  transform = NULL,
  show.se = TRUE
)
tab_model(
  m1.3, m2.3,
  pred.labels = c("Intercept", "Advertisement direction", "Goalkeeper position", "Interaction"),
  dv.labels = c("Regression", "Multilevel regression"),
  transform = NULL,
  show.se = TRUE,
  file = "plots/anniek_lars_direction_table.doc"
)


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

