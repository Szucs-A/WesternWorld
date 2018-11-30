using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PUNPlayer_Script : Photon.PunBehaviour {
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    private Animator myAnimator;
    private PUNEnemy_Script correspondentScript;

    public GameObject enemyPrefab;

    public int btnState = 0;
    public bool Died = false;

    public bool WantsRematch = false;

    public int bullets;
    public GameObject bullet1, bullet2, bullet3;

    private void Awake()
    {
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.isMine)
        {
            PUNPlayer_Script.LocalPlayerInstance = this.gameObject;
        }
    }

    // Use this for initialization
    void Start () {
        myAnimator = GetComponent<Animator>();

        if (PhotonNetwork.isMasterClient || photonView.isMine == true)
            return;
        
        Vector3 newPositionForTimer = new Vector3(0, -42, 0);
        GameObject nuller = GameObject.FindGameObjectWithTag("CountDownTimer");
        if (nuller == null)
        {
            return;
        }

        nuller.transform.SetParent(GameObject.FindGameObjectWithTag("Parent2").transform);

        nuller.transform.position = newPositionForTimer;
    }
    
    public void Update()
    {
        if (!photonView.isMine)
        {
            Destroy(this.GetComponent<ButtonMaster>());
            Destroy(this.GetComponent<Raycast>());
            return;
        }

        if(correspondentScript == null)
            FindEnemyMatch();

        checkWhoDied();
    }
    
    public void UpdateBullets()
    {
        if (bullets == 0)
        {
            bullet1.SetActive(false);
            bullet2.SetActive(false);
            bullet3.SetActive(false);
        }
        else if (bullets == 1)
        {
            bullet1.SetActive(true);
            bullet2.SetActive(false);
            bullet3.SetActive(false);
        }
        else if (bullets == 2)
        {
            bullet1.SetActive(true);
            bullet2.SetActive(true);
            bullet3.SetActive(false);
        }
        else if (bullets == 3)
        {
            bullet1.SetActive(true);
            bullet2.SetActive(true);
            bullet3.SetActive(true);
        }
    }

    public void FindEnemyMatch()
    {
        foreach (GameObject Enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (Enemy.transform.position.y == -47.5f && this.transform.position.y == -2.5f)
            {
                correspondentScript = Enemy.GetComponent<PUNEnemy_Script>();
            }
            else if(Enemy.transform.position.y == -2.5f && this.transform.position.y == -47.5f)
            {
                correspondentScript = Enemy.GetComponent<PUNEnemy_Script>();
            }
        }
    }

    public void buttonShoot()
    {
        if (!this.photonView.isMine || Died)
            return;
        btnState = 1;
        correspondentScript.buttonShoot();
        this.photonView.RPC("SetButtonState", PhotonTargets.All, btnState);
    }

    public void buttonReload()
    {
        if (!this.photonView.isMine || Died)
            return;
        btnState = 3;
        correspondentScript.buttonReload();
        this.photonView.RPC("SetButtonState", PhotonTargets.All, btnState);
    }

    public void buttonBlock()
    {
        if (!this.photonView.isMine || Died)
            return;
        btnState = 2;
        correspondentScript.buttonBlock();
        this.photonView.RPC("SetButtonState", PhotonTargets.All, btnState);
    }

    public void buttonDefault()
    {
        if (!this.photonView.isMine || Died)
            return;
        btnState = 0;
        correspondentScript.MakeDefault();
        this.photonView.RPC("SetButtonState", PhotonTargets.All, btnState);
    }
    
    public void checkWhoDied()
    {
        GameObject timer = GameObject.FindGameObjectWithTag("CountDownTimer");

        if (timer == null)
            return;

        int whoDied = timer.GetComponent<PUNTimer>().whoDied;

        GameObject go = GameObject.Find("GameOverText");
        Text gameovertext = null;

        if (go != null)
            gameovertext = GameObject.Find("GameOverText").GetComponent<Text>();
        
        if (whoDied == 1 && PhotonNetwork.isMasterClient)
        {
            // Host Died.
            this.photonView.RPC("SetButtonState", PhotonTargets.All, -1);
            if(gameovertext != null)
                gameovertext.text = "You Lose!";
        }
        else if(whoDied == 2 && !PhotonNetwork.isMasterClient)
        {
            // Client Died.
            this.photonView.RPC("SetButtonState", PhotonTargets.All, -1);
            if (gameovertext != null)
                gameovertext.text = "You Lose!";
        }
        else if(whoDied != 0)
        {
            Camera.main.GetComponent<PUNCameraMovement>().Explode();
            if (gameovertext != null)
                gameovertext.text = "You Win!";
        }
    }

    [PunRPC]
    public void SetButtonState(int BS)
    {
        if (!this.photonView.isMine)
            return;

        if (BS == -1)
        {
            Died = true;
        }

        btnState = BS;
        correspondentScript.SetButtonState(btnState);

        if (BS == -1)
        {
            Action();
        }
    }

    public void Action()
    {
        this.photonView.RPC("rpcUpdate", PhotonTargets.All);
    }

    public void jump()
    {
        this.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 7, ForceMode2D.Impulse);
    }

    public void ResetPlayer()
    {
        this.photonView.RPC("RPCReset", PhotonTargets.All);
    }

    [PunRPC]
    public void RPCReset()
    {
        Died = false;
        //GameObject.Find("GameOverText").GetComponent<Text>().text = "";
        bullets = 0;

        this.photonView.RPC("SetButtonState", PhotonTargets.All, 0);
        WantsRematch = false;
        Action();
        
        ButtonMaster bm = this.GetComponent<ButtonMaster>();
        if (bm != null)
            bm.resetButtons();

        Camera.main.GetComponent<PUNCameraMovement>().ResetDidExplode();
    }

    [PunRPC]
    public void rpcUpdate()
    {
        if (!this.photonView.isMine )
        {
            return;
        }
        UpdateAnimations();
        correspondentScript.UpdateAnimation();
    }

    public void UpdateAnimations()
    {
        if(btnState == 3)
        {
            if(bullets < 3)
            {
                bullets++;
            }
            else
            {
                buttonDefault();
            }
        }

        if(btnState == 1)
        {
            if(bullets > 0)
            {
                bullets--;
            }
            else
            {
                buttonDefault();
            }
        }

        UpdateBullets();

        if (photonView.isMine)
        {
            myAnimator.SetInteger("ButtonState", btnState);
        }
        
        // This is to make it so that the RPC's arent clumped together and get received at the exact same time from the server.
        // That creates a like 50/50 while a .1 of a second makes sure that one gets called later.
        Invoke("TellMasterClient", 0.1f);
    }

    public void TellMasterClient()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            GameObject.FindGameObjectWithTag("CountDownTimer").GetComponent<PUNTimer>().photonView.RPC("ClientReady", PhotonTargets.MasterClient);
            
            GameObject.FindGameObjectWithTag("CountDownTimer").GetComponent<PUNTimer>().calledOnce = false;
        }
    }

    [PunRPC]
    public void Rematch()
    {
        WantsRematch = true;
    }
}
