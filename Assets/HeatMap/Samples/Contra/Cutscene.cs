using UnityEngine;
using System.Collections;

public class Cutscene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Invoke ("LoadLevel", 4f);
	}

	void LoadLevel() {
		Application.LoadLevel ("Level_2");
	}
}
