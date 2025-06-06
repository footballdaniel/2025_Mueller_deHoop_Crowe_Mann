from pathlib import Path
from typing import List
from statistics import mean, stdev

from src.domain import TrialDataExperiment3And4, Persistence, Direction, Table, ColumnFormat


def descriptive_table(trials: List[TrialDataExperiment3And4], file_name: Path, persistence: Persistence):
    # Separate trials by advertisement direction
    right_ad_trials = [t.goalkeeper_position for t in trials if t.advertisement_direction == Direction.RIGHT]
    left_ad_trials = [t.goalkeeper_position for t in trials if t.advertisement_direction == Direction.LEFT]

    # Calculate mean and standard deviation for each condition
    mean_right = mean(right_ad_trials) if right_ad_trials else None
    sd_right = stdev(right_ad_trials) if len(right_ad_trials) > 1 else None

    mean_left = mean(left_ad_trials) if left_ad_trials else None
    sd_left = stdev(left_ad_trials) if len(left_ad_trials) > 1 else None

    # Format mean and SD values to two decimal places
    mean_right_str = f"{mean_right:.2f}" if mean_right is not None else "N/A"
    sd_right_str = f"{sd_right:.2f}" if sd_right is not None else "N/A"

    mean_left_str = f"{mean_left:.2f}" if mean_left is not None else "N/A"
    sd_left_str = f"{sd_left:.2f}" if sd_left is not None else "N/A"

    # Data for the transposed table
    headers = ['Condition', 'Mean [m]', 'SD [m]']
    rows = [
        ['Background Motion Right', mean_right_str, sd_right_str],
        ['Background Motion Left', mean_left_str, sd_left_str],
    ]

    # Create the table object
    table = Table("Perceived goalkeeper position based on background motion direction", headers, rows)
    persistence.save_table(table, file_name, ColumnFormat.DOUBLE)
