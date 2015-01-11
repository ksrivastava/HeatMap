using UnityEngine;
using System.Collections;

public class CameraTracking : MonoBehaviour {

	public GameObject	player;
	public GameObject	boundary;
	public GameObject	marker;
	public float		screenWidth;
	public float		rightEdge;
	public float		cameraSpeed = 5f;


	// Use this for initialization
	void Start () {
		screenWidth = (this.camera.orthographicSize * 2f * this.camera.aspect);
		rightEdge = boundary.renderer.bounds.max.x - 3;
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) {
			return;
		}

		Vector3 pos = transform.position;

		if (pos.x >= marker.transform.position.x) {
			if (pos.x + screenWidth / 2 < rightEdge) {
				pos.x += cameraSpeed * Time.deltaTime;
				transform.position = pos;
			}
		}

		else {
			pos.x = player.transform.position.x;
			if (pos.x >= transform.position.x) {
				transform.position = pos;
			}
		}
	}
}
