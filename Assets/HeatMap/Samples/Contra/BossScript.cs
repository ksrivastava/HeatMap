using UnityEngine;
using System.Collections;

public class BossScript : MonoBehaviour {

	public bool 		isActive = false;
	public bool 		isDead = false;
	private GameObject	player;
	private float 		duration = 2f;
	private float 		deltaTime = 0.0f;
	private GameObject 	flamePrefab;
	public float		screenWidth;
	
	// Use this for initialization
	void Start () {
		deltaTime = 0.0f; 
		player = GameObject.FindGameObjectWithTag ("BillRizer");
		flamePrefab = Resources.Load ("FallingFlameBall") as GameObject;
		screenWidth = (Camera.main.camera.orthographicSize * 2f * Camera.main.camera.aspect);
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive) {
			deltaTime += Time.deltaTime;
			if(deltaTime > duration)
			{
				GameObject f = Instantiate(flamePrefab) as GameObject;
				Vector3 pos = this.transform.position;
				pos.x = player.transform.position.x + Random.Range(-2f, 2f);
				pos.y += 1f;
				f.transform.position = pos;
				deltaTime = 0.0f; 
			}
		}
	}
}
