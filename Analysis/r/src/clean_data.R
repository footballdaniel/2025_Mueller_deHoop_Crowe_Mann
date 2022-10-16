clean_data <- function(df) {
    # Delete all trials where the advertisement was not visible
    df <- subset(df, advertisement != "None")
    
    # Feature engineering
    df <- df %>% mutate(
        direction = ifelse(end_x > 0, 1, 0),
        gk_distance_from_center = abs(goalkeeper_position)
    )

    # Creating factors for plotting
    df$advertisement <- factor(
        df$advertisement,
        levels = c("Left", "Right"),
        labels = c("Advertisement moving left", "Advertisement moving right")
    )
    
    df$direction <- factor(
        df$direction,
        levels = c(0, 1),
        labels = c("Left", "Right")
    )

    df$goalkeeper_position.f <- factor(
        df$goalkeeper_position,
        levels = c(-0.22, -0.11, -0.05, 0, 0.05, 0.11, 0.22),
        labels = c("22 cm Left", "11 cm Left", "5 cm Left", "Center", "5 cm Right", "11 cm Right", "22 cm Right")
    )

    return(df)
}
