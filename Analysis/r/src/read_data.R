read_all_json <- function(participants_folders) {
    # Iterating over all participant folder in a folder.
    # Reading the penalty data from Json
    # Return df with all data
    df <- data.frame()
    for (participant in participants_folders) {
        df <- ingest_json_files(participant, df)
    }
    return(df)
}

ingest_json_files <- function(participant_folder, df) {
    participant_name <- basename(participant_folder)
    search_pattern <- paste(participant_folder, "/*.json", sep = "")
    print(search_pattern)
    file_names <- Sys.glob(search_pattern)
    file_numbers <- seq(file_names)
    print(file_names)

    for (file_number in file_numbers) {
        df <- read_data(file_names[file_number], participant_name, df)
    }

    df$end_x <- as.numeric(as.character(df$end_x))
    df$end_y <- as.numeric(as.character(df$end_y))
    df$goalkeeper_position <- as.numeric(as.character(df$goalkeeper_position))
    return(df)
}

read_data <- function(file, participant_name, df) {
    data <- rjson::fromJSON(file = file)
    advertisement <- data$AdvertisementDirection
    end_x <- round(data$Events$End$EndLocation$X, 2)
    end_y <- round(data$Events$End$EndLocation$Y, 2)
    goalkeeper_position <- round(data$GoalkeeperDisplacement, 2)
    new_observations <- cbind(participant_name, advertisement, goalkeeper_position, end_x, end_y)
    df <- rbind(df, new_observations)
    return(df)
}
