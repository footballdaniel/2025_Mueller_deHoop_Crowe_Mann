clean_data <- function(df) {
    df <- subset(df, advertisement != "None")
    df <- df %>% mutate(
        direction = ifelse(end_x > 0, 1, 0),
        gk_distance_from_center = abs(goalkeeper_position)
    )

    # Label the factors (for plotting)
    df$advertisement <- factor(df$advertisement, levels = c("Left", "Right"), labels = c("Advertisement moving left", "Advertisement moving right"))
    df$direction <- factor(df$direction, levels = c(0, 1), labels = c("Left", "Right"))

    return(df)
}
