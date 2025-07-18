using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VUPenalty
{
    public class Ui : MonoBehaviour
    {
        [Header("Dependencies")] [SerializeField] Button _toggleUi;
        [SerializeField] TMP_Text _savePathString;
        [SerializeField] Button _openExplorer;
        [SerializeField] Button _calibrate;
        [SerializeField] CanvasGroup _UiContent;

        string SavePath => Application.persistentDataPath;
        public event Action OnCalibratePress;

        void Awake()
        {
            _savePathString.SetText(SavePath);
            _UiContent.alpha = 0;
        }

        void OnEnable()
        {
            _openExplorer.onClick.AddListener(OpenExplorer);
            _toggleUi.onClick.AddListener(ToggleUi);
            _calibrate.onClick.AddListener(InitiateCalibration);
        }

        void OnDisable()
        {
            _openExplorer.onClick.RemoveListener(OpenExplorer);
        }

        void ToggleUi()
        {
            _UiContent.alpha = _UiContent.alpha == 0 ? 1 : 0;
        }

        void OpenExplorer()
        {
            Application.OpenURL($"file://{SavePath}");
        }

        void InitiateCalibration()
        {
            _calibrate.interactable = false;
            OnCalibratePress?.Invoke();
        }
    }
}