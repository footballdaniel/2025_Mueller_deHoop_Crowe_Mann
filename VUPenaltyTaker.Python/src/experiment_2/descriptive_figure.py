from dataclasses import asdict
from pathlib import Path
from typing import List

import matplotlib.pyplot as plt
import pandas as pd

from src.domain import TrialData, Persistence, ColumnFormat


def descriptive_figure(trials: List[TrialData], file_name: Path, persistence: Persistence):
    # Convert the trials data to a DataFrame
    df = pd.DataFrame([asdict(trial) for trial in trials])

    # Convert enums to strings for grouping
    df['shot_direction'] = df['shot_direction'].apply(lambda x: x.value)
    df['advertisement_direction'] = df['advertisement_direction'].apply(lambda x: x.value)

    # Calculate the percentage of shots to the right by group
    percentage_df = df.groupby(['goalkeeper_movement_initiation', 'advertisement_direction']).apply(
        lambda x: (x['shot_direction'] == "Right").mean() * 100
    ).reset_index(name='percentage_shot_right')

    # Plot the percentage of shots directed to the right for each ad direction
    fig = plt.figure(figsize=(persistence.figure_width(ColumnFormat.DOUBLE), 6))

    for ad_direction in percentage_df['advertisement_direction'].unique():
        data = percentage_df[percentage_df['advertisement_direction'] == ad_direction]
        plt.plot(
            -data['goalkeeper_movement_initiation'],  # Negating the x-axis values
            data['percentage_shot_right'],
            'o-',
            label=f'Background motion direction: {ad_direction}'
        )

    # Invert the x-axis (from 1 to 0)
    plt.xlim(1, 0)

    # Add labels and title
    plt.xlabel('Goalkeeper movement initiation level')
    plt.ylabel('Percentage of shots to the right')
    plt.title('Percentage of shots to the right by background motion direction and initiation level')
    plt.legend(title='Advertisement Direction')
    plt.tight_layout()
    plt.ylim(0, 100)

    plt.tight_layout(pad=0.15)
    persistence.save_figure(fig, file_name)
    plt.close(fig)
