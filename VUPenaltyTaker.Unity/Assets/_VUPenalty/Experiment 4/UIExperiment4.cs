using System.Diagnostics;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIExperiment4 : MonoBehaviour
{
    [SerializeField] Button _startExperiment;
    [SerializeField] Button _openFolder;
    [SerializeField] Button _exit;
    [SerializeField] RectTransform _experimentFinishedPanel;
    [SerializeField] TMP_Text _trialIndexText;
    [SerializeField] TMP_InputField _participantName;
    Experiment4 _experiment4;

    public void Bind(Experiment4 experiment)
    {
        _experiment4 = experiment;
        _experiment4.ExperimentFinished += OnExperimentFinished;
        _startExperiment.onClick.AddListener(OnStartExperiment);
        _openFolder.onClick.AddListener(OnOpenFolder);
        _exit.onClick.AddListener(OnExited);
    }

    void OnExperimentFinished()
    {
        _experimentFinishedPanel.gameObject.SetActive(true);
    }

    void OnExited()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
    }

    void OnStartExperiment()
    {
        _experiment4.StartExperiment();
        _startExperiment.interactable = false;
        _experiment4.NameParticipant(_participantName.text);
    }

    void Update()
    {
        _trialIndexText.SetText($"Trial: {_experiment4.CurrentTrial}");
        
    }

    public void OnOpenFolder()
    {
        Process.Start(Application.persistentDataPath);
    }
}