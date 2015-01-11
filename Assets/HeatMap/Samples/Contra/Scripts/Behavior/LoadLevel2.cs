using UnityEngine;
using System.Collections;

public class LoadLevel2 : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "BillRizer" && this.renderer.enabled == true) {
			Application.LoadLevel("Cutscene");
		}
	}
}
