using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUNCameraMovement : MonoBehaviour {

    public GameObject topParent, bottomParent;

    public GameObject explosionPrefab;

    public bool didexplode = false;
    
    public void setTopParent()
    {
        this.transform.SetParent(topParent.transform);
        this.transform.localPosition = new Vector3(0, 0, -10);
    }

    public void setBottomParent()
    {
        this.transform.SetParent(bottomParent.transform);
        this.transform.localPosition = new Vector3(0, -25, -10);
    }

    public void jump()
    {
        this.transform.parent.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 7, ForceMode2D.Impulse);
    }

    public void BottomScreen()
    {
        Vector3 newPos = new Vector3(0, -25, -10);
        this.transform.position = newPos;
    }

    public void ResetDidExplode()
    {
        didexplode = false;
    }

    public void Explode()
    {
        if (didexplode)
            return;

        Vector3 v = this.transform.position;
        v.z = 10;
        GameObject g = Instantiate(explosionPrefab, v, Quaternion.identity);
        didexplode = true;
        g.transform.SetParent(this.transform);
    }
}
