using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour {

	protected 		Vector2 velocity = Vector2.zero;
	public 			float speed = 6f;
	protected 		List<string> safeTags;
	protected 		float damageVal = 1f;
	public			string ownerTag = "";
	public			ContraEntity owner;
	public			float screenWidth;

	// Use this for initialization
	void Awake () {
		safeTags = new List<string>()
		{
			"Floor",
			"Bridge",
			"Boundary",
			"Water",
			"Bullet",
			"PowerUp"
		};
		var mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		screenWidth = (mainCamera.camera.orthographicSize * 2f * mainCamera.camera.aspect);
	}

	public void SetVelocity(Vector2 velocity){
		velocity.Normalize ();
		this.velocity = velocity * speed;

		var pos = transform.position;
		pos.z = 0.1f;
		transform.position = pos;
	}

	void FixedUpdate(){
		transform.position = (Vector2)transform.position + velocity * Time.fixedDeltaTime;

	}
	
	// Update is called once per frame
	void Update () {
		if (!onCamera ()) {
			Destroy (this.gameObject);
		}
	}

	public bool onCamera(){
		// set xRange so that Sniper only shoots once Bill can see it
		var mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		var leftPoint = mainCamera.camera.ViewportToWorldPoint (new Vector2 (0f,0f));
		var rightPoint = mainCamera.camera.ViewportToWorldPoint (new Vector2 (1f,1f));
		return (this.transform.position.x > leftPoint.x && this.transform.position.x < rightPoint.x &&
		        this.transform.position.y > leftPoint.y && this.transform.position.y < rightPoint.y);
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other != null && other.tag != null && !safeTags.Contains(other.tag) && other.tag != this.ownerTag) {
			ContraEntity entity = other.gameObject.GetComponent<ContraEntity>(); 
			entity.Damage(damageVal);
			Destroy (this.gameObject);
		}
	}

	public void SetDamage(float d){
		this.damageVal = d;
	}

	public void SetSpeed(float s){
		speed = s;
	}
}
