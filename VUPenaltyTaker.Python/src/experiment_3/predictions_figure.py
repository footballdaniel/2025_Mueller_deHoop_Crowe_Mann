from pathlib import Path

import arviz as az
import matplotlib.pyplot as plt
import numpy as np
from scipy.stats import norm

from src.domain import ColumnFormat
from src.infra.persistence import ApaStyledPersistence


def prediction_figure(inference_data: Path, file_name: Path, persistence: ApaStyledPersistence):
    results_1 = az.from_netcdf(str(inference_data))

    # Define the number of samples
    number_samples = 500

    # Extract posterior samples from the fitted model
    posterior_samples = results_1.posterior
    mu_samples = posterior_samples['mu'].values.flatten()[:number_samples]
    ad_direction_samples = posterior_samples['advertisement_direction'].values.flatten()[:number_samples]
    sigma_samples = posterior_samples['sigma'].values.flatten()[:number_samples]

    # Define the x-axis range for plotting the normal distribution
    x_range = np.linspace(-3, 3, 100)

    # Set up plot
    fig, ax = plt.subplots(figsize=(persistence.figure_width(ColumnFormat.DOUBLE), 1.5))
    color_cycle = plt.rcParams['axes.prop_cycle'].by_key()['color']
    first_color, second_color = color_cycle[0], color_cycle[1]

    # Generate and plot normal distributions for each sample
    for i in range(number_samples):
        mean_ad0 = mu_samples[i] + ad_direction_samples[i] * 0
        y_ad0 = norm.pdf(x_range, loc=mean_ad0, scale=sigma_samples[i])
        ax.plot(x_range, y_ad0, color=first_color, alpha=0.05)

        mean_ad1 = mu_samples[i] + ad_direction_samples[i] * 1
        y_ad1 = norm.pdf(x_range, loc=mean_ad1, scale=sigma_samples[i])
        ax.plot(x_range, y_ad1, color=second_color, alpha=0.05)

    # Calculate and plot the mean lines
    mean_mu = np.mean(mu_samples)
    mean_ad_direction = np.mean(ad_direction_samples)
    mean_sigma = np.mean(sigma_samples)

    # Posterior mean line for advertisement_direction == 0
    mean_y_ad0 = norm.pdf(x_range, loc=mean_mu, scale=mean_sigma)
    ax.plot(x_range, mean_y_ad0, color="black")

    # Posterior mean line for advertisement_direction == 1
    mean_y_ad1 = norm.pdf(x_range, loc=mean_mu + mean_ad_direction, scale=mean_sigma)
    ax.plot(x_range, mean_y_ad1, color="black", linestyle="--")

    # Show vertical lines at the mean of each distribution
    # # Add vertical dotted lines at the mean of each distribution
    # # Vertical line for advertisement_direction == 0
    # ax.vlines(mean_mu, ymin=0, ymax=norm.pdf(mean_mu, loc=mean_mu, scale=mean_sigma),
    #           color="black", linestyle="dashed")
    #
    # # Vertical line for advertisement_direction == 1
    # ax.vlines(mean_mu + mean_ad_direction, ymin=0,
    #           ymax=norm.pdf(mean_mu + mean_ad_direction, loc=mean_mu + mean_ad_direction, scale=mean_sigma),
    #           color="black", linestyle="dashed")

    # Add labels for color-coded lines
    ax.plot([], [], color=first_color, linewidth=6, label="Background Motion Left")
    ax.plot([], [], color=second_color, linewidth=6, label="Background Motion Right")

    # Customizing plot style
    ax.spines['top'].set_visible(False)
    ax.spines['right'].set_visible(False)

    # Set specific x and y limits and labels
    ax.set_xlim(-2, 2)
    ax.set_xlabel("Perceived Goalkeeper Position [cm]")
    ax.set_ylabel("Density")

    # Customize x-ticks to show in centimeters
    x_ticks_m = np.arange(-1.5, 1.6, 0.5)  # original ticks in meters
    x_ticks_cm = [int(x * 100) for x in x_ticks_m]  # convert to cm
    ax.set_xticks(x_ticks_m)
    ax.set_xticklabels(x_ticks_cm)

    # Add the legend
    ax.legend(frameon=False)

    # Apply tight layout
    plt.tight_layout(pad=0.15)

    persistence.save_figure(fig, file_name)
    plt.close(fig)
