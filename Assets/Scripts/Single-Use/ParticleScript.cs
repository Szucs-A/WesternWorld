using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PlayParticles();
        Destroy(this.gameObject, 5f);
    }
	
    public void PlayParticles()
    {
        this.GetComponent<ParticleSystem>().Play();
    }
}
