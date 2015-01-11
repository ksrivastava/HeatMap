using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BillVerticalSpawner : MonoBehaviour {

	private GameObject 	bill;
	public float 		lavaSpeed = 0.6f;
	public bool 		setPosition = false;
	public bool 		isBossSpawner = false;
	public float 		startingYDelta = 1.5f;
	private Lava 		lava;
	void Start () {
		bill = GameObject.FindGameObjectWithTag ("BillRizer");
		GameObject lavaObj = GameObject.FindGameObjectWithTag ("Lava");
		lava = (Lava) lavaObj.GetComponent(typeof(Lava));
	}
	
	// Update is called once per frame
	void Update () {
		if (bill == null)
						return;
		if (!isBossSpawner && bill.transform.position.y >= this.transform.position.y || (
			isBossSpawner && bill.transform.position.y + 1f >= this.transform.position.y)) {
			Bill b = bill.GetComponent<Bill>();
			if (b.spawner.transform.position.y < this.transform.position.y) {
				b.spawner = this.gameObject;
				lava.lavaSpeed = lavaSpeed;
				lava.startingYDelta = startingYDelta;

				if (setPosition) {
					lava.ResetPosition ();
				}
			}
		}
	}

//	public void PositionSpawner() {
//		// pick a side of the screen
//		var mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
//		
//		Vector2 pointA = new Vector2 (0f, 0f); // will be the Bottom-Left
//		Vector2 pointB = new Vector2 (1f, 1f); // will be the Top-Right
//		
//		// set XY Coords for overlap rectangle
//		int screenDirection = 1;
//		
//		//convert from camera to world coordinates
//		pointA = mainCamera.camera.ViewportToWorldPoint (pointA);
//		pointB = mainCamera.camera.ViewportToWorldPoint (pointB);
//		
//		// Get all possible platforms scanning from the top
//		Collider2D[] overlapObjects = Physics2D.OverlapAreaAll(pointA, pointB);
//
//		GameObject chosenPlatform = null;
//
//		foreach(var overlapCollider in overlapObjects){
//			if(overlapCollider.tag == "Floor") {
//				var lavaTop = lava.renderer.bounds.max.y;
//				bool isActive = (lavaTop < overlapCollider.bounds.min.y);
//
//				if (isActive) {
//					if (chosenPlatform == null) {
//						chosenPlatform = overlapCollider.gameObject;
//					}
//					else if (overlapCollider.gameObject.transform.position.y < chosenPlatform.transform.position.y) {
//						chosenPlatform = overlapCollider.gameObject;
//					}
//				}
//			}
//		}
//		
//		// check to see if there are any possible floors
//		if (chosenPlatform == null) {
//			throw new Exception();
//		}
//		
//		// spawn a footsoldier on that platform
//		GameObject bill = GameObject.FindGameObjectWithTag ("BillRizer");
//		Bill b = bill.GetComponent<Bill> ();
//
//		var height = bill.transform.localScale.y;
//		Vector2 pos = chosenPlatform.transform.position;
//		pos.y = chosenPlatform.collider2D.bounds.max.y + height / 2 + 0.3f;
//
//		transform.position = pos;
//
//	}
}
