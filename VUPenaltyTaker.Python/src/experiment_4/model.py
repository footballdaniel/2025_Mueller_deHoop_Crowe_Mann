import os
from pathlib import Path
from typing import List

import arviz as az
import bambi as bmb
import pandas as pd

from src.domain import TrialDataExperiment3And4, Persistence


def model_1(trials: List[TrialDataExperiment3And4], inference_data_path: Path, file_name: Path,
            persistence: Persistence) -> None:
    df = pd.DataFrame({
        "advertisement_direction": [shot.advertisement_direction.value for shot in trials],
        "participant": [t.participant for t in trials],
        "goalkeeper_position": [t.goalkeeper_position for t in trials],
        "condition": [t.condition.value for t in trials],
    })
    df["advertisement_direction"] = pd.Categorical(
        df["advertisement_direction"],
        categories=["Left", "Right"],
        ordered=True)
    df["condition"] = pd.Categorical(
        df["condition"],
        categories=["AdvertisementOnly", "AdvertisementAndGoal"],
        ordered=True)
    df["participant"] = pd.Categorical(df["participant"])
    flat_prior = bmb.Prior("Uniform", lower=-3, upper=3)
    normal_distribution = bmb.Prior("Normal", mu=0, sigma=2.5)
    half_normal_distribution = bmb.Prior("HalfNormal", sigma=2.5)
    priors = {
        "Intercept": normal_distribution,
        "advertisement_direction": flat_prior,
        "sigma": half_normal_distribution,
        "advertisement_direction:condition": flat_prior,
    }
    formula = "goalkeeper_position ~ advertisement_direction + advertisement_direction:condition + (1|participant)"
    model_1 = bmb.Model(formula, df, family="gaussian", priors=priors)
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
