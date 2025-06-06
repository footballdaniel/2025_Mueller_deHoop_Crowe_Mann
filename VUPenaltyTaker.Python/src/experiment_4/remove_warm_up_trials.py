from pathlib import Path
from typing import List

from src.domain import TrialDataExperiment3And4, Persistence


def remove_warm_up_trials(exp_4_data: List[TrialDataExperiment3And4]):
    warmup_trials = [t for t in exp_4_data if t.trial <= 5]
    for t in warmup_trials:
        exp_4_data.remove(t)
