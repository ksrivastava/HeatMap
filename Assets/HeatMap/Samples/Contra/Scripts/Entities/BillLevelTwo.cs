using UnityEngine;
using System.Collections;

public class BillLevelTwo : Bill {
	
	protected override void Respawn() {
		base.Respawn ();

		VerticalCameraTracking cameraScript = (VerticalCameraTracking) Camera.main.GetComponent(typeof(VerticalCameraTracking));
		cameraScript.Center ();

		ResetLava ();
	}

	void ResetLava() {
		GameObject lavaObj = GameObject.FindGameObjectWithTag ("Lava");
		Lava lavaScript = (Lava) lavaObj.GetComponent(typeof(Lava));
		lavaScript.ResetPosition ();
	}
}
