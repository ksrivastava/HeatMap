using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FootSoldierSpawner : MonoBehaviour {

	public int max = 0;
	private int spawned = 0;
	GameObject footSoldier;
	//public GameObject	marker;
	GameObject	bill;
	public float prob = 0.1f; // Probability that a foot soldier will spawn
	bool enabled = true;
	protected float t_lastStep = 0;
	protected float t_timeBetweenSteps = 0.5f;

	void Start() {
		bill = GameObject.FindGameObjectWithTag ("BillRizer");
		if (max == 0) {
			max = 10;
		}
	}

	
	public virtual bool onCamera(){
		
		// set xRange so that Sniper only shoots once Bill can see it
		var mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		var leftPoint = mainCamera.camera.ViewportToWorldPoint (new Vector2 (0f,0f));
		var rightPoint = mainCamera.camera.ViewportToWorldPoint (new Vector2 (1f,1f));
		return (this.transform.position.x > leftPoint.x && this.transform.position.x < rightPoint.x &&
		        this.transform.position.y > leftPoint.y && this.transform.position.y < rightPoint.y);
	}

	void FixedUpdate(){
		if (!onCamera ()) return;

		if (t_lastStep == 0) {
			t_lastStep = Time.time;
		}
		else if (Time.time - t_lastStep > t_timeBetweenSteps) {

			// Timer has run down.
			// Call the SpawnFootSoldier function TODO:with some probability
			if (Random.value <= prob) {
				SpawnFootSoldier();
			}

			t_lastStep = Time.time;
		}
	}

	void SpawnFootSoldier(){

		if (spawned >= max) {
			return;
		}

		// pick a side of the screen
		var mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");

		Vector2 pointA = new Vector2 (0f, 0f); // will be the Bottom-Left
		Vector2 pointB = new Vector2 (0f, 0f); // will be the Top-Right

		// set XY Coords for overlap rectangle
		int screenDirection = Random.Range (0, 2);// 0 IS LEFT ; 1 IS RIGHT
		if (screenDirection == 0) { 
			pointA.x = -0.1f; //TODO: tune this.
			pointA.y = 0f;

			pointB.x = +0f;
			pointB.y = 1f;
		} else {
			pointA.x = +1.1f; //TODO: tune this.
			pointA.y = 0f;

			pointB.x = +1f;
			pointB.y = 1f;
		}

		//convert from camera to world coordinates

		pointA = mainCamera.camera.ViewportToWorldPoint (pointA);
		pointB = mainCamera.camera.ViewportToWorldPoint (pointB);

		// Get all possible platforms scanning from the top
		Collider2D[] overlapObjects = Physics2D.OverlapAreaAll(pointA, pointB);
		List<GameObject> possibleFloors = new List<GameObject>();
	
		foreach(var overlapCollider in overlapObjects){
			if(overlapCollider.tag == "Floor" && overlapCollider.enabled)
				possibleFloors.Add(overlapCollider.gameObject);
		}

		// check to see if there are any possible floors
		if (possibleFloors.Count == 0) {
			return;
		}

		//pick a random platform from those candidates
		GameObject chosenPlatform = possibleFloors [Random.Range (0, possibleFloors.Count)];

		// spawn a footsoldier on that platform
		GameObject footSoldier = Instantiate(Resources.Load("FootSoldier")) as GameObject;
		Vector2 pos = chosenPlatform.transform.position;

		pos.y = chosenPlatform.collider2D.bounds.max.y + transform.localScale.y / 2;
		pos.x = pointA.x;
		footSoldier.transform.position = pos;

		spawned++;
	}
}
