using UnityEngine;
using System.Collections;

public class GunVisuals : MonoBehaviour {

	public GameObject left;
	public GameObject right;
	public GameObject topLeft;
	public GameObject topRight;
	public GameObject bottomLeft;
	public GameObject bottomRight;
	public GameObject up;
	public GameObject down;

	private Vector2 TL;
	private Vector2 TR;
	private Vector2 BL;
	private Vector2 BR;
	private Vector2 L;
	private Vector2 R;
	private Vector2 U;
	private Vector2 D;

	private ContraEntity entity;

	void Awake(){
		RenderersOff ();
	}

	// Use this for initialization
	void Start () {
		RenderersOff ();
		this.entity = this.transform.parent.gameObject.GetComponent<ContraEntity> ();
			
			L = new Vector2(-1,0).normalized;
			R = new Vector2(1,0).normalized;
			TL = new Vector2(-1,1).normalized;
			TR = new Vector2(1,1).normalized;
			BL = new Vector2(-1,-1).normalized;
			BR = new Vector2(1,-1).normalized;
			U = new Vector2(0,1).normalized;
			D = new Vector2(0,-1).normalized;
	}

	void Update(){
		if (this.transform.parent.gameObject.renderer.enabled == false) {
			RenderersOff();		
			return;
		}
		this.transform.position = entity.transform.position;
		UpdateVisual ();
	}

	public void UpdateVisual(){
		if (entity.dir == Vector2.zero)
						return;
		RenderersOff ();

		float angle = Vector2.Angle (Vector2.up, entity.dir.normalized);
		angle = (Mathf.Round(angle/ 45f ) * 45f);
		if (entity.leftOrRight == -1) {
			angle = 360 - angle;	
		}

		var shootDirection = new Vector2(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));
		shootDirection.Normalize ();
		float mag = shootDirection.magnitude;


		Vector2 dir = shootDirection;
		if (dir == L) { //L
				left.renderer.enabled = true;
		} else if (dir == R) { //R
				right.renderer.enabled = true;
		} else if (dir == TL) { //TL
				topLeft.renderer.enabled = true;
		} else if (dir == TR) { // TR
				topRight.renderer.enabled = true;
		} else if (dir == BL) { // BL
				bottomLeft.renderer.enabled = true;
		} else if (dir == BR) { //BR
				bottomRight.renderer.enabled = true;
		} else if (dir == U) {//UP
				up.renderer.enabled = true;
		} else if (dir == D) {//DOWN
				down.renderer.enabled = true;
		}
	}

	private void RenderersOff(){
		left.renderer.enabled = false;
		right.renderer.enabled = false;
		topLeft.renderer.enabled = false;
		topRight.renderer.enabled = false;
		bottomLeft.renderer.enabled = false;
		bottomRight.renderer.enabled = false;
		up.renderer.enabled = false;
		down.renderer.enabled = false;
	}
}
