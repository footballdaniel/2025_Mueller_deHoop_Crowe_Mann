using UnityEngine;
using VUPenalty;

public class Experiment3 : MonoBehaviour
{
    public User _userPrefab;
    public GameObject _keeperPrefab;
    public ExperimentalData _experimentalData;
    public GameObject Sphere;
    public VideoDisplay _videoDisplay;
    public TestMode _testMode;
    
    int _currentTrial = 0;
    Persistence _persistence;
    Pointer _pointer;
    ControllerInput _controllerInput;
    int _triggerCount;
    GameObject _keeper;
    TrialSetting _trial;

    void Start()
    {
        _persistence = new Persistence();
        var user = Instantiate(_userPrefab);
        _pointer = user.GetComponentInChildren<Pointer>();
        _controllerInput = user.GetComponentInChildren<ControllerInput>();

        if (_pointer == null || _controllerInput == null)
        {
            Debug.LogError("Pointer or ControllerInput not found");
            return;
        }
        
        _controllerInput.TriggerDown += OnTriggerDown;
        _pointer.Sphere = Sphere;
        _pointer.CanPlace = true;
        NextTrial();
    }

    [ContextMenu("Simulate trigger down")]
    void OnTriggerDown()
    {
        _triggerCount++;

        if (_triggerCount == 1)
        {
            Debug.Log("Can adjust");
            _pointer.CanPlace = false;
            _pointer.CanAdjust = true;
        }

        if (_triggerCount == 2)
        {
            _persistence.Add(SaveData.Create(_pointer.Sphere.transform.position.x, _trial.AdvertisementDirection));
            Debug.Log("OFFSET WAS: " + _pointer.Sphere.transform.position.x);
            _triggerCount = 0;
            _pointer.CanPlace = true;
            _pointer.CanAdjust = false;
            NextTrial();
        }
        
    }

    void NextTrial()
    {
        Debug.Log("Go to next trial");
        if (_currentTrial == _experimentalData.TrialSettings.Count)
        {
            _persistence.Save(_experimentalData.ParticipantName, _testMode);
            Debug.Log("EXPERIMENT FINISHED");
            return;
        }
        
        if (_keeper)
            Destroy(_keeper);
        
        _trial = _experimentalData.TrialSettings[_currentTrial];
        _videoDisplay.LoadVideo(_trial.Video);
        _videoDisplay.Play();
        Sphere.GetComponent<MeshRenderer>().enabled = false;
        
        Invoke(nameof(StartTrial), 1f);
        
        _currentTrial++;
    }

    public void StartTrial()
    {
        var displacement=  _trial.GoalKeeperDisplacement;
        _keeper = Instantiate(_keeperPrefab, new Vector3(displacement, 0, 0), Quaternion.identity);
        Destroy(_keeper, 2f);
        Invoke(nameof(LetUserSelect), 3f); 
    }
    
    public void LetUserSelect()
    {
        _pointer.CanPlace = true;
        Sphere.GetComponent<MeshRenderer>()
            .enabled = true;
    }
}
