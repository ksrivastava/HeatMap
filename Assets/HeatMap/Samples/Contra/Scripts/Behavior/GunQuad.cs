using UnityEngine;
using System.Collections;

public class GunQuad : MonoBehaviour {

	ContraEntity parentEntity;
	private int leftOrRight;
	void Awake () {
		parentEntity = this.transform.parent.gameObject.GetComponent<ContraEntity> ();
	}

	void Update () {

	}
}
