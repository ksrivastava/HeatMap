using UnityEngine;
using System.Collections;

public class Lava : Boundary {
	
	public float 	lavaSpeed = 0.4f;
	public float 	startingYDelta = 1.5f;
	private bool	canMove;
	public float 	waitTime = 1.5f;
	Bill bill;

	void Start() {
		GameObject b = GameObject.FindGameObjectWithTag ("BillRizer");
		bill = b.GetComponent<Bill>();
		dam = -2;
		ResetPosition ();
		canMove = false;
		Invoke ("CanMove", waitTime);
	}

	void Update() {

		if (canMove) {
			var pos = transform.position;
			pos.y += lavaSpeed * Time.deltaTime;
			transform.position = pos;
		}
	}

	private void CanMove() {
		canMove = true;
	}

	public void ResetPosition () {
		canMove = false;

		Vector2 t_a = new Vector2 (0f, 0f); // will be the Bottom-Left
		var mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		t_a = mainCamera.camera.ViewportToWorldPoint (t_a);

		Vector3 pointA = new Vector3(t_a.x, t_a.y);

		pointA.x += renderer.bounds.size.x/2;
		pointA.y -= renderer.bounds.size.y / 2 - startingYDelta;
		pointA.z = -0.3f;
		transform.position = pointA;

		Invoke ("CanMove", waitTime);

	}


	protected void OnTriggerEnter2D (Collider2D other) {
		OnTriggerExit2D (other);
	}
}
