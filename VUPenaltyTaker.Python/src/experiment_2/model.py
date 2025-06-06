import os
from pathlib import Path
from typing import List

import arviz as az
import bambi as bmb
import pandas as pd

from src.domain import Direction, TrialData, Persistence


def model_1(data: List[TrialData], inference_data_path: Path, file_name: Path, persistence: Persistence) -> None:
    # Create a set of unique participant names
    filtered_data = [p for p in data if p.advertisement_direction in {Direction.LEFT, Direction.RIGHT}]
    df = pd.DataFrame(
        {
            "advertisement_direction": [shot.advertisement_direction.value for shot in filtered_data],
            "participant": [p.participant for p in filtered_data],
            "shot_location": [p.shot_location_x for p in filtered_data],
            "goalkeeper_movement_initiation": [p.goalkeeper_movement_initiation for p in filtered_data],
            "shot_direction": [p.shot_direction.value for p in filtered_data],
        }
    )
    # Convert to categorical
    df["advertisement_direction"] = pd.Categorical(df["advertisement_direction"])
    df["shot_direction"] = pd.Categorical(df["shot_direction"])
    df["participant"] = pd.Categorical(df["participant"])

    flat_prior = bmb.Prior("Uniform", lower=-3, upper=3)
    priors = {
        "goalkeeper_movement_initiation": flat_prior,
        "advertisement_direction": flat_prior,
        "goalkeeper_movement_initiation:advertisement_direction": flat_prior,
    }

    formula = "shot_direction['Right'] ~ goalkeeper_movement_initiation * advertisement_direction + (1|participant)"
    model_1 = bmb.Model(
        formula,
        df,
        family="bernoulli",
        priors=priors,
    )

    persistence.save_model(model_1.__str__(), file_name)

    if not os.path.exists(str(inference_data_path)):
        results_1 = model_1.fit(
            draws=5000,
            chains=4,
            idata_kwargs={"log_likelihood": True},
            random_seed=1991
        )
        model_1.predict(results_1, kind="response")
        az.to_netcdf(results_1, str(inference_data_path))
