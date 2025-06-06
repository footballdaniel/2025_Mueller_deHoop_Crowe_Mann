using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using VUPenalty;

public class Pointer : MonoBehaviour
{
	public GameObject Sphere;
	[SerializeField] ControllerInput _controllerInput;
	[SerializeField] bool _firstTrigger;
	[SerializeField] bool _secondTrigger;

	public bool CanPlace { get; set; }
	public bool CanAdjust { get; set; }

	void Update()
	{
		if (CanPlace)
			ShowLiveLocation();

		if (CanAdjust)
			AddAdjustment();
	}

	void AddAdjustment()
	{
		Sphere.transform.position += new Vector3(_controllerInput.Offset, 0, 0);
	}

	public void ShowLiveLocation()
	{
		var ray = new Ray(transform.position, transform.forward);

		if (Physics.Raycast(ray, out var hit, 100, LayerMask.GetMask("Goal")))
			Sphere.transform.position = hit.point;
	}
}

[Serializable]
public enum TestMode
{
	AdvertisementOnly,
	AdvertisementAndGoal
}

[Serializable]
public class Persistence
{
	[SerializeField] public List<SaveData> _saveData = new();

	public void Add(SaveData data)
	{
		_saveData.Add(data);
	}

	public void Save(string fileName, TestMode testMode)
	{
		var points = _saveData.Select(data => data.PositionX)
			.ToList();

		var directions = _saveData.Select(data => data.AdvertisementDirection.ToString())
			.ToList();

		var reactionTimeAdjustments = _saveData.Select(data => data.ReactionTimeAdjustment)
			.ToList();

		var reactionTimes = _saveData.Select(data => data.ReactionTime)
			.ToList();

		var path = Application.persistentDataPath + $"/save_{fileName}_{testMode}.csv";

		using (var file = File.CreateText(path))
		{
			for (var i = 0; i < points.Count; i++)
				file.WriteLine($"{points[i]},{directions[i]},{testMode},{reactionTimes[i]},{reactionTimeAdjustments[i]}");
		}
	}
}

[Serializable]
public class SaveData
{
	[SerializeField] public float PositionX { get; set; }
	[SerializeField] public Direction AdvertisementDirection { get; set; }
	[SerializeField] public float ReactionTimeAdjustment { get; set; }
	[SerializeField] public float ReactionTime { get; set; }

	public static SaveData Create(float positionX, Direction trialAdvertisementDirection)
	{
		return new SaveData
		{
			PositionX = positionX,
			AdvertisementDirection = trialAdvertisementDirection
		};
	}

	public static SaveData Create(float positionX, float trialAdvertisementDirection, float reactionTimeAdjustment, Direction advertisementDirection)
	{
		return new SaveData
		{
			PositionX = positionX,
			AdvertisementDirection = advertisementDirection,
			ReactionTimeAdjustment = reactionTimeAdjustment,
			ReactionTime = trialAdvertisementDirection
		};
	}
}