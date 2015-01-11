using UnityEngine;
using System.Collections;

public class CannonController : SniperController {

	public CannonController (ContraEntity entity) : base(entity) 
	{
		angleBoundary = 45f;
	}

	protected override float adjustAngle(float angle) {
		return angle;
	}

	protected override bool canShoot(float angle) {
		if (angle == 315 || angle == 270) {
			return true;
		}
		else return false;
	}

}
