using UnityEngine;
using System.Collections;

public class Controller {

	protected ContraEntity		entity;

	public Controller (ContraEntity entity) {
		this.entity = entity;
	}

	public virtual void Run () {}
}
