using UnityEngine;
using System.Collections;

public class FlameBall : ContraEntity {

	private float timeBetweenSteps = 1.5f;
	private float lastStep = 0;
	private bool flameOn = false;
	public bool isBoss = false;

	void Start () {
		if(this.health == 0)
		this.health = 10;
	}

	void Update () {
	
	}

	void FixedUpdate(){
		if (CanShoot ()) {
			RenderFlames(flameOn);
			flameOn = ! flameOn;
		}
	}

	private bool CanShoot() {
		if (lastStep == 0) {
			lastStep = Time.time;
		}
		
		else if (Time.time - lastStep > timeBetweenSteps) {
			lastStep = Time.time;
			return true;
		}
		
		return false;
	}

	private void RenderFlames (bool renderFlag)
	{
		GameObject parent = transform.parent.gameObject;
		var Flames = parent.GetComponentsInChildren<Flame>();
		
		foreach (Flame flame in Flames) {
			if(renderFlag)
				flame.StartFlame();
			else
				flame.StopFlame();
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		Debug.Log ("FlameBall collided with "+other.tag);
	}

	public override void Damage (float damageTaken)
	{
		this.health--;
		if (this.health == 0) {
			GameObject parent = transform.parent.gameObject;
			if(isBoss){
				BossScript bossScript = (BossScript) parent.transform.parent.GetComponent(typeof(BossScript));
				bossScript.isDead = true;
				bossScript.isActive = false;
				var stairway = GameObject.FindGameObjectWithTag("NextLevel");
				stairway.transform.GetChild(0).transform.GetChild(0).renderer.enabled = true;
				stairway.transform.GetChild(1).transform.GetChild(0).renderer.enabled = true;
				stairway.transform.GetChild(2).transform.GetChild(0).renderer.enabled = true;
			}
			Destroy(parent);
		}
	}
}
