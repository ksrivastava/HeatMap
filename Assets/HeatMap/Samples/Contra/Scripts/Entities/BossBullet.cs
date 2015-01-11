using UnityEngine;
using System.Collections;

public class BossBullet : Bullet {

	public Vector2		vel = Vector2.zero;
	public float		gravityVal = -18f;
	public float 		xSpeed = -1f;
	public float		jumpVal = 5f;

	void Start(){
		Vector2 jumpForce = new Vector2(xSpeed, jumpVal);
		vel += (Vector2)jumpForce;
		transform.position =  (Vector2)transform.position +vel*Time.deltaTime;
	}

	void FixedUpdate(){
		// Apply gravity and acc to vel
		vel += new Vector2(0,gravityVal) * Time.fixedDeltaTime;
		
		// Apple vel to position
		transform.position = (Vector2) transform.position + vel * Time.fixedDeltaTime;
	}
	
}
