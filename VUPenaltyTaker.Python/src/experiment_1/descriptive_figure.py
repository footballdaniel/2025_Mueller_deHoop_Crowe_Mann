from dataclasses import asdict
from pathlib import Path
from typing import List

import matplotlib.pyplot as plt
import pandas as pd

from src.domain import ColumnFormat, TrialData, Persistence


def descriptive_figure(trials: List[TrialData], file_name: Path, persistence: Persistence):
    # Convert the trials data to a DataFrame
    df = pd.DataFrame([asdict(trial) for trial in trials])

    # Convert the Enum type columns to strings to avoid TypeError during grouping
    df['advertisement_direction'] = df['advertisement_direction'].apply(lambda x: x.name if hasattr(x, 'name') else str(x))

    # Group by 'goalkeeper_displacement' and 'advertisement_direction' to calculate the mean 'shot_location_x'
    grouped = df.groupby(['goalkeeper_displacement', 'advertisement_direction'])['shot_location_x'].mean().reset_index()

    # Initialize the plot with the desired width from persistence
    fig = plt.figure(figsize=(persistence.figure_width(column_format=ColumnFormat.DOUBLE), 6))

    # Plot each line for advertisement directions separately
    for direction in df['advertisement_direction'].unique():
        condition_group = grouped[grouped['advertisement_direction'] == direction]

        # Scatter plot for the mean points
        plt.scatter(
            condition_group['goalkeeper_displacement'],
            condition_group['shot_location_x'],
            label=f'Mean Shot Location - {direction}',
            s=50
        )

        # Line plot for the trend across goalkeeper displacements
        plt.plot(
            condition_group['goalkeeper_displacement'],
            condition_group['shot_location_x'],
            linestyle='-',
            label=f'Line - {direction}'
        )

    plt.xlabel('Goalkeeper Displacement')
    plt.ylabel('Mean Shot Location')
    plt.title('Mean shot location by goalkeeper displacement and background motion direction')

    plt.ylim(0, 1)
    plt.legend()
    persistence.save_figure(fig, file_name)
    plt.close(fig)

