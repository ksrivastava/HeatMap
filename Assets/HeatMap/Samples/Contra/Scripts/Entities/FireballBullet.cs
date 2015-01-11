using UnityEngine;
using System.Collections;

public class FireballBullet : Bullet {

	private float radSpeed = 20f;
	private float radSize = 0.5f;
	private float startTime;
	void Start(){
		startTime = Time.time;
	}

	void FixedUpdate(){

		var timeMovement = Time.time - startTime;
		var radPos = new Vector2(Mathf.Sin(timeMovement * radSpeed) * radSize, Mathf.Cos(timeMovement * radSpeed) * radSize);

		transform.position = ((Vector2)transform.position + velocity * (Time.fixedDeltaTime)) + radPos;

	}
}
