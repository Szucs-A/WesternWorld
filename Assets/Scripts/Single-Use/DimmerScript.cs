using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DimmerScript : MonoBehaviour {

    public static int RoomToGoTo;

    public static bool isInRoom = false;

    public static bool goingToRoom = false;

    public static bool blackScreenIt = false;

	// Use this for initialization
	void Start () {
        this.GetComponent<Animator>().speed = 0;
	}

    public void Begin()
    {
        this.GetComponent<Animator>().speed = 1;
        MusicScript.TurnMusicOff();
    }

    public void JumpToRoom()
    {
        if (isInRoom)
        {
            PhotonNetwork.LeaveRoom();
        }

        if (goingToRoom)
        {
            PhotonNetwork.LoadLevel(3);
            goingToRoom = false;
            isInRoom = false;
            return;
        }

        isInRoom = false;
        goingToRoom = false;

        if(!blackScreenIt)
            SceneManager.LoadScene(RoomToGoTo);

        blackScreenIt = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
