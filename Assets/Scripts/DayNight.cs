using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNight : MonoBehaviour {

	public float CycleMins;
	public float CycleCalc;
	public float OneCycle;
	public float time;
	public int startDay = 0;

	public GameObject displayDay;

	public string[] weekdays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

	// Use this for initialization
	void Start () {
		displayDay.GetComponent<Text> ().text = weekdays [startDay];
		CycleMins = 5f;
		OneCycle = 60 * CycleMins;
		CycleCalc = 0.1f / CycleMins * -1f;
	}

	// Update is called once per frame
	void Update () {

		if(Time.timeScale != 0){
			time += Time.deltaTime;
			transform.Rotate (0, 0, CycleCalc, Space.World);
		}

		if(time >= OneCycle) {
			time = time - OneCycle;
			startDay++;
			displayDay.GetComponent<Text>().text = weekdays[startDay%weekdays.Length];
		}

	}

}
