using UnityEngine;
using System.Collections;

public class LGun : Gun {

	GameObject firedBeam = null;

	public LGun (ContraEntity entity) {
		this.entity = entity;
		bulletPrefab = Resources.Load ("LaserBullet") as GameObject;
		bulletCount = 0;
		timeBetweenSteps = 0.4f;
		numMaxBullets = 1;
	}
	
	public override void Shoot() {
		if (canShoot()) {
			PerformShoot();
			bulletCount++;
		}
	}
	
	private bool canShoot() {
		if (bulletCount < numMaxBullets) {
			return true;
		}
		else {
			if (lastStep == 0) {
				lastStep = Time.time;
			}
			
			else if (Time.time - lastStep > timeBetweenSteps) {
				lastStep = Time.time;
				bulletCount = 0;
				return true;
			}
			
			return false;
		}
	}
	
	private void PerformShoot() {

		//destroy a beam fired earlier if it exists
		if (firedBeam) {
			Destroy(firedBeam);
		}

		GameObject bullet = Instantiate( bulletPrefab ) as GameObject;
		
		Vector3 pos = entity.transform.position;
		// pos.x += ((entity.transform.localScale.x/2 + entity.bulletDeltaSpace) * (entity.leftOrRight));
		pos.z = 0.1f;
		bullet.transform.position = pos;
		
		Bullet b = bullet.GetComponent<Bullet>();
		b.owner = entity;
		
		b.speed = 9f;
		b.ownerTag = entity.tag;
		//		Debug.Log ("The owner is " + b.owner);
		b.SetVelocity(entity.dir);

		//rotate bullet
		var r = bullet.transform.rotation;
		r.z = 0.5f*((entity.dir.y != 0)? entity.dir.y : 0)*((entity.dir.x != 0)? entity.dir.x : 2);

		bullet.transform.rotation = r;

		firedBeam = bullet;
	}
}
