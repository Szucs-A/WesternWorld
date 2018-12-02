using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject prefabSpawn;

    public float lowestYRange, highestYRange;

    public float starttime;
    public float TimeRange;

    public GameObject parents;

	// Use this for initialization
	void Start () {
        InvokeRepeating("spawn", starttime, TimeRange);	
	}

    public void spawn()
    {
        float addingRange = Random.Range(lowestYRange, highestYRange);

        Vector3 newPos = this.transform.position;
        newPos.y += addingRange;

        GameObject go = Instantiate(prefabSpawn, newPos, Quaternion.identity, null);

        if (parents != null)
            go.transform.SetParent(parents.transform);
    }
}
