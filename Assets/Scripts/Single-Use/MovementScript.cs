using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour {

    public float speed = 0.5f;

    public bool movingRight = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float temp = speed * Time.deltaTime;

        if (movingRight)
            transform.Translate(temp, 0, 0);
        else
            transform.Translate(-temp, 0, 0);

        if(transform.position.x <= -14 || transform.position.x >= 14)
        {
            Destroy(this.gameObject);
        }
	}
}
