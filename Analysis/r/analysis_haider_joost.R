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
df_raw <- read_all_json("../data/HaiderJoost/")
df <- clean_data(df_raw)

# Descriptive summary -----------------------------------------------------
names(df)

# Barplot for shot direction based on ads
ggplot(df, aes(x = direction, fill = advertisement)) +
  geom_bar(position = "dodge") +
  xlab("Shot direction") +
  ylab("Number of shots") +
  scale_fill_manual(values = c("black", "white")) +
  theme(legend.background = element_rect(fill = "white", color = "black")) +
  labs(title = "Study 1: Big advertisements, no GK movement")
ggsave("plots/haider_joost_direction_barplot.jpg", width = 6, height = 4, dpi = 300)


# Ads influencing shot direction (regression) --------------------------------
# Run fixed effects model
m1.2 <- glm(
  direction ~ advertisement,
  data = df,
  family = binomial(link = "logit")
)
tab_model(m1.2)
plot_model(m1.2, show.values = TRUE, vline.color = "black")
plot_model(
  m1.2, 
  type = "pred",
  title = "Study 1: Big advertisements, no GK movement \n
  Fixed effect model shows a significant add effect")


# Ads influencing shot direction (multilevel regression) --------------------
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
  title = "Study 1: Big advertisements, no GK movement"
)

# Compare effects
tab_model(
  m1.2, m2.2,
  pred.labels = c("Intercept", "Advertisement direction"),
  dv.labels = c("Regression", "Multilevel regression"),
  transform = NULL,
  show.se = TRUE
)

# Save
tab_model(
  m1.2, m2.2,
  pred.labels = c("Intercept", "Advertisement direction"),
  dv.labels = c("Regression", "Multilevel regression"),
  transform = NULL,
  show.se = TRUE,
  file = "plots/haider_joost_direction_table.doc"
)

plot_model(
  m1.2,
  show.values = TRUE,
  vline.color = "black",
  axis.labels = c("Advertisement direction"),
  title = "Study 1: Big advertisements, no GK movement",
  m.labels = c("Fixed effect model", "Varying intercept model")
)
ggsave("plots/haider_joost_direction_forestplot.jpg", width = 7, height = 3, dpi = 300)

## Plot biases
lme4::ranef(m2.2)
print(as.data.frame(ranef(m2.2)))
dotplot.ranef.mer(ranef(m2.2))

# Forest plot for random effects
plot_model(
  m2.2,
  type = "re",
  transform = NULL,
)

# Modeled bias per participant
plot_model(
  m2.2,
  type = "re",
  colors = "bw",
  title = "Shot bias per participant",
  transform = NULL) +
  xlab("Participants") +
  ylab("Bias (Odds ratios)")
ggsave("plots/haider_joost_direction_random_intercepts.jpg", width = 6, height = 8, dpi = 300)


# Distribution per participant
ggplot(df, aes(x = end_x, y = participant_name)) +
  geom_density_ridges() +
  xlim(-4, 4) +
  xlab("Shot distribution [m]") +
  ylab("Participants") +
  labs(title = "Study 1: Shot distribution")
  ggsave("plots/haider_joost_direction_facet_density.jpg", width = 6, height = 8, dpi = 300)
