using UnityEngine;
using System.Collections;

public abstract class Gun : MonoBehaviour {

	protected float	lastStep;
	protected int		bulletCount = 0;
	public float 	timeBetweenSteps = 2f;
	protected	int		numMaxBullets = 4;
	public GameObject   	bulletPrefab;

	public ContraEntity		entity;
	
	public virtual void Shoot(){}
}
