using UnityEngine;
using System.Collections;

public class BossPanel : ContraEntity {

	// Use this for initialization
	void Start () {
		this.health = 7;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Damage(float damageTaken = 0) {
		Debug.Log ("Took Damage!");
		this.health--;
		if (health <= 0) {
			var nextLvlStairway = GameObject.FindGameObjectWithTag("NextLevel");
			nextLvlStairway.transform.GetChild(0).renderer.enabled = true;
			Destroy(this.gameObject);
		}
	}
}
