from pathlib import Path
from typing import List

from src.domain import TrialData, Persistence, Direction, Table, ColumnFormat


def descriptive_table(trials: List[TrialData], file_name: Path, persistence: Persistence):
    # Counting for all five occurrences
    ad_right_shot_right = sum(
        1 for trial in trials if trial.advertisement_direction == Direction.RIGHT and trial.shot_location_x > 0
    )
    ad_right_shot_left = sum(
        1 for trial in trials if trial.advertisement_direction == Direction.RIGHT and trial.shot_location_x <= 0
    )
    ad_left_shot_right = sum(
        1 for trial in trials if trial.advertisement_direction == Direction.LEFT and trial.shot_location_x > 0
    )
    ad_left_shot_left = sum(
        1 for trial in trials if trial.advertisement_direction == Direction.LEFT and trial.shot_location_x <= 0
    )

    # Calculate totals
    total_right = ad_right_shot_right + ad_right_shot_left
    total_left = ad_left_shot_right + ad_left_shot_left

    # Data for the transposed table with percentages
    headers = ['Condition', 'Shot Right', 'Shot Left']
    rows = [
        [
            'Background Motion Right',
            f"{ad_right_shot_right} ({ad_right_shot_right / total_right * 100:.1f}%)",
            f"{ad_right_shot_left} ({ad_right_shot_left / total_right * 100:.1f}%)"
        ],
        [
            'Background Motion Left',
            f"{ad_left_shot_right} ({ad_left_shot_right / total_left * 100:.1f}%)",
            f"{ad_left_shot_left} ({ad_left_shot_left / total_left * 100:.1f}%)"
        ],
    ]

    # Create the table object
    table = Table("Shot distribution based on background motion direction", headers, rows)

    persistence.save_table(table, file_name, ColumnFormat.DOUBLE)
