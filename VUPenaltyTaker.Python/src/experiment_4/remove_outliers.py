from pathlib import Path
from typing import List

from src.domain import TrialDataExperiment3And4, Persistence


def remove_outliers(exp_4_data: List[TrialDataExperiment3And4], file_name: Path, persistence: Persistence):
    goal_width = 7.32
    half_goal_width = goal_width / 2
    outlier_trials = [t for t in exp_4_data if
                      t.goalkeeper_position < -half_goal_width or t.goalkeeper_position > half_goal_width]

    persistence.save_outliers(outlier_trials, file_name)

    for t in outlier_trials:
        exp_4_data.remove(t)
