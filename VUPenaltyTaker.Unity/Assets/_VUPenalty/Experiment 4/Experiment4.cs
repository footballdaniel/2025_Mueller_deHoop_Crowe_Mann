using System;
using UnityEngine;
using VUPenalty;

public class Experiment4 : MonoBehaviour
{
	public User _userPrefab;
	public GameObject _keeperPrefab;
	public ExperimentalData _experimentalData;
	public GameObject Sphere;
	public GameObject Goal;
	public VideoDisplay _videoDisplay;
	public UIExperiment4 _uiExperiment4;
	public TestMode _testMode;

	ControllerInput _controllerInput;
	int _currentTrial = 0;
	GameObject _keeper;
	Persistence _persistence;
	Pointer _pointer;
	float _reactionTime;
	float _reactionTimeAdjustment;
	float _startSelectionTime;
	TrialSetting _trial;
	int _triggerCount;

	public int CurrentTrial => _currentTrial;

	void Start()
	{
		_uiExperiment4.Bind(this);

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

		if (_testMode == TestMode.AdvertisementOnly)
			HideGoal();
	}

	public event Action ExperimentFinished;

	public void LetUserSelect()
	{
		_pointer.CanPlace = true;
		_startSelectionTime = Time.time;
		
		_pointer.ShowLiveLocation();
		
		Sphere.GetComponent<MeshRenderer>().enabled = true;
	}

	public void StartTrial()
	{
		if (_testMode == TestMode.AdvertisementAndGoal)
			Goal.SetActive(true);


		var displacement = _trial.GoalKeeperDisplacement;
		_keeper = Instantiate(_keeperPrefab, new Vector3(displacement, 0, 0), Quaternion.identity);
		Destroy(_keeper, 2f);
		Invoke(nameof(LetUserSelect), 3f);
		Invoke(nameof(HideGoal), 2f);
		Invoke(nameof(HideAdvertisement), 2f);
	}

	void HideAdvertisement()
	{
		_videoDisplay.gameObject.SetActive(false);
	}

	void HideGoal()
	{
		Goal.SetActive(false);
	}

	void NextTrial()
	{
		Debug.Log("Go to next trial");

		if (_currentTrial == _experimentalData.TrialSettings.Count)
		{
			_persistence.Save(_experimentalData.ParticipantName, _testMode);
			ExperimentFinished?.Invoke();
			return;
		}

		if (_keeper)
			Destroy(_keeper);

		_trial = _experimentalData.TrialSettings[_currentTrial];
		_videoDisplay.gameObject.SetActive(true);
		_videoDisplay.LoadVideo(_trial.Video);
		_videoDisplay.Play();
		Sphere.GetComponent<MeshRenderer>().enabled = false;

		Invoke(nameof(StartTrial), 1f);

		_currentTrial++;
	}

	[ContextMenu("Simulate trigger down")]
	void OnTriggerDown()
	{
		if (!_pointer.CanAdjust && !_pointer.CanPlace)
		{
			Debug.Log("Not allowed to click");
			return;
		}

		_triggerCount++;

		if (_triggerCount == 1)
		{
			_reactionTime = Time.time - _startSelectionTime;
			_startSelectionTime = Time.time;
			Debug.Log("Can adjust");
			_pointer.CanPlace = false;
			_pointer.CanAdjust = true;
		}

		if (_triggerCount == 2)
		{
			_reactionTimeAdjustment = Time.time - _startSelectionTime;

			var saveData = SaveData.Create(
				_pointer.Sphere.transform.position.x,
				_reactionTime,
				_reactionTimeAdjustment,
				_trial.AdvertisementDirection);
			_persistence.Add(saveData);
			Debug.Log("OFFSET WAS: " + _pointer.Sphere.transform.position.x);
			_triggerCount = 0;
			_pointer.CanPlace = false;
			_pointer.CanAdjust = false;
			NextTrial();
		}
	}

	public void StartExperiment()
	{
		NextTrial();
	}

	public void NameParticipant(string participantNameText)
	{
		_experimentalData.ParticipantName = participantNameText;
	}
}