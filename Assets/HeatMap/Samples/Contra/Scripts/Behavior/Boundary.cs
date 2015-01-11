using UnityEngine;
using System.Collections;

public class Boundary : MonoBehaviour {
	/*void OnTriggerEnter2D (Collider2D other) {
		OnTriggerExit2D (other);
	}*/

	protected int dam = -1;

	protected void OnTriggerExit2D (Collider2D other)
	{
		if (other.tag == "Bullet") {
			Bullet bullet = other.gameObject.GetComponent<Bullet>(); 
			Destroy (bullet.gameObject);
		}
		else {
			ContraEntity entity = other.gameObject.GetComponent<ContraEntity>(); 
			if(entity != null) {
				entity.Damage(dam);
			}
		}
	}
}
