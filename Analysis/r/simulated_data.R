
# Simulated data for multilevel analysis ----------------------------------

participants <- 5
trials_per_participant <- 100
half_trials <- trials_per_participant / 2

# Simulate conditions
df <- data.frame()
for (i in 1:participants) {
  df <- rbind(df, data.frame(
    participant = i,
    trial = 1:trials_per_participant,
    ADDIRECTION = rep(c("ADLEFT", "ADRIGHT"), each = half_trials)
  ))
}


# Participant 1
# Completely influenced by shots
df[df$participant == 1, "x"] <- rnorm(trials_per_participant, 3, 1)
df[df$participant == 1 & df$ADDIRECTION == "ADLEFT", "x"] <- df[df$participant == 1 & df$ADDIRECTION == "ADLEFT", "x"] - 6

# Participant 2
# Random shots, but when ADRIGHT, the shot goes more to the right and vice versa
df[df$participant == 2, "x"] <- rnorm(trials_per_participant, 0, 1)
# Some additional influence of add
df[df$participant == 2 & df$ADDIRECTION == "ADRIGHT", "x"] <- df[df$participant == 2 & df$ADDIRECTION == "ADRIGHT", "x"] + rnorm(1, 2, 1)
df[df$participant == 2 & df$ADDIRECTION == "ADLEFT", "x"] <- df[df$participant == 2 & df$ADDIRECTION == "ADLEFT", "x"] - rnorm(1, 2, 1)



# Participant 3
# Always in the corner, but unaffected by the ADDIRECTION
df[df$participant == 3, "x"] <- rnorm(trials_per_participant, 3, 1)
# Every second goes to the left
df[df$participant == 3, "x"] <- ifelse(df[df$participant == 3, "trial"] %% 2 == 0, rnorm(half_trials, -3, 1), df[df$participant == 3, "x"])


# Participant 4
# Always shooting to corner
df[df$participant == 4, "x"] <- rnorm(trials_per_participant, 3, 1)

# Participant 5
# Always shooting to corner
df[df$participant == 5, "x"] <- rnorm(trials_per_participant, -3, 1)


# Height of the shots is just random
df$y <- runif(nrow(df), 0, 3)

# PLOTTING
library(ggplot2)
# histogram per participant

# histogram of all participants
ggplot(df, aes(x = x)) + 
  geom_histogram(bins = 20) +
  coord_fixed(ratio = 1) +
  geom_segment(x = -3.65, y = 0, xend = -3.65, yend = 2.7, color = "red", size = 1) +
  geom_segment(x = 3.65, y = 0, xend = 3.65, yend = 2.7, color = "red", size = 1) +
  geom_segment(x = -3.65, y = 2.7, xend = 3.65, yend = 2.7, color = "red", size = 1) 

# Title
ggplot(df, aes(x = x)) +
  coord_fixed(ratio = 1) +
  geom_histogram() +
  facet_wrap(~participant, ncol = participants) + 
  geom_segment(x = -3.65, y = 0, xend = -3.65, yend = 2.7, color = "red", size = 1) +
  geom_segment(x = 3.65, y = 0, xend = 3.65, yend = 2.7, color = "red", size = 1) +
  geom_segment(x = -3.65, y = 2.7, xend = 3.65, yend = 2.7, color = "red", size = 1) +
  ggtitle("Simulated data with bias for Particpant 4 and 5")
ggsave("plots/simulation_participant_level_bias.png", width = 10, height = 10)

# Scatterplot left and right shots with color for condititions
ggplot(df, aes(x = x, y = y, color = ADDIRECTION)) +
  coord_fixed(ratio = 1) +
  geom_point() +
  facet_wrap(~participant, nrow = participants) +
  geom_segment(x = -3.65, y = 0, xend = -3.65, yend = 2.7, color = "red", size = 1) +
  geom_segment(x = 3.65, y = 0, xend = 3.65, yend = 2.7, color = "red", size = 1) +
  geom_segment(x = -3.65, y = 2.7, xend = 3.65, yend = 2.7, color = "red", size = 1) +
  scale_color_manual(values = c("ADLEFT" = "blue", "ADRIGHT" = "red"), labels = c("Ads moving left", "Ads moving right")) +
  ggtitle("Simulated data influenced by participant 4 and 5")
# Save figure
ggsave("plots/simulation_participant_level.png", width = 10, height = 10)


library(lme4)
library(lmerTest)
library(sjPlot)
# Run regression to see if there is a difference between conditions
m1 <- lm(x ~ ADDIRECTION, data = df)
summary(m1)
tab_model(m1)
plot_model(m1, type = "pred", terms = "ADDIRECTION", show.values = TRUE, show.ci = TRUE, show.legend = TRUE)
plot_model(m1, type = "res", terms = "ADDIRECTION", show.values = TRUE, show.ci = TRUE, show.legend = TRUE)

