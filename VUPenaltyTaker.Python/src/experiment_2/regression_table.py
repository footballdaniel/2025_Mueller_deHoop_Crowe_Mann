from pathlib import Path

import arviz as az
import numpy as np
import pandas as pd

from src.domain import Persistence, Table
from src.utils import probability_effect_is_zero


def model_1_table(inference_data: Path, file_name: Path, persistence: Persistence):
    results_1 = az.from_netcdf(str(inference_data))
    summary_predictors = az.summary(
        results_1,
        hdi_prob=0.95,
        stat_funcs={"Zero effect probability": probability_effect_is_zero},
        var_names=["~^p$|participant"],
        filter_vars="regex"
    )

    summary_varying_intercepts_sigma = az.summary(
        results_1,
        hdi_prob=0.95,
        stat_funcs={"Zero effect probability": probability_effect_is_zero},
        var_names=["sigma"],
        filter_vars="like"
    )

    # combine both summaries
    summary = pd.concat([summary_predictors, summary_varying_intercepts_sigma], axis=0)  # for stacking rows

    # Extract relevant columns from the summary
    summary_df = summary[["mean", "sd", "hdi_2.5%", "hdi_97.5%", "ess_bulk", "Zero effect probability"]].copy()

    # Round all numeric values to two decimals
    summary_df = summary_df.round(2)

    summary_df["ess_bulk"] = summary_df["ess_bulk"].astype(int)

    # Add custom columns: Odds Ratios and Confidence Intervals (CI)
    summary_df["Odds Ratios"] = summary_df.apply(
        lambda row: f"{np.exp(row['mean']):.2f}", axis=1
    )

    summary_df["CI"] = summary_df.apply(
        lambda row: f"{row['hdi_2.5%']:.2f} – {row['hdi_97.5%']:.2f}", axis=1
    )

    # Insert the variable names into the first column
    summary_df.insert(0, 'Predictors', summary.index)

    # Construct the final table rows by converting the values to strings with the necessary formatting
    rows = summary_df[["Predictors", "mean", "CI", "Odds Ratios", "ess_bulk", "Zero effect probability"]].astype(
        str).values.tolist()

    # Define table headers
    header = ["Predictors", "Estimates", "CI (2.5%, 97.5%)", "Odds Ratios", "ESS", "Probability of null effect"]

    table = Table(
        title="Hierarchical logistic regression with interaction term",
        header=header,
        rows=rows
    )

    table.rename_element("0.0", "<0.01")
    table.rename_element("Intercept", "Participant-specific intercept αj")
    table.rename_element("goalkeeper_movement_initiation", "Goalkeeper movement initiation")
    table.rename_element("advertisement_direction[Right]", "Background Motion[Right]")
    table.rename_element("goalkeeper_movement_initiation:advertisement_direction[Right]", "Goalkeeper movement initiation × Background Motion[Right]")
    table.rename_element("1|participant_sigma", "σ Participant")

    persistence.save_table(table, file_name)
