using UnityEngine;
using System.Collections;

public class BillSpawner : MonoBehaviour {

	public float leftBoundary = 1.6f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		var vertExtent = Camera.main.camera.orthographicSize;   
		var horzExtent = vertExtent * Screen.width / Screen.height;
		pos.x = Camera.main.transform.position.x - horzExtent + leftBoundary;
		pos.x = Mathf.Max (transform.position.x, pos.x);
		transform.position = pos;
	}
}
