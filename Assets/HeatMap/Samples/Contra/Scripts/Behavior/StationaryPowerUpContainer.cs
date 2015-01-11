using UnityEngine;
using System.Collections;

public class StationaryPowerUpContainer : MonoBehaviour {

	public GameObject powerup; 
	public string gunType ;
	public bool isOpen = false;

	public Material openMaterial;
	public Material closedMaterial;

	public float 	timeBetweenSteps = 2f;
	public float 	lastStep = 0;
	public int 			leftOrRight = 1;

	// Use this for initialization
	void Start () {
		powerup = Resources.Load(this.gunType+"PowerUp") as GameObject;
	}
	
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Bullet" && isOpen) {

			Bullet bullet = other.gameObject.GetComponent<Bullet>();
			if(bullet.ownerTag == "BillRizer"){

				powerup.transform.position = this.transform.position;
				PowerUp p = powerup.transform.FindChild("PowerUp").gameObject.GetComponent<PowerUp>();
				p.gunType = this.gunType;
				p.leftOrRight = leftOrRight;
				Instantiate(powerup);
				Destroy(this.gameObject);
			}
		}
	}

	void FixedUpdate() {
		if (lastStep == 0) {
			lastStep = Time.time;
		}
		
		else if (Time.time - lastStep > timeBetweenSteps) {
			lastStep = Time.time;

			if(this.isOpen == false) {
				//Debug.Log ("Open"); 
				this.isOpen = true;
			} else if(this.isOpen == true){
				//Debug.Log("Closed");
				this.isOpen = false;
			}
		}

		//renderer.enabled = isOpen;

		if (isOpen) {
	
						renderer.material = openMaterial;
				} else {
			renderer.material = closedMaterial;
				}
	}
}
