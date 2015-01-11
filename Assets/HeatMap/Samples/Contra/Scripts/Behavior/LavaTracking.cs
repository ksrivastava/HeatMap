using UnityEngine;
using System.Collections;

public class LavaTracking : MonoBehaviour {

	public float 		lavaSpeed = 0.4f;
	public float 		waitTime = 2.0f;
	private GameObject 	lavaObject;
	public GameObject	billSpawner;
	private bool		canMove;

	// Use this for initialization
	void Start () {
		lavaObject = GameObject.FindGameObjectWithTag ("Lava");
		canMove = false;
		Invoke ("CanMove", waitTime);
	}
	
	// Update is called once per frame

	void Update () {
		if (canMove) {
			updatePosition (this.gameObject);
			updatePosition (lavaObject);
			updatePosition (billSpawner);
		}
	}

	public void ResetCamera() {
		Debug.Log ("Resetting camera");

		float t_delta = 3f;
		resetPosition (this.gameObject, t_delta);
		resetPosition (lavaObject, t_delta);
		resetPosition (billSpawner, t_delta);

		canMove = false;
		Invoke ("CanMove", waitTime);
	}

	private void CanMove() {
		canMove = true;
	}

	private void updatePosition(GameObject obj) {
		var pos = obj.transform.position;
		pos.y += lavaSpeed * Time.deltaTime;
		obj.transform.position = pos;
	}

	private void resetPosition(GameObject obj, float delta) {
		var pos = obj.transform.position;
		pos.y -= delta;
		obj.transform.position = pos;
	}
}
