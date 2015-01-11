using UnityEngine;
using System.Collections;

public class MovingFloorSegment : MonoBehaviour {
	
	public float baseX;
	public float amplitude = 1f;
	public float frequency = 1f;
	public bool billOnPlatform = false;

	public int initialDirection = 1; //1 is right, -1 is left
	void Awake(){
		baseX = transform.position.x;
	}
	
	void FixedUpdate(){
		Vector2 pos = transform.position;
		
		pos.x = baseX + initialDirection*Mathf.Sin (Time.time * frequency) * amplitude;
		
		if (billOnPlatform) {
			GameObject billObject = GameObject.FindGameObjectWithTag ("BillRizer");
			Bill bill = billObject.GetComponent<Bill>();
			if(!bill.isFallingThrough){
				Vector2 billPos = billObject.transform.position;
				
				var delta = billPos.x - transform.position.x;
				billPos.x = pos.x + delta;
				
				billObject.transform.position = billPos;
			}
		}
		
		transform.position = pos;
	}

	public void BillIsDead() {
		billOnPlatform = false;
	}

	void OnTriggerExit2D (Collider2D other)
	{
		Debug.Log ("TriggerExit");
		if (other.tag == "BillRizer") {
			billOnPlatform = false;
		}
	}
	
	void OnTriggerStay2D(Collider2D other){
		// && other.renderer.bounds.min.y >= this.renderer.bounds.max.y
		if (other.tag == "BillRizer" && !billOnPlatform) {
			billOnPlatform = true;
		}
	}
}
