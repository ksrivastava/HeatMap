using UnityEngine;
using System.Collections;

public class BossCannon : ContraEntity {

	private float timeBetweenSteps = 0.9f;
	private float lastStep = 0;
	// Use this for initialization
	void Start () {
		this.health = 7;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (CanShoot ()) {
			Shoot();
		}
	}

	private bool CanShoot() {
		if (lastStep == 0) {
			lastStep = Time.time;
		}
		
		else if (Time.time - lastStep > timeBetweenSteps) {
			lastStep = Time.time;
			return true;
		}
		
		return false;
	}

	public override void Shoot ()
	{
		GameObject bullet = Instantiate( bulletPrefab ) as GameObject;
		
		Vector3 pos = transform.position;
		// pos.x += ((transform.localScale.x/2 + bulletDeltaSpace) * (leftOrRight));
		pos.z = 0.1f;
		bullet.transform.position = pos;
		
		Bullet b = bullet.GetComponent<Bullet>();
		b.owner = this;
		b.ownerTag = this.tag;
		b.SetVelocity(dir);
	}

	public override void Damage (float damageTaken)
	{
		this.health--;
		if (health <= 0) {
			Destroy(this.gameObject);
		}
	}
}
