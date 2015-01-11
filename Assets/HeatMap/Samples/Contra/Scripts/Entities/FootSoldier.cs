using UnityEngine;
using System.Collections;

public class FootSoldier : ContraEntity {

	private bool 		onFloor = false;
	private Vector2		vel = Vector2.zero;
	private float 		xSpeed = 1f;
	private float 		jumpVal = 7.5f;
	private int			billDirection;
	private bool 		onStartOfBridge;
	public int 			touchingBridges = 0;
	private bool 		onBridge = false;
	void Start () {
		var bill = GameObject.FindGameObjectWithTag ("BillRizer");
		billDirection = (bill.transform.position.x < this.transform.position.x) ? -1 : 1; 
	}


	// Update is called once per frame
	void FixedUpdate () {
		if (!onFloor) {
			// Apply gravity and acc to vel
			vel += (Vector2)Physics.gravity * Time.fixedDeltaTime;
			
			// Apple vel to position
			transform.position = (Vector2)transform.position + vel * Time.fixedDeltaTime;
		} else {
			if(billDirection < 0) MoveLeft();
			else MoveRight();
		}
	}

	public override void MoveLeft() {
		vel = new Vector2(-1f*xSpeed, 0f);	
		transform.position = (Vector2)transform.position + vel * Time.fixedDeltaTime;
	}
	
	public override void MoveRight() {
		vel = new Vector2(xSpeed, 0f);	
		transform.position = (Vector2)transform.position + vel * Time.fixedDeltaTime;
	}
	
	public void Jump() {
		if (onFloor) {
			onFloor = false; //you're going to leave the floor either way
			Vector2 jumpForce = new Vector2(0f, jumpVal);
			vel += (Vector2)jumpForce;
			transform.position =  (Vector2)transform.position +vel*Time.deltaTime;
		}
	}
	

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Floor" || other.tag == "Bridge") {
			
			if( (transform.position.y) < other.bounds.max.y){
				return;
			}
			onFloor = true;
			if (other.tag == "Bridge") touchingBridges++;

			Vector2 pos = transform.position;
			pos.y = other.bounds.max.y + transform.localScale.y / 2; 
			
			transform.position = pos;

		} else if (other.tag == "BillRizer") {
			Bill bill = other.gameObject.GetComponent<Bill>(); 
			bill.Damage();
		}
		
	}

	void OnTriggerExit2D (Collider2D other){
		if (other.tag == "Floor") {
			Jump();
		} else if (other.tag == "Bridge") {
			touchingBridges--;
			if (touchingBridges == 0) {
				onFloor = false;
			}
		} else if (other.tag == "Water") {
			this.Damage();
		}
	}
	
	public override void Damage(float damageTaken = 0) {
		Destroy (this.gameObject);
	}
}
