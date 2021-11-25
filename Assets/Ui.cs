using TMPro;
using UnityEngine;

public class Ui : MonoBehaviour
{
    [SerializeField] TMP_Text _savePath;

    void Awake()
    {
        _savePath.SetText(Application.persistentDataPath);
    }
}