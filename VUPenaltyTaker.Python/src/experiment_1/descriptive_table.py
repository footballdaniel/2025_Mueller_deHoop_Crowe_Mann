from pathlib import Path
from typing import List

from src.domain import TrialData, Persistence, Direction, Table, ColumnFormat


def descriptive_table_conditional_on_goalkeeper(trials: List[TrialData], file_name: Path,
                                                persistence: Persistence):
    # Counting based on ad direction and goalkeeper displacement
    ad_right_gk_right_shot_right = sum(
        1 for trial in trials if
        trial.advertisement_direction == Direction.RIGHT and trial.goalkeeper_displacement > 0 and trial.shot_location_x > 0
    )
    ad_right_gk_right_shot_left = sum(
        1 for trial in trials if
        trial.advertisement_direction == Direction.RIGHT and trial.goalkeeper_displacement > 0 >= trial.shot_location_x
    )
    ad_right_gk_left_shot_right = sum(
        1 for trial in trials if
        trial.advertisement_direction == Direction.RIGHT and trial.goalkeeper_displacement < 0 < trial.shot_location_x
    )
    ad_right_gk_left_shot_left = sum(
        1 for trial in trials if
        trial.advertisement_direction == Direction.RIGHT and trial.goalkeeper_displacement < 0 and trial.shot_location_x <= 0
    )
    ad_left_gk_right_shot_right = sum(
        1 for trial in trials if
        trial.advertisement_direction == Direction.LEFT and trial.goalkeeper_displacement > 0 and trial.shot_location_x > 0
    )
    ad_left_gk_right_shot_left = sum(
        1 for trial in trials if
        trial.advertisement_direction == Direction.LEFT and trial.goalkeeper_displacement > 0 >= trial.shot_location_x
    )
    ad_left_gk_left_shot_right = sum(
        1 for trial in trials if
        trial.advertisement_direction == Direction.LEFT and trial.goalkeeper_displacement < 0 < trial.shot_location_x
    )
    ad_left_gk_left_shot_left = sum(
        1 for trial in trials if
        trial.advertisement_direction == Direction.LEFT and trial.goalkeeper_displacement < 0 and trial.shot_location_x <= 0
    )

    # Calculate total filtered trials
    filtered_trials = sum(
        1 for trial in trials if 
        trial.advertisement_direction in [Direction.LEFT, Direction.RIGHT]
    )

    # Data for the table
    headers = ['Condition', 'Shot Right', 'Shot Left']
    rows = [
        [
            'Background Motion Right / GK Right',
            f'{ad_right_gk_right_shot_right} ({ad_right_gk_right_shot_right / filtered_trials * 100:.1f}%)',
            f'{ad_right_gk_right_shot_left} ({ad_right_gk_right_shot_left / filtered_trials * 100:.1f}%)'
        ],
        [
            'Background Motion Right / GK Left',
            f'{ad_right_gk_left_shot_right} ({ad_right_gk_left_shot_right / filtered_trials * 100:.1f}%)',
            f'{ad_right_gk_left_shot_left} ({ad_right_gk_left_shot_left / filtered_trials * 100:.1f}%)'
        ],
        [
            'Background Motion Left / GK Right',
            f'{ad_left_gk_right_shot_right} ({ad_left_gk_right_shot_right / filtered_trials * 100:.1f}%)',
            f'{ad_left_gk_right_shot_left} ({ad_left_gk_right_shot_left / filtered_trials * 100:.1f}%)'
        ],
        [
            'Background Motion Left / GK Left',
            f'{ad_left_gk_left_shot_right} ({ad_left_gk_left_shot_right / filtered_trials * 100:.1f}%)',
            f'{ad_left_gk_left_shot_left} ({ad_left_gk_left_shot_left / filtered_trials * 100:.1f}%)'
        ]
    ]

    # Create and save the table
    table = Table("Shot distribution based on background motion and Goalkeeper displacement", headers, rows)
    persistence.save_table(table, file_name, ColumnFormat.DOUBLE)


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
    control_shot_right = sum(
        1 for trial in trials if trial.advertisement_direction == Direction.NONE and trial.shot_location_x > 0
    )
    control_shot_left = sum(
        1 for trial in trials if trial.advertisement_direction == Direction.NONE and trial.shot_location_x <= 0
    )

    # Calculate total filtered trials
    filtered_trials = sum(
        1 for trial in trials if 
        trial.advertisement_direction in [Direction.LEFT, Direction.RIGHT]
    )

    # Data for the transposed table
    headers = ['Condition', 'Shot Right', 'Shot Left']
    rows = [
        [
            'Background Motion Right',
            f'{ad_right_shot_right} ({ad_right_shot_right / filtered_trials * 100:.1f}%)',
            f'{ad_right_shot_left} ({ad_right_shot_left / filtered_trials * 100:.1f}%)'
        ],
        [
            'Background Motion Left',
            f'{ad_left_shot_right} ({ad_left_shot_right / filtered_trials * 100:.1f}%)',
            f'{ad_left_shot_left} ({ad_left_shot_left / filtered_trials * 100:.1f}%)'
        ],
        # [
        #    'No Advertisement (Control)',
        #    f'{control_shot_right} ({control_shot_right / filtered_trials * 100:.1f}%)',
        #    f'{control_shot_left} ({control_shot_left / filtered_trials * 100:.1f}%)'
        # ]
    ]

    # Create the table object
    table = Table("Shot distribution based on background motion direction", headers, rows)

    persistence.save_table(table, file_name, ColumnFormat.DOUBLE)
