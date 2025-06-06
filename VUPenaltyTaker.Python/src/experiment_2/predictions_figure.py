from pathlib import Path

import arviz as az
import numpy as np
from matplotlib import pyplot as plt
from matplotlib.ticker import FuncFormatter

from src.domain import ColumnFormat
from src.infra.persistence import ApaStyledPersistence


def prediction_figure(inference_data: Path, file_name: Path, persistence: ApaStyledPersistence):
    results_1 = az.from_netcdf(str(inference_data))

    """PREDICT AVERAGE"""
    number_samples = 500
    x_lims = (1, 0)  # Keep original range for predictions (from 1 to 0)
    goalkeeper_movement_initiation_range = np.linspace(x_lims[0], x_lims[1], 100)
    advertisement_vals = [0, 1]

    posterior_samples = results_1.posterior
    intercept_samples = posterior_samples['Intercept'].values.flatten()[0:number_samples]
    gd_samples = posterior_samples['goalkeeper_movement_initiation'].values.flatten()[0:number_samples]
    ad_samples = posterior_samples['advertisement_direction'].values.flatten()[0:number_samples]
    interaction_samples = posterior_samples['goalkeeper_movement_initiation:advertisement_direction'].values.flatten()[
                          0:number_samples]

    predicted_probs = []
    for i in range(len(intercept_samples)):
        for ad in advertisement_vals:
            log_odds = (
                    intercept_samples[i] +
                    gd_samples[i] * goalkeeper_movement_initiation_range +
                    ad_samples[i] * ad +
                    interaction_samples[i] * goalkeeper_movement_initiation_range * ad
            )
            # Convert to probability
            prob = 1 / (1 + np.exp(-log_odds))
            predicted_probs.append(prob)

    predicted_probs = np.array(predicted_probs).reshape(len(intercept_samples), 2, -1)

    fig, ax = plt.subplots(figsize=(persistence.figure_width(ColumnFormat.DOUBLE), 3))

    # Colors (assigned from main script)
    color_cycle = plt.rcParams['axes.prop_cycle'].by_key()['color']
    first_color, second_color = color_cycle[0], color_cycle[1]

    # Plot for advertisement_direction = 0
    for i in range(len(intercept_samples)):
        ax.plot(goalkeeper_movement_initiation_range, predicted_probs[i, 0], alpha=0.1, color=first_color)

    # Plot for advertisement_direction = 1
    for i in range(len(intercept_samples)):
        ax.plot(goalkeeper_movement_initiation_range, predicted_probs[i, 1], alpha=0.1, color=second_color)

    # Add labels manually:
    ax.plot([], [], color=first_color, linewidth=6, label='Background Motion Left')
    ax.plot([], [], color=second_color, linewidth=6, label='Background Motion Right')

    # Calculate and plot the mean lines
    mean_probs_ad0 = predicted_probs[:, 0, :].mean(axis=0)
    mean_probs_ad1 = predicted_probs[:, 1, :].mean(axis=0)
    ax.plot(
        goalkeeper_movement_initiation_range,
        mean_probs_ad0,
        color="black"
    )
    ax.plot(
        goalkeeper_movement_initiation_range,
        mean_probs_ad1,
        color="black",
        linestyle="--"
    )

    # Set labels, title, and limits
    ax.spines['top'].set_visible(False)
    ax.spines['right'].set_visible(False)
    plt.legend()
    ax.legend(frameon=False)
    ax.set_ylim(0, 1)
    ax.set_xlim(x_lims)
    ax.set_xlabel("Goalkeeper movement initiation before kick [s]")
    ax.set_ylabel("Predicted Shot direction \nto right side [%]")

    # Set y-ticks to only 0% and 100%
    ax.set_yticks([0, 1])
    ax.yaxis.set_major_formatter(FuncFormatter(lambda y, _: f'{y * 100:.0f}%'))

    # Modify x-ticks: Add "-" sign to the original x-ticks
    x_ticks = np.linspace(1, 0, 6)  # From 1 to 0
    ax.set_xticks(x_ticks)
    ax.set_xticklabels([f"-{x:.1f}" for x in x_ticks])  # Add "-" to each tick label

    plt.tight_layout(pad=0.15)
    persistence.save_figure(fig, file_name)
    plt.close(fig)
