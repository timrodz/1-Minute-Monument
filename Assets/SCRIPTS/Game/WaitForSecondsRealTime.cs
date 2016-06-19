using System;
using UnityEngine;

public class WaitForSecondsRealTime : CustomYieldInstruction {

	private float waitTime;

	public override bool keepWaiting {

		get {
			return (Time.realtimeSinceStartup < waitTime);
		}

	}

	public WaitForSecondsRealTime(float time) {

		waitTime = Time.realtimeSinceStartup + time;

	}
	
}