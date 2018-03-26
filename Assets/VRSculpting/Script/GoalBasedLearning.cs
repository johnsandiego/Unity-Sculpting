using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace sandiegoJohn.VRsculpting{

	public class GoalBasedLearning : MonoBehaviour {

		public float timeRequiredToReachGoal = 60f;
		public GameObject[] Tools;
		Stack<GameObject> ToolList = new Stack<GameObject>();
		public GameObject currentTool;
		public GameObject[] currentTools;
		PaintMeshDeformer PMD;
		int count = 1;
		// Use this for initialization
		void Start () {
			for (int i = 0; i < Tools.Length; i++) {
				ToolList.Push (Tools [i]);
			}

			PMD = GameObject.FindGameObjectWithTag ("PMD").GetComponent<PaintMeshDeformer> ();

			foreach (GameObject tl in Tools) {
				tl.SetActive (false);
			}

			Tools [0].SetActive (true);
		}
		
		// Update is called once per frame
		void Update () {
			try{
				if (Tools[0].activeSelf || Tools[1].activeSelf || Tools[2].activeSelf || Tools[3].activeSelf || Tools[4].activeSelf || Tools[5].activeSelf) {
					timeRequiredToReachGoal -= Time.deltaTime;

					if (GoalReached(timeRequiredToReachGoal)) {

						if(count == 7){
							Debug.Log("You've learned all the tools! Congratulations!");
							return;
						}
						Tools[count].SetActive(true);

						timeRequiredToReachGoal = 60f;
						count++;


					}
				}
			}catch(Exception ex){
				if (ex is NullReferenceException || ex is MissingComponentException) {
					return;
				}
				throw;
			}


		}

		public bool GoalReached(float timeRequired){
			if (timeRequired < 1f) {
				return true;
			} else
				return false;


		}
	}

}