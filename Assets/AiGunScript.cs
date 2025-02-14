﻿using UnityEngine;
using System.Collections;

public class AiGunScript : MonoBehaviour {

	[HideInInspector]
	public GunSharedValues val;

	public float accuracy = 0.5f;

	public int damage = 5;
	public int fireRate = 600; //BPM

	public int minBulletsPerVolley = 4;
	public int maxBulletsPerVolley = 6;
	public float minTimeBetweenVolleys = 0.5f;
	public float maxTimeBetweenVolleys = 1f;

	//public Transform barrelPoint;
    [HideInInspector]
	public Transform player;

	public float rotSpeed = 10f;
	public float shootDistance = 30f;
	public bool isInRange = false;
	// Use this for initialization
	void Start () {

		if (val == null)
			val = GetComponent<GunSharedValues> ();

		player = GameObject.FindGameObjectWithTag ("Player").transform;

		//ShootRoutine ();
	}
	
	// Update is called once per frame
	void Update () {
	
		Quaternion lookRotation = Quaternion.LookRotation (player.position - transform.position);

		transform.rotation = Quaternion.RotateTowards (transform.rotation, lookRotation, (float)rotSpeed * (float)Time.deltaTime);

		//print (Vector3.Distance (transform.position, player.position));

		if (Vector3.Distance (transform.position, player.position) > shootDistance) {
			//print ("out of range");
			CancelInvoke ();
			StopAllCoroutines ();
			isInRange = false;
		} else if(!isInRange){

			isInRange = true;
			//print ("im in range");
			Invoke ("ShootRoutine", Random.Range ((float)minTimeBetweenVolleys, (float)maxTimeBetweenVolleys));

		}

	}

	void ShootRoutine(){

		StartCoroutine ("Volley", Random.Range(minBulletsPerVolley, maxBulletsPerVolley));

		Invoke ("ShootRoutine", Random.Range ((float)minTimeBetweenVolleys, (float)maxTimeBetweenVolleys));

	}

	IEnumerator Volley(int bulletCount){

		for (int i = 0; i <= bulletCount; i++) {
			val.damage = (int)Random.Range ((float)damage * 0.8f, (float)damage * 1.2f);
			BroadcastMessage ("Shoot");
			//print (i);
			yield return new  WaitForSeconds (60f/(float)fireRate);
		}
	}
}
