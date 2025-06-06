from pathlib import Path
import matplotlib.pyplot as plt
import seaborn as sns
import numpy as np
from scipy.stats import ttest_ind

from src.domain import Direction, Condition
from src.infra.data_reader import Reader

exp_3_data = Reader.read_csv_files(Path('data/Experiment_4/Experiment/**/*.csv'))

# same but i want to have a parameter to say what lambda filters
trials_right = lambda trial: trial.advertisement_direction == Direction.RIGHT
trials_left = lambda trial: trial.advertisement_direction == Direction.LEFT

right = list(filter(trials_right, exp_3_data))
left = list(filter(trials_left, exp_3_data))

# density plot with goalkeeper position (x), and swarm plot for points (scatter in y)
goalkeeper_right = [trial.goalkeeper_position for trial in right]
goalkeeper_left = [trial.goalkeeper_position for trial in left]

mean_right = sum(goalkeeper_right) / len(goalkeeper_right)
mean_left = sum(goalkeeper_left) / len(goalkeeper_left)

# Plot for condition "AdvertisementOnly"
right_advertisement_only = list(filter(lambda trial: trial.condition == Condition.ADVERTISEMENTONLY, right))
left_advertisement_only = list(filter(lambda trial: trial.condition == Condition.ADVERTISEMENTONLY, left))

goalkeeper_right_advertisement_only = [trial.goalkeeper_position for trial in right_advertisement_only]
goalkeeper_left_advertisement_only = [trial.goalkeeper_position for trial in left_advertisement_only]

mean_right_advertisement_only = sum(goalkeeper_right_advertisement_only) / len(goalkeeper_right_advertisement_only)
mean_left_advertisement_only = sum(goalkeeper_left_advertisement_only) / len(goalkeeper_left_advertisement_only)

# Plot for condition "AdvertisementAndGoal"
right_advertisement_and_goal = list(filter(lambda trial: trial.condition == Condition.ADVERTISEMENTANDGOAL, right))
left_advertisement_and_goal = list(filter(lambda trial: trial.condition == Condition.ADVERTISEMENTANDGOAL, left))

goalkeeper_right_advertisement_and_goal = [trial.goalkeeper_position for trial in right_advertisement_and_goal]
goalkeeper_left_advertisement_and_goal = [trial.goalkeeper_position for trial in left_advertisement_and_goal]

mean_right_advertisement_and_goal = sum(goalkeeper_right_advertisement_and_goal) / len(goalkeeper_right_advertisement_and_goal)
mean_left_advertisement_and_goal = sum(goalkeeper_left_advertisement_and_goal) / len(goalkeeper_left_advertisement_and_goal)

# Combine all three plots into a single figure with subplots arranged vertically
fig, axs = plt.subplots(3, 1, figsize=(10, 15))

# Main plot across conditions
sns.kdeplot(goalkeeper_right, ax=axs[0], label='Right')
sns.kdeplot(goalkeeper_left, ax=axs[0], label='Left')
axs[0].axvline(mean_right, color='blue', linestyle='dashed', linewidth=1)
axs[0].axvline(mean_left, color='orange', linestyle='dashed', linewidth=1)
axs[0].set_xlim(-2, 2)
axs[0].set_xlabel('Goalkeeper position')
axs[0].set_ylabel('Density')
axs[0].legend()
axs[0].set_title('Main plot across conditions')

# Plot for condition "AdvertisementOnly"
sns.kdeplot(goalkeeper_right_advertisement_only, ax=axs[1], label='Right')
sns.kdeplot(goalkeeper_left_advertisement_only, ax=axs[1], label='Left')
axs[1].axvline(mean_right_advertisement_only, color='blue', linestyle='dashed', linewidth=1)
axs[1].axvline(mean_left_advertisement_only, color='orange', linestyle='dashed', linewidth=1)
axs[1].set_xlim(-2, 2)
axs[1].set_xlabel('Perceived Goalkeeper position [m]')
axs[1].set_ylabel('Density')
axs[1].legend()
axs[1].set_title('Perception of goalkeeper when only advertisement is shown')

# Plot for condition "AdvertisementAndGoal"
sns.kdeplot(goalkeeper_right_advertisement_and_goal, ax=axs[2], label='Right')
sns.kdeplot(goalkeeper_left_advertisement_and_goal, ax=axs[2], label='Left')
axs[2].axvline(mean_right_advertisement_and_goal, color='blue', linestyle='dashed', linewidth=1)
axs[2].axvline(mean_left_advertisement_and_goal, color='orange', linestyle='dashed', linewidth=1)
axs[2].set_xlim(-2, 2)
axs[2].set_xlabel('Perceived Goalkeeper position [m]')
axs[2].set_ylabel('Density')
axs[2].legend()
axs[2].set_title('Perception of goalkeeper when advertisement and goal are shown')

plt.tight_layout()
plt.subplots_adjust(hspace=0.5) 
plt.show()
# save the figure
fig.savefig('goalkeeper_position.png')

"""Paired t-test for goalkeeper position"""
import numpy as np
from scipy.stats import ttest_rel
from statsmodels.stats.power import TTestPower

# Filter out trials where goalkeeper_position > 0.5m or < -0.5m
filtered_goalkeeper_right = np.array([trial.goalkeeper_position for trial in right if abs(trial.goalkeeper_position) <= 0.5])
filtered_goalkeeper_left = np.array([trial.goalkeeper_position for trial in left if abs(trial.goalkeeper_position) <= 0.5])

# Ensure paired data after filtering
min_size = min(len(filtered_goalkeeper_right), len(filtered_goalkeeper_left))
filtered_goalkeeper_right = np.random.choice(filtered_goalkeeper_right, min_size, replace=False)
filtered_goalkeeper_left = np.random.choice(filtered_goalkeeper_left, min_size, replace=False)

# Compute difference scores
filtered_differences = filtered_goalkeeper_right - filtered_goalkeeper_left

# Compute Cohen's d for paired t-test
filtered_mean_diff = np.mean(filtered_differences)
filtered_std_diff = np.std(filtered_differences, ddof=1)  # Standard deviation with Bessel's correction
cohens_d_filtered = filtered_mean_diff / filtered_std_diff

# Perform paired t-test
t_stat_filtered, p_value_filtered = ttest_rel(filtered_goalkeeper_right, filtered_goalkeeper_left)

# Power analysis for paired t-test
power_analysis_filtered = TTestPower()
sample_size_filtered = power_analysis_filtered.solve_power(effect_size=cohens_d_filtered, alpha=0.05, power=0.8)

print(f"Cohen's d (filtered): {cohens_d_filtered}")
print(f"T-statistic: {t_stat_filtered}, p-value: {p_value_filtered}")
print(f"Sample size needed (filtered): {round(sample_size_filtered)}")

