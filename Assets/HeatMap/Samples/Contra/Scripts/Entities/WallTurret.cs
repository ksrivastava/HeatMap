using UnityEngine;
using System.Collections;

public class WallTurret : Sniper {

	protected void Start () {
		base.Start ();
		controller = new WallTurretController (this);
		health = 5;
		numMaxBullets = 1;
		timeBetweenSteps = 0f;
		t_timeBetweenSteps = 2f;
	}

	public override void Damage(float damageTaken) {
		health -= damageTaken;
		if (health <= 0) {
			Debug.Log("Wall Turret destroyed");
			Destroy (gameObject);
		}
	}

}
