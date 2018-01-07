using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalBasedLearning : MonoBehaviour {

	public float timeRequiredToReachGoal;
	public Button NewTool;
	Stack<GameObject> ToolList = new Stack<GameObject>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool GoalTracker(float timeRequired){

		return true;

	}
}
