using UnityEngine;
using System.Collections;

public class FGun : BasicGun {

	public FGun (ContraEntity entity) : base(entity) {
		this.entity = entity;
		bulletPrefab = Resources.Load ("FireballBullet") as GameObject;
		bulletCount = 0;
		timeBetweenSteps = 0.9f;
		numMaxBullets = 4;
	}
}
