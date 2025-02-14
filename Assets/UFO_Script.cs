﻿using UnityEngine;
using System.Collections;

public class UFO_Script : MonoBehaviour {

    public bool ShouldKillOutsidePlayer = true;

	public Vector2 xRange;
	public Vector2 zRange;

	public float speed = 0.5f;

	public Vector2 areaChangeTime;

	public Vector3 areaToGo = new Vector3 (0, 0, 0);
    public bool didReachDestination;
    public float threshold = 5;
    public int minDistance = 60;

    public bool shouldPickNewLocation = true;

	// Use this for initialization
	void Start () {
        //InvokeRepeating ("PickArea", areaChangeTime / 2, areaChangeTime);
        Invoke("PickArea", 30);
	}
	
	// Update is called once per frame
	void Update () {

		transform.position = Vector3.MoveTowards (transform.position, areaToGo, speed * Time.deltaTime);

        if (!shouldPickNewLocation)
            return;

        if (!didReachDestination)
        {
            if(Vector3.Distance(transform.position, areaToGo) < threshold)
            {
                didReachDestination = true;
                Invoke("PickArea", Random.Range(areaChangeTime.x, areaChangeTime.y));
            }

        }
        else
        {
            if (Vector3.Distance(transform.position, areaToGo) > threshold)
            {
                didReachDestination = false;
            }
        }
	
	}

	void PickArea (){

        Vector3 lastAreaToGo = areaToGo;

        while(Vector3.Distance(lastAreaToGo, areaToGo) < minDistance)
        {
            areaToGo = new Vector3(
                        Random.Range(xRange.x, xRange.y),
                        0,
                        Random.Range(zRange.x, zRange.y)
                    );
        }

	}

	void OnTriggerExit (Collider col){

		if(col.gameObject.tag == "Player" && ShouldKillOutsidePlayer)
        {
			print ("cant escape");
			col.gameObject.GetComponentInParent<Health> ().Damage (9000, transform);
			StartCoroutine ("aSecondLater", col);
		}
	}

	IEnumerator aSecondLater (Collider col){
		yield return null;
		yield return null;
		if(col.gameObject.GetComponentInParent<Health> ().isAlive == true)
			col.gameObject.GetComponentInParent<Health> ().AllDie ();
	}

    public void lastArea()
    {
        areaToGo = new Vector3(-148.6f, 0f, -35.3f);
        shouldPickNewLocation = false;
        CancelInvoke();
    }
}
