using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PUNGameManager : Photon.PunBehaviour {

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject timerPrefab;

    public GameObject disconnectUI;

    public GameObject actualDisconnectionUI;

    public Text rematchText;
    public GameObject archingRematchUI;
    public int rematchInt = 0;

    // Use this for initialization
    void Start () {
        Vector3 newPosition, oppositePosition;

        if (PUNPlayer_Script.LocalPlayerInstance == null)
        {
            if (PhotonNetwork.isMasterClient)
            {
                newPosition = new Vector3(-4, -2.5f, 0);
                oppositePosition = new Vector3(4, -47.5f, 0);
                GameObject go = PhotonNetwork.Instantiate(this.timerPrefab.name, new Vector3(0, 3, 0), Quaternion.identity, 0);
                Camera.main.GetComponent<PUNCameraMovement>().setTopParent();
                go.transform.SetParent(GameObject.FindGameObjectWithTag("Parent1").transform);
            }
            else
            {
                newPosition = new Vector3(-4, -47.5f, 0);
                oppositePosition = new Vector3(4, -2.5f, 0);
                //Adjust the camera.
                Camera.main.GetComponent<PUNCameraMovement>().setBottomParent();
                //AdjustCamera();
            }
            
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(this.playerPrefab.name, newPosition, Quaternion.identity, 0);

            PhotonNetwork.Instantiate(this.enemyPrefab.name, oppositePosition, Quaternion.identity, 0);
        }
    }

    private void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        int numberOfRematch = 0;

        foreach (GameObject go in players)
        {
            if (go.GetComponent<PUNPlayer_Script>().WantsRematch)
                numberOfRematch++;
        }

        if (archingRematchUI.GetActive())
        {
            if(numberOfRematch == 1)
            {
                rematchText.text = numberOfRematch + "/2";
            }
            else if(numberOfRematch == 2)
            {
                rematchText.text = numberOfRematch + "/2";
            }
            
            if(numberOfRematch >= 2)
            {
                GameObject.FindGameObjectWithTag("CountDownTimer").GetComponent<PUNTimer>().photonView.RPC("setRematchText", PhotonTargets.All);
                //Launch restart.
                GameObject.FindGameObjectWithTag("CountDownTimer").GetComponent<PUNTimer>().callsRestart();
            }
        }

        if(players.Length == 0)
        {
            actualDisconnectionUI.GetComponent<UIscript>().StartOpenings();
        }
    }
    
    public void StartNewGame()
    {
        archingRematchUI.GetComponent<UIscript>().BeginDeactiving();
    }

    public void AdjustCamera()
    {
        Camera.main.GetComponent<PUNCameraMovement>().BottomScreen();
    }
    
    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        GameObject timer = GameObject.FindGameObjectWithTag("CountDownTimer");
        timer.GetComponent<Animator>().enabled = false;

        if (PhotonNetwork.isMasterClient)
            timer.SetActive(false);

        disconnectUI.GetComponent<UIscript>().StartOpenings();
        Invoke("LeaveRoom", 3f);
    }

    public void LeaveRoom()
    {
        DimmerScript.isInRoom = true;
        DimmerScript.RoomToGoTo = 2;
        GameObject.Find("Leaver").GetComponent<DimmerScript>().Begin();
    }
    
    public void setRematchText2()
    {
        rematchText.text = "2/2";
    }
    
    /// <summary>
    /// FORGETTING TO CALL THIS
    /// </summary>
    public void EndedGame()
    {
        // Turn on the UI for rematch.
        Invoke("endingGame", 2f);
    }

    public void endingGame()
    {
        archingRematchUI.GetComponent<UIscript>().StartOpenings();
        rematchText.text = "0/2";
    }

    // Link this with a button somehow.
    public void PressedRematch()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject go in players)
        {
            if (go.GetPhotonView().isMine)
            {
                go.GetPhotonView().RPC("Rematch", PhotonTargets.All);
            }
        }
    }
}
