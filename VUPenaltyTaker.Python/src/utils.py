def probability_effect_is_zero(x):
    prob = max((x > 0).mean(), (x < 0).mean())
    rounded_prob = round(prob, 2)
    return 1 - rounded_prob
