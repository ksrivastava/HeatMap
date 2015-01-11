using UnityEngine;
using System.Collections;

public class SniperController : Controller {

	protected GameObject 		bill = null;
	protected bool				onScreen = false;
	protected float				angleBoundary;
	
	public SniperController (ContraEntity entity) : base(entity) 
	{
		bill = GameObject.FindGameObjectWithTag ("BillRizer");
		angleBoundary = 15f;
	}

	protected virtual bool canShoot(float angle) {
		return true;
	}

	protected virtual float adjustAngle(float angle) {
		var sniper = entity as Sniper;

		if (sniper.isWaterSniper) {
			if (angle > 90 && angle < 180) {
				angle = 90;
			}
			else if (angle >= 180 && angle < 270) {
				angle = 270;
			}
		}
		return angle;
	}

	public override void Run () {

		if (bill == null) {
			return;	
		}
		
		Vector2 upVector = new Vector2 (0, 1);
		
		var mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		Vector2 screenPos = mainCamera.camera.WorldToViewportPoint(entity.transform.position);
		
		entity.leftOrRight = (bill.transform.position.x < entity.transform.position.x) ? -1 : 1;
		entity.upOrDown = (bill.transform.position.y < entity.transform.position.y) ? 1 : -1; 

		Vector2 t_dir = bill.transform.position - entity.transform.position;
		
		float t_angle = Vector2.Angle (upVector, t_dir);
		float angle = (Mathf.Round(t_angle/ angleBoundary ) * angleBoundary);
		
		if (entity.leftOrRight == -1) {
			angle = 360 - angle;	
		}

		angle = adjustAngle (angle);

		var shootDirection = new Vector2(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));
		entity.dir = shootDirection.normalized;

		if (canShoot (angle)) {
			entity.Shoot ();
		} else {
			entity.dir = Vector2.zero;
		}
	}
}

