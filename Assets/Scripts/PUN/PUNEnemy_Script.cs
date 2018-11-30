using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PUNEnemy_Script : Photon.PunBehaviour {
    
    private Animator myAnimator;

    public int ButtonState = 0;

	// Use this for initialization
	void Start () {
        myAnimator = GetComponent<Animator>();
    }

    public void jump()
    {
        this.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 7, ForceMode2D.Impulse);
    }

    public void UpdateAnimation()
    {
        myAnimator.SetInteger("ButtonState", ButtonState);
    }
    
    public void buttonShoot()
    {
        ButtonState = 1;
        this.photonView.RPC("SetButtonState", PhotonTargets.All, 1);
    }

    public void buttonReload()
    {
        ButtonState = 3;
        this.photonView.RPC("SetButtonState", PhotonTargets.All, 3);
    }

    public void buttonBlock()
    {
        ButtonState = 2;
        this.photonView.RPC("SetButtonState", PhotonTargets.All, 2);
    }

    public void MakeDefault()
    {
        ButtonState = 0;
        this.photonView.RPC("SetButtonState", PhotonTargets.All, 0);
    }

    [PunRPC]
    public void SetButtonState(int BS)
    {
        ButtonState = BS;
    }

    [PunRPC]
    public void Action()
    {
        UpdateAnimation();
    }
}
