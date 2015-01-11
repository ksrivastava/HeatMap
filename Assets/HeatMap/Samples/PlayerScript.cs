using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	HeatMap heatmap;
	HeatTag progressionTag, deathTag;
	string url = "http://iamkos.com/heatmap.php";
	public bool ShowHeatMap = false;
	public bool TrackPlayer = false;
	public bool TrackDeath = false;
	public float ProgressionMapColorDelta = 0.2f;
	public float speed = 4f;
	private Vector2 dir;

	// Use this for initialization
	void Start () {
		Respawn ();
		heatmap = GetComponent<HeatMap> ();

		progressionTag = new HeatTag ("Sample Progression 2", url);
		progressionTag.MapPointColorDelta = ProgressionMapColorDelta;

		deathTag = new HeatTag ("Sample Death 2", url);
		deathTag.Type = HeatTag.HeatType.POINT;

		if (TrackPlayer) {
			heatmap.TrackPlayer (this.gameObject, progressionTag, 0.2f);
		}

		if (ShowHeatMap) {
			heatmap.PlotData(progressionTag);
			heatmap.PlotData(deathTag);
		}
	}

	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		pos.x += dir.x * speed * Time.deltaTime;
		pos.y += dir.y * speed * Time.deltaTime;

		transform.position = pos;
	}

	void Respawn() {
		transform.position = Vector3.zero;
		dir = new Vector2 (Random.Range (-3f, 3f), Random.Range (-3f, 3f));
		dir.Normalize ();
	}

	void OnTriggerEnter(Collider other) {
		if (TrackDeath) heatmap.Post (transform.position, deathTag);
		Respawn ();
	}
}
