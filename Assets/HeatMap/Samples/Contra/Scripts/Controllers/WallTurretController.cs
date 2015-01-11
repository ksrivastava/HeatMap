using UnityEngine;
using System.Collections;

public class WallTurretController : SniperController {

	protected override float adjustAngle(float angle) {
		return angle;
	}

	public WallTurretController (ContraEntity entity) : base(entity) 
	{
		angleBoundary = 30f;
	}

}
