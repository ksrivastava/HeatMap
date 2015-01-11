using UnityEngine;
using System.Collections;

public class BasicGun : Gun {

	private KeyCode	shootKey = KeyCode.Z;
	
	public BasicGun (ContraEntity entity) {
		this.entity = entity;
		bulletPrefab = Resources.Load ("Bullet") as GameObject;
		bulletCount = 0;
		timeBetweenSteps = 0.8f;
		numMaxBullets = 4;
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
		GameObject bullet = Instantiate( bulletPrefab ) as GameObject;

		Vector3 pos = entity.transform.position;
		// pos.x += ((entity.transform.localScale.x/2 + entity.bulletDeltaSpace) * (entity.leftOrRight));
		pos.z = 0.1f;
		bullet.transform.position = pos;
		
		Bullet b = bullet.GetComponent<Bullet>();
		b.owner = entity;

		b.speed = 14f;
		b.ownerTag = entity.tag;
//		Debug.Log ("The owner is " + b.owner);
		b.SetVelocity(entity.dir);
	}
}