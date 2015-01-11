using UnityEngine;
using System.Collections;

public class VerticalCameraTracking : MonoBehaviour {
	
	private GameObject	player;
	private GameObject	boss;
	public GameObject	marker;
	public GameObject	boundaryMarker;
	public float		screenWidth;
	public float		topEdge;
	public bool 		bossInView;
	public bool 		finalScene = false;
	public Color 		color1;
	public Color 		color2;
	private float 		duration = 5f;
	private float 		deltaTime = 0.0f;
	public float		cameraSpeed = 5f;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("BillRizer");
		deltaTime = 0.0f;
		screenWidth = (this.camera.orthographicSize * 2f * this.camera.aspect);
		topEdge = boundaryMarker.transform.position.y;

		boss = GameObject.Find ("Boss");
		// TODO enable boss when bill hits marker
		bossInView = false;
	}

	void showBoss() {

		BossScript bossScript = (BossScript) boundaryMarker.GetComponent(typeof(BossScript));
		if (!bossScript.isDead) bossScript.isActive = true;
	}

	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;

		var bottomLeft = this.camera.ViewportToWorldPoint (new Vector2(0, 0));
		if (player != null && player.renderer.bounds.min.y <= bottomLeft.y) {
			Bill bill = player.GetComponent<Bill>();
			bill.Damage(-1);
		}

		deltaTime += Time.deltaTime;
		if(deltaTime < duration)
		{
			camera.backgroundColor = Color.Lerp(color1, color2, deltaTime/duration);
			
		}
		else { 
			deltaTime = 0.0f; 
			
			var _t = color1;
			color1 = color2;
			color2 = _t;
		}
		
		if (player == null) {
			return;
		}

		if (finalScene) {
			if (pos.y + screenWidth / 2 < topEdge) {
				pos.y += cameraSpeed * Time.deltaTime;
				transform.position = pos;
			}
			else {
				showBoss ();
			}

			var topRight = this.camera.ViewportToWorldPoint (new Vector2(1, 1));
			if (player.transform.position.y >= topRight.y) {
				Application.LoadLevel("Final");
			}


		}
		else {
			if (player.transform.position.y + 1f >= marker.transform.position.y) {
				finalScene = true;
			}
			
			else {
				pos.y = player.transform.position.y;
				if (pos.y >= transform.position.y) {
					transform.position = pos;
				}
			}
		}
	}

	public void Center() {
		if (player == null || finalScene) {
			return;
		}
		Vector3 pos = transform.position;
		pos.y = player.transform.position.y;
		transform.position = pos;
	}
	
}
