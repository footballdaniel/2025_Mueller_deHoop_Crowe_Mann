from pathlib import Path

import pandas as pd

import src.experiment_1 as exp_1
import src.experiment_2 as exp_2
import src.experiment_3 as exp_3
import src.experiment_4 as exp_4
from src.infra.data_reader import Reader
from src.infra.persistence import ApaStyledPersistence

if __name__ == '__main__':
    pd.set_option('display.max_columns', None)  # Show all columns
    pd.set_option('display.width', 1000)  # Set a wider width

    persistence = ApaStyledPersistence(
        font=Path("Calibri.ttf"),
        font_size=11,
        double_column_width_inches=6.5,
        single_column_width_inches=3,
        grayscale=False
    )

    exp_1_data = Reader.read_json_files(Path('data/Experiment_1/**/*.json'))
    exp_1.model_1(exp_1_data, Path("exp_1.nc"), Path("results/exp1/model"), persistence)
    exp_1.model_1_table(Path("exp_1.nc"), Path("results/exp1/model"), persistence)
    exp_1.prediction_figure(Path("exp_1.nc"), Path("results/exp1/predictions"), persistence)
    exp_1.descriptive_table(exp_1_data, Path("results/exp1/shot_distribution"), persistence)
    exp_1.descriptive_figure(exp_1_data, Path("results/exp1/shot_distribution_figure"), persistence)
    exp_1.descriptive_table_conditional_on_goalkeeper(
        exp_1_data,
        Path("results/exp1/shot_distribution_by_gk"),
        persistence
    )

    exp_2_data = Reader.read_json_files(Path('data/Experiment_2/**/*.json'))
    exp_2.model_1(exp_2_data, Path("exp_2.nc"), Path("results/exp2/model"), persistence)
    exp_2.model_1_table(Path("exp_2.nc"), Path("results/exp2/model"), persistence)
    exp_2.prediction_figure(Path("exp_2.nc"), Path("results/exp2/predictions"), persistence)
    exp_2.descriptive_table(exp_2_data, Path("results/exp2/shot_distribution_table"), persistence)
    exp_2.descriptive_figure(exp_2_data, Path("results/exp2/shot_distribution_figure"), persistence)

    exp_3_data = Reader.read_csv_files(Path('data/Experiment_3/Experiment/**/*.csv'))
    exp_3.remove_outliers(exp_3_data, Path("results/exp3/outliers.txt"), persistence)
    exp_3.model_1(exp_3_data, Path("exp_3.nc"), Path("results/exp3/model"), persistence)
    exp_3.model_1_table(Path("exp_3.nc"), Path("results/exp3/model"), persistence)
    exp_3.prediction_figure(Path("exp_3.nc"), Path("results/exp3/predictions"), persistence)
    exp_3.descriptive_table(exp_3_data, Path("results/exp3/shot_distribution"), persistence)

    exp_4_data = Reader.read_csv_files(Path('data/Experiment_4/Experiment/**/*.csv'))
    exp_4.remove_outliers(exp_4_data, Path("results/exp4/outliers.txt"), persistence)
    exp_4.remove_warm_up_trials(exp_4_data)
    exp_4.model_1(exp_4_data, Path("exp_4.nc"), Path("results/exp4/model"), persistence)
    exp_4.model_1_table(Path("exp_4.nc"), Path("results/exp4/model"), persistence)
    exp_4.prediction_figure(Path("exp_4.nc"), Path("results/exp4/predictions"), persistence)
    exp_4.descriptive_table(exp_4_data, Path("results/exp4/shot_distribution"), persistence)

