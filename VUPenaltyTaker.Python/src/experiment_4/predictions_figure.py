from pathlib import Path

import arviz as az
import numpy as np
from matplotlib import pyplot as plt
from scipy.stats import norm

from src.domain import ColumnFormat, Persistence


def prediction_figure(inference_data: Path, file_name: Path, persistence: Persistence):
    results = az.from_netcdf(str(inference_data))
    number_samples = 500
    posterior_samples = results.posterior
    intercept_samples = posterior_samples["Intercept"].values.flatten()[:number_samples]
    ad_samples = posterior_samples["advertisement_direction"].values.flatten()[:number_samples]
    interaction_samples = posterior_samples["advertisement_direction:condition"].values.flatten()[:number_samples]
    sigma_samples = posterior_samples["sigma"].values.flatten()[:number_samples]
    x_range = np.linspace(-1.5, 1.5, 100)
    advertisement_vals = [0, 1]
    conditions = [0, 1]

    all_preds = np.empty((number_samples, 2, 2, x_range.shape[0]))
    for cond_idx, cond in enumerate(conditions):
        for ad_idx, ad in enumerate(advertisement_vals):
            for i in range(number_samples):
                pred = intercept_samples[i] + ad_samples[i] * ad + interaction_samples[i] * ad * cond
                y = norm.pdf(x_range, loc=pred, scale=sigma_samples[i])
                all_preds[i, cond_idx, ad_idx, :] = y
    mean_preds = np.mean(all_preds, axis=0)

    fig, axes = plt.subplots(nrows=2, figsize=(persistence.figure_width(ColumnFormat.DOUBLE), 1.5 * 2))

    color_cycle = plt.rcParams["axes.prop_cycle"].by_key()["color"]
    first_color, second_color = color_cycle[0], color_cycle[1]

    for cond_idx, cond in enumerate(conditions):
        ax = axes[cond_idx]
        for ad_idx, ad in enumerate(advertisement_vals):
            for i in range(number_samples):
                ax.plot(x_range, all_preds[i, cond_idx, ad_idx, :],
                        color=first_color if ad_idx == 0 else second_color,
                        alpha=0.05)
        for ad_idx, ad in enumerate(advertisement_vals):
            ax.plot(x_range, mean_preds[cond_idx, ad_idx, :],
                    color="black",
                    linestyle="solid" if ad_idx == 0 else "dashed")

        ax.set_xlim(-1.5, 1.5)
        axes[1].set_xlabel("Perceived Goalkeeper Position [cm]")
        ax.set_ylabel("Density")
        x_ticks_m = np.arange(-1.5, 1.51, 0.5)
        x_ticks_cm = [int(x * 100) for x in x_ticks_m]
        ax.set_xticks(x_ticks_m)
        ax.set_xticklabels(x_ticks_cm)
        title = "Background" if cond == 0 else "Background And Goal"
        ax.set_title(title)
        ax.spines["top"].set_visible(False)
        ax.spines["right"].set_visible(False)

    axes[1].plot([], [], color=first_color, linewidth=6, label="Background Motion Left")
    axes[1].plot([], [], color=second_color, linewidth=6, label="Background Motion Right")
    axes[1].legend(frameon=False)

    plt.tight_layout(pad=0.15, h_pad=0.5)
    persistence.save_figure(fig, file_name)
    plt.close(fig)
