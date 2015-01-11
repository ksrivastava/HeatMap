using UnityEngine;
using System.Collections;

public class Gravity : MonoBehaviour {

	private float		gravityVal = -9f;
	public Vector2		vel = Vector2.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		vel += new Vector2(0, gravityVal) * Time.fixedDeltaTime;
		transform.position = (Vector2) transform.position + vel * Time.fixedDeltaTime;

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Lava") {
			Destroy (this.gameObject);		
		}
//		if (other.tag == "BillRizer") {
//			Bill bill = other.gameObject.GetComponent<Bill>(); 
//			if(bill != null) {
//				bill.Damage();
//			}
		//}
	}
}
