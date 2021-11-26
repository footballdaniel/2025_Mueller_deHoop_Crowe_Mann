import glob

import jsonpickle
from matplotlib import pyplot as plt

import streamlit as st

st.title("Penalty environment")
select_axis = st.selectbox('Foot movement', ('X', 'Y', 'Z'))

fig, ax = plt.subplots()
for filename in glob.glob('C:/Users/danie/AppData/LocalLow/DefaultCompany/VUPenalty/Trial*.json'):
    filecontent = open(filename)
    content = jsonpickle.decode(filecontent.read())
    filecontent.close()

    tracking = content["Tracking"]
    events = content["Events"]

    kick = events["Start"]
    kick_velocity_vector = kick["VelocityVector"]


    foot_movement = [item[select_axis] for item in tracking["Foot"]]

    plt.scatter(foot_movement, list(range(len(tracking["Foot"]))))

st.pyplot(fig)