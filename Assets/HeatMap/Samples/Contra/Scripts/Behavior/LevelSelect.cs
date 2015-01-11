using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

	public GUIText level;
	public string levelName;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			Application.LoadLevel("Level_1");
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			Application.LoadLevel("Level_2");
		}
	}


}
