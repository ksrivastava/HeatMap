using UnityEngine;
using System.Collections;

public class FlyingPowerUpContainer : StationaryPowerUpContainer {

	public float 		xSpeed = 0.1f;
	public float		baseYCoord;
	public float 		maxYCoord = 2f;
	public float		oscillationSpeed = 4f;

	private bool active = false;
	void Awake(){
		this.isOpen = false;
		baseYCoord = transform.position.y;
		this.renderer.enabled = false;
	}

	void Update(){
		GameObject bill = GameObject.FindGameObjectWithTag("BillRizer");

		var mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		var cameraLeftBound = mainCamera.camera.ViewportToWorldPoint (new Vector2 (0, 0));

		if (this.transform.position.x < cameraLeftBound.x + this.transform.localScale.x/2) {
			active = true;
			this.renderer.enabled = true;
			this.isOpen = true;
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (!active)
						return;
		// move in a sine wave
		Vector2 pos = transform.position;
		pos.x += xSpeed;
		pos.y = baseYCoord + Mathf.Sin(Time.time * oscillationSpeed) * maxYCoord;
		transform.position = pos;
	}
}
