using UnityEngine;
using System.Collections;

public class Sniper : ContraEntity {
	
	public	bool 		isWaterSniper = false;
	protected float		lastStep;
	protected int		bulletCount = 0;
	protected float 	timeBetweenSteps = 3f;
	protected int		numMaxBullets = 3;
	
	protected virtual void Start () {
		controller = new SniperController (this);
	}

	protected float t_lastStep = 0;
	protected float t_timeBetweenSteps = 0.5f;

	protected virtual void Update() {
		if (t_lastStep == 0) {
			t_lastStep = Time.time;
		}
		else if (Time.time - t_lastStep > t_timeBetweenSteps) {
			controller.Run ();
			t_lastStep = Time.time;
		}
	}

	public override void Shoot() {
		if (CanShoot () && renderer.enabled) {
			// Debug.Log ("Sniper canshoot!");
			PerformShoot ();
		} else {
			// Debug.Log("Sniper can't shoot!");
		}
	}


	protected virtual bool CanShoot(){
		var bill = GameObject.FindGameObjectWithTag ("BillRizer");
		if (bill != null && this.onCamera()) {

			
			if (bulletCount < numMaxBullets) {
				return true;
			} else {
				if (lastStep == 0) {
					lastStep = Time.time;
				} else if (Time.time - lastStep > timeBetweenSteps) {
					lastStep = Time.time;
					bulletCount = 0;
					return true;
				}
				
				return false;
			}
		} else {
			return false;
		}
	}

//	void OnTriggerEnter2D (Collider2D other)
//	{
//		if (other.tag == "Floor") {
//			
//			if( (transform.position.y) < other.bounds.max.y){
//				return;
//			}
//			onFloor = true;
//			
//			Vector2 pos = transform.position;
//			pos.y = other.bounds.max.y + transform.localScale.y / 2; 
//			
//			transform.position = pos;
//			
//		} else if (other.tag == "BillRizer") {
//			Bill bill = other.gameObject.GetComponent<Bill>(); 
//			bill.Damage();
//		}
//		
//	}

	protected virtual void PerformShoot() {
		//Debug.Log ("Sniper shoot!");
		GameObject bullet = Instantiate( bulletPrefab ) as GameObject;
		
		Vector3 pos = transform.position;
		// pos.x += ((transform.localScale.x/2 + bulletDeltaSpace) * (leftOrRight));
		// pos.y += ((transform.localScale.y/2 + bulletDeltaSpace)  * (upOrDown)); 
		// Not needed since bullets are always instantiated from the side, right?
		pos.z = 0.1f;
		bullet.transform.position = pos;
		
		Bullet b = bullet.GetComponent<Bullet>();
		b.owner = this;
		b.ownerTag = this.tag;
		b.SetVelocity(dir);
		bulletCount++;
	}
	
	public override void Damage(float damageTaken = 0) {
		Destroy (this.gameObject);
	}

	public virtual bool onCamera(){
		
		// set xRange so that Sniper only shoots once Bill can see it
		var mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		var leftPoint = mainCamera.camera.ViewportToWorldPoint (new Vector2 (0f,0f));
		var rightPoint = mainCamera.camera.ViewportToWorldPoint (new Vector2 (1f,1f));
		return (this.transform.position.x > leftPoint.x && this.transform.position.x < rightPoint.x &&
		        this.transform.position.y > leftPoint.y && this.transform.position.y < rightPoint.y);
	}
}
