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
        [SerializeField] CanvasGroup _UiContent;
    
        string SavePath => Application.persistentDataPath;

        void Awake()
        {
            _savePathString.SetText(SavePath);
            _UiContent.alpha = 0;
        }

        void OnEnable()
        {
            _openExplorer.onClick.AddListener(OpenExplorer);
            _toggleUi.onClick.AddListener(ToggleUi);
        }

        void ToggleUi()
        {
            _UiContent.alpha = _UiContent.alpha == 0 ? 1 : 0;
        }

        void OpenExplorer()
        {
            Application.OpenURL($"file://{SavePath}");
        }

        void OnDisable()
        {
            _openExplorer.onClick.RemoveListener(OpenExplorer);
        }
    }
}