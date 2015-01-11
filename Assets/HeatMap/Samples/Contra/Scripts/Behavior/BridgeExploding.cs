using UnityEngine;
using System.Collections;

public class BridgeExploding : MonoBehaviour {
	
	Bill bill;
	public bool billOnBridge = false;

	void Start() {
		GameObject billObject = GameObject.FindGameObjectWithTag("BillRizer");
		bill = billObject.GetComponent<Bill>();
	}

	void Update() {
		if (bill != null && shouldExplode ()) {
			StartCoroutine(Explode());
		}
	}

	bool shouldExplode() {
		float min_x = transform.position.x-3f ;
		return min_x < bill.renderer.bounds.max.x;
	}

	IEnumerator Explode() {
		yield return new WaitForSeconds(0.7f);
		ExplodeHelper ();
	}

	void ExplodeHelper() {
		Destroy(this.gameObject);
		if (billOnBridge) {
			bill.onFloor = false;
		}
	}

	void OnTriggerStay2D (Collider2D other){
		if (other.tag == "BillRizer") {
			billOnBridge = true;
		}
	}

	void OnTriggerExit2D (Collider2D other){
		if (other.tag == "BillRizer") {
			billOnBridge = false;
		}
	}
	
}
