using UnityEngine;
using System.Collections;

public class CamoSniperController : Controller {

	private GameObject 		bill = null;



	public CamoSniperController (ContraEntity entity) : base(entity) 
	{
		bill = GameObject.FindGameObjectWithTag ("BillRizer");
	}

	public override void Run () {
		entity.Uncrouch ();

		var mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		Vector2 screenPos = mainCamera.camera.WorldToViewportPoint(entity.transform.position);
		if(screenPos.x > 1.0 || screenPos.x <0.0)
			return;

		if (bill == null) {
			return;
		} else {
			entity.leftOrRight = (bill.transform.position.x < entity.transform.position.x) ? -1 : 1;
			entity.dir = new Vector2(entity.leftOrRight, 0);
		}
		
		if (entity.leftOrRight < 1) {
			entity.MoveLeft();
		} else {
			entity.MoveRight();
		}

		// adjust direction && shoot
		entity.Shoot ();

		entity.Crouch ();
	}

}
