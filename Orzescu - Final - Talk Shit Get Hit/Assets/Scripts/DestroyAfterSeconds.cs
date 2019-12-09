using UnityEngine;
using System.Collections;

public class DestroyAfterSeconds : MonoBehaviour {

    public float secondsTillDestroy;
    private float timer = 0;

	// Use this for initialization
	void Start () {

        timer = Time.time;

	}
	
	// Update is called once per frame
	void Update () {

        if (Time.time - timer > secondsTillDestroy)
        {
            
        }

	}
}
