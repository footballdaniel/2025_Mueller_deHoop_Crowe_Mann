import arviz as az
import matplotlib.pyplot as plt

from src.utils import probability_effect_is_zero

results_1 = az.from_netcdf("../exp_1.nc")

# Separation plot
ax = az.plot_separation(results_1, y="shot_direction", figsize=(9, 0.5))
plt.show()

# Posterior
az.plot_posterior(
    results_1,
    var_names=["~^p$|participant"],
    filter_vars="regex"
)
plt.show()

# Summary with custom stats
summary = az.summary(
    results_1,
    kind="stats",
    hdi_prob=0.94,
    stat_funcs={"Zero effect probability": probability_effect_is_zero},
    var_names=["~^p$|participant"],
    filter_vars="regex"
)

# Printing
print(summary)
print("posterior mean for specific variable")
print((results_1.posterior["goalkeeper_displacement:advertisement_direction"] > 0).mean().item())


# forrest plot
fig, ax = plt.subplots(figsize=(7, 3), dpi=120)
forest = az.plot_forest(
    results_1,
    kind='ridgeplot',
    hdi_prob=0.94,
    # combined=True,
    # rope=[-1, 1],
    var_names=["~^p$|participant"],
    filter_vars="regex",
    ridgeplot_truncate=False,
    ax=ax
)
# add a vertical line at 0
ax.axvline(0, color="black", linestyle="--")
plt.tight_layout()
plt.show()
plt.savefig("exp_1.png")

# predictions based on individuals
samples = az.extract(results_1, group="posterior_predictive", num_samples=100)
shot_direction_predictions = samples["shot_direction"]

# Model comparison
# https://bambinos.github.io/bambi/notebooks/model_comparison.html
models_dict = {
    "model1": results_1,
}
df_compare = az.compare(models_dict)
az.plot_compare(df_compare, insample_dev=False)
