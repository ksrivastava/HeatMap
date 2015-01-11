using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {

	GameObject lava;
	public bool isWaterFloor = false; 
	// Use this for initialization
	void Start () {
		if (Application.loadedLevelName == "Level_2") {
			lava = GameObject.FindGameObjectWithTag ("Lava");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Application.loadedLevelName == "Level_2") {
			var lavaTop = lava.renderer.bounds.max.y;
			collider2D.enabled = (lavaTop < collider2D.bounds.min.y);
		}
	}
}