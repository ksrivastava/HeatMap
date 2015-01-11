using UnityEngine;
using System.Collections;

public class FallingFlameBallController : MonoBehaviour {

	public bool 		isActive = false;
	private float 		duration = 1f;
	private float 		deltaTime = 0.0f;
	private GameObject 	flamePrefab;
	public float		screenWidth;

	// Use this for initialization
	void Start () {
		deltaTime = 0.0f; 
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
				pos.x = Random.Range(pos.x - screenWidth/2, pos.x + screenWidth/2);
				pos.y += 1f;
				f.transform.position = pos;
				deltaTime = 0.0f; 
			}
		}
	}
}
