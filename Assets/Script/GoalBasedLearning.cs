using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalBasedLearning : MonoBehaviour {

	/*
		The main inspiration for this project is to create an easy to learn sculpting tool for beginners. 
		The author implemented a goal-based learning mechanic by introducing new features the longer 
		a specific tool is used in the scene. Meaning over time, an achievement will be shown in front of 
		the user saying that they have unlocked a new feature because they have been using a particular tool 
		long enough. Using this mechanic, the author can view their level of experience on a specific tool, 
		which the system will use to introduce new tools. The level of experience corresponds to the amount of 
		time the user used the tool. 
		The user will start with only able to use the following tools: mesh sculpting with additive and subtractive 
		drop down, mesh smoothing, and mesh symmetry. The user will start with these tools because they are intuitive 
		for beginners to learn first. While the user interacts with the clay, they are slowly gaining knowledge and 
		becoming more familiar with the following tools. The author created a user interface which shows the user how 
		long they have used each tool on the pallet. While the user holds the sculpt button, the system is counting 
		the time it is being used and storing that time in a local variable. All the variables increments when they are 
		used on the clay only, this will prevent outliers regarding if they are effectively learning the tools. 
	*/
	public float timeRequiredToReachGoal = 50F;
	float currentTime = 0f;
	public Button NewTool;
	Stack<GameObject> ToolList = new Stack<GameObject>();
	public Vector3 scultHead;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 100))
			Debug.DrawLine(ray.origin, hit.point);
		

		if (Input.GetMouseButton (0)) {
			timeRequiredToReachGoal -= Time.deltaTime;
			Debug.Log (timeRequiredToReachGoal);
		}
	}

	public bool GoalTracker(float timeRequired){

		return true;

	}
}
