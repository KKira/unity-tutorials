using System;
using UnityEngine;

public class Clock: MonoBehaviour {

	//The amount of degrees per hours does not change, make it a constant.
	const float degreesPerHour = 30f;
	const float degreesPerMinute = 6f;
	const float degreesPerSecond = 6f;

	public Transform hoursTransform, minutesTransform, secondsTransform;

	public bool continuous;

	void Update () {
		if (continuous) {
			UpdateContinuous();
		} 
		else {
			UpdateDiscrete();
		}
	}

	void UpdateContinuous () {

		//Retrieve the time only once by using a variable.
		TimeSpan time = DateTime.Now.TimeOfDay;
		//Clock has twelve hour indicators set at 30 degrees intervals.
		//To make rotation match, multiply hours by 30.
		//Need to apply rotation to the hoursTransform local rotation
		hoursTransform.localRotation = Quaternion.Euler(0f, (float)time.TotalHours * degreesPerHour, 0f);
		minutesTransform.localRotation = Quaternion.Euler(0f, (float)time.TotalMinutes * degreesPerMinute, 0f);
		secondsTransform.localRotation = Quaternion.Euler(0f, (float)time.TotalSeconds * degreesPerSecond, 0f);
	}

	void UpdateDiscrete () {

		//Retrieve the time only once by using a variable.
		DateTime time = DateTime.Now;
		//Clock has twelve hour indicators set at 30 degrees intervals.
		//To make rotation match, multiply hours by 30.
		//Need to apply rotation to the hoursTransform local rotation
		hoursTransform.localRotation = Quaternion.Euler(0f, time.Hour * degreesPerHour, 0f);
		minutesTransform.localRotation = Quaternion.Euler(0f, time.Minute * degreesPerMinute, 0f);
		secondsTransform.localRotation = Quaternion.Euler(0f, time.Second * degreesPerSecond, 0f);
	}
}