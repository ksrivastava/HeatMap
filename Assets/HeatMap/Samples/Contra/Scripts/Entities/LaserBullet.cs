using UnityEngine;
using System.Collections;

public class LaserBullet : Bullet {

	void OnTriggerEnter2D (Collider2D other) {
		if (other != null && other.tag != null && !safeTags.Contains(other.tag) && other.tag != this.ownerTag) {
			ContraEntity entity = other.gameObject.GetComponent<ContraEntity>(); 
			entity.Damage(damageVal);
		}
	}
}
