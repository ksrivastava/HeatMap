using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SGun : Gun {
	
	// Gun definitions taken from http://strategywiki.org/wiki/Contra_(NES)/Weapons
	
	private float bulletDamage = 1.5f;
	private float bulletSpeed = 8f;

	private float scatterRangeDegrees = 45f;

	public SGun (ContraEntity entity) {
		this.entity = entity;
		bulletPrefab = Resources.Load ("Bullet") as GameObject;
		bulletCount = 0;
		timeBetweenSteps = 0.5f;
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


		List<GameObject> bullets = new List<GameObject>();
		float[] rotations = new float[5] {0f, 1/16f, 1/8f, -1/16f, -1/8f};

		//create 5 bullets with no velocity but all other properties set
		for (int i=0; i<5; ++i) {
			GameObject bullet = Instantiate( bulletPrefab ) as GameObject;
			Vector3 pos = entity.transform.position;
			// pos.x += ((entity.transform.localScale.x/2 + entity.bulletDeltaSpace) * (entity.leftOrRight));
			pos.z = 0.1f;
			bullet.transform.position = pos;
			Bullet b = bullet.GetComponent<Bullet>();
			b.SetDamage (this.bulletDamage);
			b.SetSpeed (this.bulletSpeed);
			b.owner = entity;
			b.ownerTag = entity.tag;
			bullets.Add(bullet);
			b.speed = 10f;
			b.SetVelocity (RotateZ(entity.dir,rotations[i]));
		}
	}

	public static Vector2 RotateZ(Vector2 vector, float angle )
	{
		float sin = Mathf.Sin( angle );
		float cos = Mathf.Cos( angle );
		
		float tx = vector.x;
		float ty = vector.y;
		vector.x = (cos * tx) - (sin * ty);
		vector.y = (cos * ty) + (sin * tx);

		return vector;
	}
}
