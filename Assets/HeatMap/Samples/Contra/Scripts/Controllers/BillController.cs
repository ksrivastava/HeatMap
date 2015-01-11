using UnityEngine;
using System.Collections;

public class BillController : Controller {
	
	private KeyCode		jumpKey = KeyCode.X;
	private KeyCode		shootKey = KeyCode.Z;
	private KeyCode		invincibleKey = KeyCode.G;
	private Bill		bill = null;


	public BillController (ContraEntity entity) : base(entity) 
	{
		bill = entity as Bill;
	}

	public override void Run () {

		float horizontalAxis = Input.GetAxisRaw ("Horizontal");
		float verticalAxis = Input.GetAxisRaw ("Vertical");
		Vector2 dir = new Vector2 (entity.leftOrRight, 0);
		
		if (horizontalAxis > 0) {
			entity.MoveRight();
			
		} else if (horizontalAxis < 0) {
			entity.MoveLeft();
		}


		dir.Set (entity.leftOrRight, 0);

		if (verticalAxis < 0) {
			if (horizontalAxis != 0) { //Moving horizontally
				dir.y = -1;
			}
			else if (!bill.onFloor && !bill.inWater) {
				dir.Set (0, -1);
			}
			else {
				entity.Crouch();
			}

			if(bill.inWater){
				entity.Crouch();
			}
		} 
		else {
			entity.Uncrouch();
		}

		if (verticalAxis > 0) {
			if (horizontalAxis != 0) {
				dir.y = 1;
			}
			else {
				dir.Set (0, 1);
			}
		}

		if (Input.GetKeyDown (jumpKey) ){
			//entity.Jump();
			bill.Jump (horizontalAxis);
		}


		if (Input.GetKeyDown (shootKey)) {
			// shoot logic
			//Debug.Log("Shoot!");
			entity.Shoot();
		} else if (Input.GetKey (shootKey)) {
			//Debug.Log("If i have an MGun, continuously shoot!");
			if(bill.gun.GetType().ToString() == "MGun"){
				entity.Shoot();
			}
		}

		if (Input.GetKeyDown (invincibleKey)) {
			bill.godMode = !bill.godMode;	
		}

		if (bill.isCrouched) {
			dir.y = 0;		
		}

	
		entity.dir = dir;
	}
}
