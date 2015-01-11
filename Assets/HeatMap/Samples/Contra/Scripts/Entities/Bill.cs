using UnityEngine;
using System.Collections;

public class Bill : ContraEntity {
	private float 		leftBoundary = 1.2f;
	private float 		rightBoundary = 0f;
	public bool 		isFallingThrough;
	public bool 		isCrouched;
	public bool 		inWater;
	public bool 		onFloor;
	public bool 		onBridge;
	private float 		xSpeed = 0.06f;
	public float 		jumpVal = 10.5f;
	public Vector2		vel = Vector2.zero;
	public GameObject 	spawner;
	public float		gravityVal = -18f;
	public bool			invincibleFlag = false;
	private int			invincibleSeconds = 2;	
	public Gun 			gun;
	public bool 		isOnWaterFloor;
	public	bool		godMode = false;
	public bool 		isJumping = false;
	public float 		startingHeight;
	public float 		startingWidth;
	public bool 		stopMoving = false;
	public enum BillState {Normal, CrouchedOnLand, CrouchedInWater, InWater, Jumping, Null};
	public BillState 		currentState = BillState.Null;
	private GUIText 		billHealth;

	HeatMap 	heatmap;
	HeatTag progressionTag, deathTag;
	string url = "http://iamkos.com/heatmap.php";
	public bool ShowHeatMap = false;
	public bool TrackPlayer = false;
	public bool TrackDeath = false;
	public float ProgressionMapColorDelta = 0.2f;

	// Use this for initialization
	protected virtual void Start () {

		this.gun = new BasicGun(this);
		controller = new BillController (this);
		leftOrRight = 0;
		health = 3;

		billHealth = GameObject.Find ("BillHealth").GetComponent<GUIText>();

		if (godMode) health = 1000;

		startingHeight = renderer.bounds.size.y;
		startingWidth = renderer.bounds.size.x;

		if (Application.loadedLevelName == "Level_2") {
			leftBoundary = 0.8f;
			rightBoundary = 0.8f;
		}

		Respawn ();

		if (Application.loadedLevelName == "Level_1") {

						heatmap = GetComponent<HeatMap> ();

						string p_string = "Contra Bill Progresssion Level 1";	
						string d_string = "Contra Bill Death Level 1";

						progressionTag = new HeatTag ("p_string", url);
						progressionTag.SetSorted ();
						progressionTag.MapPointColorDelta = ProgressionMapColorDelta;
		
						deathTag = new HeatTag ("d_string", url);
						deathTag.Type = HeatTag.HeatType.POINT;
		
						if (TrackPlayer) {
								heatmap.TrackPlayer (this.gameObject, progressionTag, 0.2f);
						}
		
						if (ShowHeatMap) {
								heatmap.PlotData (progressionTag);
								heatmap.PlotData (deathTag);
						}
				}


	}
	
	// Update is called once per frame
	void Update () {
		controller.Run ();
	}


	// Update is called once per frame
	void FixedUpdate () {	
		if (!onFloor && !inWater) {
			// Apply gravity and acc to vel
			vel += new Vector2(0,gravityVal) * Time.fixedDeltaTime;
			
			// Apple vel to position
			transform.position = (Vector2) transform.position + vel * Time.fixedDeltaTime;

		} else {
			vel = Vector2.zero;
		}
		
	}

	public override void MoveLeft() {
		if (!isFallingThrough && !isCrouched && !stopMoving) {
			leftOrRight = -1;
			Vector2 pos = transform.position;
			pos.x -= xSpeed;
			
			var vertExtent = Camera.main.camera.orthographicSize;
			var horzExtent = vertExtent * Screen.width / Screen.height;
			
			if (pos.x >= (Camera.main.transform.position.x - horzExtent + leftBoundary)) {
				transform.position = pos;
			}
		}
	}

	public override void MoveRight() {
		if (!isFallingThrough && !isCrouched && !stopMoving) {
			leftOrRight = 1;
			Vector2 pos = transform.position;
			pos.x += xSpeed;

			var vertExtent = Camera.main.camera.orthographicSize;
			var horzExtent = vertExtent * Screen.width / Screen.height;
			
			if (pos.x <= (Camera.main.transform.position.x + horzExtent - rightBoundary)) {
				transform.position = pos;
			}
		}
	}

	public  void Jump(float horizontalAxisInputValue) {

		if (!inWater && (onFloor)) {
			if (isCrouched) {
				if (canFallThrough()) {
					FallThrough();
				}
			}
			else {
				PerformJump(horizontalAxisInputValue);
			}
		}
	}

	private void PerformJump(float horizontalAxisInputValue) {
		// ScaleDown ();
			SetState (BillState.Jumping);
		Vector2 jumpForce = new Vector2(0f, jumpVal);
		vel += (Vector2)jumpForce;
		transform.position =  (Vector2)transform.position +vel*Time.deltaTime;
	}

	public override void Crouch() {
		if (!isCrouched && !isFallingThrough && (onFloor || inWater)) {
			if (inWater) {
				SetState (BillState.CrouchedInWater);
			}
			else {
				SetState (BillState.CrouchedOnLand);
			}

			isCrouched = true;
		}
	}

	public override void Uncrouch() {
		if (isCrouched) {
			if (inWater) {
				SetState (BillState.InWater);
			}
			else {
				SetState (BillState.Normal);
			}
			isCrouched = false;
		}
	}

	private void SetState (BillState state) {

		switch (state) {
		case BillState.Normal:
			onFloor = !isFallingThrough;
			inWater = false;
			isJumping = false;
			break;
		case BillState.CrouchedOnLand:
			isCrouched = true;
			break;
		case BillState.CrouchedInWater:
			isCrouched = true;
			break;
		case BillState.InWater:
			inWater = true;
			onFloor = false;
			isFallingThrough = false;
			isCrouched = false;


			if (currentState != BillState.CrouchedInWater) {
				stopMoving = true;
				Invoke("StartMovingInWater", 0.5f);
				SetSize (BillState.CrouchedInWater);
			}
			break;
		case BillState.Jumping:
			onFloor = false;
			isJumping = true;
			isCrouched = false;
			break;
		}

		if (!stopMoving) SetSize (state);
		currentState = state;
	}

	private void StartMoving() {
		stopMoving = false;
	}

	private void StartMovingInWater() {
		stopMoving = false;
		SetSize (BillState.InWater);
	}

	private void SetSize(BillState state) {
		float t_height = startingHeight;
		float t_width = startingWidth;

		switch (state) {
			case BillState.Normal:
				break;
			case BillState.CrouchedOnLand:
				t_height = startingHeight / 4f;
				t_width = startingWidth * 2.8f;
				break;
			case BillState.CrouchedInWater:
				t_height = startingHeight / 10f;
				break;
			case BillState.InWater:
				t_height = startingHeight / 2f;
				break;
			case BillState.Jumping:
				t_height = startingHeight / 2f;
				break;
		}

		SetSizeHelper(t_height, t_width);
	}

	private void SetSizeHelper(float height, float width) {
		float currentHeight = renderer.bounds.size.y;
		float currentWidth = renderer.bounds.size.x;

		Vector3 scale = transform.localScale;
		var pos = transform.position;

		if (currentHeight != height) {
			pos.y += (height - scale.y)/2;
			scale.y = height;
		}

		if (currentWidth != width) {
			scale.x = width;
		}

		transform.localScale = scale;
		transform.position = pos;
	}
	

	public override void FallThrough() {
		isFallingThrough = true;
		onFloor = false;
		Uncrouch();
	}

	private bool canFallThrough() {
		if (isOnWaterFloor) {
			return false;	
		}

		if (!onFloor) {
			return false;
		}
		Vector2 pointA = (Vector2) renderer.bounds.min;

		Vector2 pointB = (Vector2) renderer.bounds.max;
		var vertExtent = Camera.main.camera.orthographicSize;
		pointB.y = -vertExtent;

		Collider2D[] others = Physics2D.OverlapAreaAll(pointA, pointB);

		bool haveSeenMyFloor = false;

		foreach (var other in others) {
			if (other.tag == "Floor" && other.enabled) {
				if (haveSeenMyFloor) {
					return true;
				}
				else {
					haveSeenMyFloor = true;
				}
			}
			else if (other.tag == "Water") {
				return true;
			}
		}

		return false;
	}
	
	public override void Shoot() {
		if (!(inWater && isCrouched)) {
			gun.Shoot ();
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Floor" || other.tag == "Bridge") {

			//FIX FOR BILL LAND TIME COINCIDING WITH KEY PRESS TIME
			if(this.vel.y < 0){
				this.vel.y = 0;
			}
			//----------

			isOnWaterFloor = false;

			GameObject floor = other.gameObject;
			Floor floorScript = (Floor) floor.GetComponent(typeof(Floor));
			isOnWaterFloor = floorScript.isWaterFloor;

				if(renderer.bounds.min.x > other.bounds.max.x || renderer.bounds.max.x < other.bounds.min.x){
					if (isOnWaterFloor == false) {
						return;
					}
				}

				if ((transform.position.y) < other.bounds.max.y ) {
					if (!inWater) return;
			}

			onFloor = true;
			isFallingThrough = false;

			if (isOnWaterFloor && currentState == BillState.InWater) {
				stopMoving = true;
				StartCoroutine(ClimbOnToLand(other));
			}
			else {
				OnLand (other);
			}

		} else if (other.tag == "Water") {

			SetState (BillState.InWater);

			Vector2 pos = transform.position;
			pos.y = other.bounds.max.y + transform.localScale.y / 2; 	
			transform.position = pos;

		}
		else if (other.tag == "Enemy") {
			this.Damage ();
		}
	}

	IEnumerator ClimbOnToLand(Collider2D other) {
		yield return new WaitForSeconds(0.5f);
		stopMoving = false;
		OnLand (other);
	}

	private void OnLand(Collider2D other) {
		SetState (BillState.Normal);
		Vector2 pos = transform.position;
		pos.y = other.bounds.max.y + transform.localScale.y / 2; 
		transform.position = pos;
	}

	void OnTriggerExit2D (Collider2D other){
		if (other.tag == "Floor" || other.tag == "Bridge") {
			onFloor = onBridge || false;
			isOnWaterFloor = false;
		}
		else if (other.tag == "Water") {
			if (inWater) SetState (BillState.Normal);
			inWater = false;
		}
	}

	protected virtual void Respawn() {
		vel = Vector2.zero;
		onFloor = false;
		currentState = BillState.Null;
		SetState (BillState.Jumping);
		if (inWater) {
			// ScaleUp();
			inWater = false;
		}

		transform.position = spawner.transform.position;

		leftOrRight = 1;
		StartCoroutine(Blink(invincibleSeconds));
		gun = new BasicGun (this);
	}

	IEnumerator Blink(float blinkTime) {
		invincibleFlag = true;
		var endTime = Time.time + blinkTime;
		while(Time.time < endTime){
			renderer.enabled = false;
			yield return new WaitForSeconds(0.2f);
			renderer.enabled = true;
			yield return new WaitForSeconds(0.2f);
		}
		invincibleFlag = false;
	}

	public override void Damage(float damageTaken = 0) { // -1 means fell off screen, -2 means lava

		bool isUnderWater = (inWater && isCrouched);
		bool fellOffInGodMode = (damageTaken == -1 || damageTaken == -2) && (godMode || invincibleFlag);

		if (isUnderWater || ( (invincibleFlag || godMode) && !fellOffInGodMode)) {
			return;
		}

		MovingFloorSegment[] movingFloors = FindObjectsOfType(typeof(MovingFloorSegment)) as MovingFloorSegment[];
		foreach (MovingFloorSegment floor in movingFloors) {
			MovingFloorSegment floorScript = (MovingFloorSegment) floor.GetComponent(typeof(MovingFloorSegment));
			floorScript.BillIsDead ();
		}

		//Debug.Log("Dead!!");
		// Do death animation
		if (fellOffInGodMode) {
			Respawn ();
			return;
		}
		else {
			health--;

			if (Application.loadedLevelName == "Level_1") {

				if (TrackDeath) heatmap.Post (transform.position, deathTag);

			}

			string healthString = "";

			if (health == 1f) healthString = "O";
			else if (health == 2f) healthString = "O O";

			billHealth.text = healthString;
		}

		if (health > 0) {
			Respawn ();
		}
		else {
			Debug.Log("Game Over");
			Destroy	(gameObject);
			Application.LoadLevel(Application.loadedLevel);
		}
	}

	public void PowerUp(string powerUpType){
		if (powerUpType == "SGun") {
				this.gun = new SGun (this);
		} else if (powerUpType == "MGun") {
				this.gun = new MGun (this);
		} else if (powerUpType == "LGun") {
				this.gun = new LGun (this);
		} else if (powerUpType == "FGun") {
				this.gun = new FGun (this);
		} else if (powerUpType == "R") {
			this.gun.timeBetweenSteps = this.gun.timeBetweenSteps*0.7f;
		}
	}
}
