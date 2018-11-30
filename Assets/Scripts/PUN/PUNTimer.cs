using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PUNTimer : Photon.PunBehaviour {

    public bool calledOnce = false;

    public bool clientReady = false;

    public int whoDied = 0;

    public bool calledOnceDetermine = false;

    public void Start()
    {
        this.GetComponent<Animator>().speed = 0;
        //omg
    }

    [PunRPC]
    public void PlayerDetermineSounds(int btnState)
    {
        if (btnState == 0)
        {
            SoundManager.current.DefaultSound();
        }
        else if (btnState == 1)
        {
            SoundManager.current.PlayPlayerShotSound();
        }
        else if (btnState == 2)
        {
            SoundManager.current.PlayPlayerDuckSound();
        }
        else if (btnState == 3)
        {
            SoundManager.current.PlayPlayerReloadSound();
        }
    }
    
    [PunRPC]
    public void EnemyDetermineSounds(int ButtonState)
    {
        if (ButtonState == 0)
        {
            SoundManager.current.DefaultSound();
        }
        else if (ButtonState == 1)
        {
            SoundManager.current.PlayEnemyShotSound();
        }
        else if (ButtonState == 2)
        {
            SoundManager.current.PlayEnemyDuckSound();
        }
        else if (ButtonState == 3)
        {
            SoundManager.current.PlayEnemyReloadSound();
        }
    }

    public void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        if (players.Length == 2)
        {
            StartTimer();
        }

        if (clientReady)
        {
            this.photonView.RPC("StartTimerPUN", PhotonTargets.All);
            clientReady = false;
        }
    }

    public void StartTimer()
    {
        this.GetComponent<Animator>().speed = 1;
    }
    
    public void Stoptimer()
    {
        this.GetComponent<Animator>().enabled = false;
    }

    [PunRPC]
    public void turnOnDraw()
    {
        GameObject.Find("DrawText").GetComponent<Animator>().SetBool("TurnOff", true);
    }

    public void Determine()
    {
        if (calledOnceDetermine)
            return;

        calledOnceDetermine = true;

        calledOnce = false;
        
        GameObject host, client;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length != 2)
        {
            Debug.Log("Not enuf players");
            return;
        }

        if (players[0].transform.position.y == -2.5f)
        {
            host = players[0];
            client = players[1];
        }
        else
        {
            host = players[1];
            client = players[0];
        }

        if(host == null || client == null)
        {
            Debug.Log("Someone is null.");
        }

        Debug.Log("Host: " + host.GetComponent<Animator>().GetInteger("ButtonState") + " - Client: " + client.GetComponent<Animator>().GetInteger("ButtonState"));
        
        if (host.GetComponent<Animator>().GetInteger("ButtonState") == 1 && (client.GetComponent<Animator>().GetInteger("ButtonState") == 3 || client.GetComponent<Animator>().GetInteger("ButtonState") == 0))
        {
            Debug.Log("Host Wins!");
            this.photonView.RPC("ActivateDeaths", PhotonTargets.All, false);
            playWinSound();
            this.photonView.RPC("playLostSound", PhotonTargets.Others);
            PlayerDetermineSounds(1);
            this.photonView.RPC("EnemyDetermineSounds", PhotonTargets.Others, 1);
        }
        else if ((host.GetComponent<Animator>().GetInteger("ButtonState") == 3 || host.GetComponent<Animator>().GetInteger("ButtonState") == 0) && client.GetComponent<Animator>().GetInteger("ButtonState") == 1)
        {
            Debug.Log("Client Wins!");
            this.photonView.RPC("ActivateDeaths", PhotonTargets.All, true);
            this.photonView.RPC("playWinSound", PhotonTargets.Others);
            playLostSound();
            EnemyDetermineSounds(1);
            this.photonView.RPC("PlayerDetermineSounds", PhotonTargets.Others, 1);
        }
        else if (host.GetComponent<Animator>().GetInteger("ButtonState") == 1 && client.GetComponent<Animator>().GetInteger("ButtonState") == 1)
        {
            this.photonView.RPC("turnOnDraw", PhotonTargets.All);
        }
        else
        {
            if (PhotonNetwork.isMasterClient)
            {
                this.photonView.RPC("PlayerDetermineSounds", PhotonTargets.Others, client.GetComponent<Animator>().GetInteger("ButtonState"));
                this.photonView.RPC("EnemyDetermineSounds", PhotonTargets.Others, host.GetComponent<Animator>().GetInteger("ButtonState"));

                PlayerDetermineSounds(host.GetComponent<Animator>().GetInteger("ButtonState"));
                EnemyDetermineSounds(client.GetComponent<Animator>().GetInteger("ButtonState"));
            }
        }
    }

    [PunRPC]
    public void playLostSound()
    {
        SoundManager.current.PlayRoundLost();
    }

    [PunRPC]
    public void playWinSound()
    {
        SoundManager.current.PlayGameWin();
    }

    public void TakeAction()
    {
        if (calledOnce)
            return;

        Camera.main.GetComponent<PUNCameraMovement>().jump();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for(int i = 0; i < players.Length; i++)
        {
            if(players[i].GetPhotonView().isMine)
                players[i].GetComponent<PUNPlayer_Script>().Action();
        }

        calledOnce = true;

        calledOnceDetermine = false;
    }
    
    [PunRPC]
    public void ActivateDeaths(bool HostDied)
    {
        if (HostDied)
        {
            whoDied = 1;
        }
        else
        {
            whoDied = 2;
        }

        this.GetComponent<Animator>().SetInteger("WhoDied", whoDied);
        this.endGameActivateRematchs();
    }
    
    [PunRPC]
    public void setRematchText()
    {
        GameObject.Find("GameManager").GetComponent<PUNGameManager>().setRematchText2();
    }

    public void endGameActivateRematchs()
    {
        GameObject.Find("GameManager").GetComponent<PUNGameManager>().EndedGame();
    }

    public void callsRestart()
    {
        this.photonView.RPC("restartGame", PhotonTargets.All);
    }

    [PunRPC]
    public void restartGame()
    {
        whoDied = 0;
        this.GetComponent<Animator>().SetInteger("WhoDied", whoDied);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject.Find("GameManager").GetComponent<PUNGameManager>().StartNewGame();

        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PUNPlayer_Script>().ResetPlayer();
        }
    }

    [PunRPC]
    public void StartTimerPUN()
    {
        this.GetComponent<Animator>().enabled = true;

        if(PhotonNetwork.isMasterClient)
            Determine();
    }

    [PunRPC]
    public void ClientReady()
    {
        clientReady = true;
    }
}
