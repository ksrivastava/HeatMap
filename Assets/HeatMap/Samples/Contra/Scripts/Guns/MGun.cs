using UnityEngine;
using System.Collections;

public class MGun : Gun {

	// Gun definitions taken from http://strategywiki.org/wiki/Contra_(NES)/Weapons

	private float bulletDamage = 1.5f;
	private float bulletSpeed = 14f;

	public MGun (ContraEntity entity) {
		this.entity = entity;
		bulletPrefab = Resources.Load ("Bullet") as GameObject;
		bulletCount = 0;
		timeBetweenSteps = 0.3f;
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
		GameObject bullet = Instantiate( bulletPrefab ) as GameObject;
			
		Vector3 pos =entity.transform.position;
		// pos.x += ((entity.transform.localScale.x/2 + entity.bulletDeltaSpace) * (entity.leftOrRight));
		pos.z = 0.1f;
		bullet.transform.position = pos;
		
		Bullet b = bullet.GetComponent<Bullet>();
		b.SetDamage (this.bulletDamage);
		b.SetSpeed (this.bulletSpeed);
		b.owner = entity;
		b.ownerTag = entity.tag;
		b.SetVelocity(entity.dir);
	}
}
