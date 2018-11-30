using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PUNNetworkManager : Photon.PunBehaviour {

    private const string VERISON = "v1";

    public static bool FriendInviteRoom = false;

    public static string previousChallenge = "";

    public Text connectingText;
    public Button CancelButton;
    public GameObject connectingUI;
    public GameObject waitingUI, waitingCancelBtn;

    public static bool LeftMultiplayer = false;
    public static bool isLooking = false;

    public GameObject loading;
    
	void Start ()
    {
        if (!PhotonNetwork.connected)
        {
            //CHANGED THIS
            PhotonNetwork.automaticallySyncScene = true;
            PhotonNetwork.autoJoinLobby = true;
            PhotonNetwork.ConnectUsingSettings(VERISON);
        }

        InvokeRepeating("TestForGames", 3, 3);
    }

    public static void BeginRepeating()
    {
        GameObject.Find("NetworkManager").GetComponent<PUNNetworkManager>().InvokeRepeating("TestForGames", 3, 3);
        Debug.Log("Looking for Games.");
    }

    public void showWaitingUI()
    {
        waitingUI.GetComponentInChildren<Text>().text = "Waiting for other player...";
        waitingCancelBtn.SetActive(true);
        waitingUI.GetComponent<UIscript>().StartOpenings();
    }

    public void hideWaitingUI()
    {
        waitingUI.GetComponent<UIscript>().BeginDeactiving();
    }
    
    public void TestForGames()
    {
        isLooking = true;

        if (PhotonNetwork.inRoom)
        {
            return;
        }

        if (PhotonNetwork.insideLobby && SettingsScript.AcceptChallenges)
        {
            RoomInfo[] rooms = PhotonNetwork.GetRoomList();

            for (int i = 0; i < rooms.Length; i++)
            {

                //FriendsInvites
                if (rooms[i].Name == UsernameScript.username.username + UsernameScript.username.userId)
                {
                    CancelInvoke();
                    isLooking = false;
                    FriendInviteRoom = true;
                    JoinRoom(rooms[i].Name);
                    break;
                }
                
                //Challenges
                foreach (string name in PUNFriends.friendsList)
                {
                    if (name == previousChallenge)
                    {
                        previousChallenge = "";
                        continue;
                    }

                    if (rooms[i].PlayerCount == 0)
                        continue;

                    if (rooms[i].Name == name + UsernameScript.username.username + UsernameScript.username.userId)
                    {
                            previousChallenge = name;
                            CancelInvoke();
                            isLooking = false;
                            FriendInviteRoom = false;
                            JoinRoom(rooms[i].Name);
                            activateChallengeUI(name);
                            break;
                    }
                }
            }
        }
    }

    public void activateChallengeUI(string name)
    {
        GameObject friendsobj = GameObject.Find("FriendManager");
        PUNFriends friendscript = friendsobj.GetComponent<PUNFriends>();

        friendscript.showChallengerUI(name);
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.CreateRoom(UsernameScript.username.username, roomOptions, TypedLobby.Default);
    }

    public void CreateRoom(string name)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.CreateRoom(name, roomOptions, TypedLobby.Default);
    }

    public void JoinRoom(string FriendsName)
    {
        PhotonNetwork.JoinRoom(FriendsName);
    }

    public override void OnJoinedLobby()
    {
        turnOffConnectingUI();
    }

    public void turnOffConnectingUI()
    {
        connectingUI.GetComponent<UIscript>().BeginDeactiving();
    }

    public override void OnLeftLobby()
    {
        //This is called when someone disconnects from the internet when on multiplayer screen.
        
    }

    public override void OnConnectedToMaster()
    {

    }

    public override void OnDisconnectedFromPhoton()
    {
        //Debug.Log("BONFIRE");
        if (LeftMultiplayer)
        {
            LeftMultiplayer = false;
            return;
        }

        //This is called when someone disconnects from the internet when on multiplayer screen.
        connectingText.text = "Seems like there isn't a connection to the internet.";
        CancelButton.gameObject.SetActive(true);
        connectingUI.GetComponent<UIscript>().StartOpenings();
        loading.gameObject.SetActive(false);
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        if (!IsInvoking("TestForGames"))
        {
            BeginRepeating();
        }
    }

    public override void OnJoinedRoom()
    {
        if (FriendInviteRoom)
        {
            //check if its the master otherwise show the ui and make the friendscreen popup
            if (!PhotonNetwork.isMasterClient)
            {
                GameObject friendsobj = GameObject.Find("FriendManager");
                PUNFriends friendscript = friendsobj.GetComponent<PUNFriends>();

                friendscript.ShowFriendUI();
                friendscript.clientAddingScreen();
            }
        }
        else
        {
            //PhotonNetwork.LoadLevel(3);
        }
    }

    public override void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        if (LeftMultiplayer)
        {
            LeftMultiplayer = false;
            return;
        }

        //This is called when someone disconnects from the internet when on multiplayer screen.
        connectingText.text = "Seems like there isn't a connection to the internet.";
        CancelButton.gameObject.SetActive(true);
        connectingUI.GetComponent<UIscript>().StartOpenings();
        loading.gameObject.SetActive(false);
    }

    public override void OnConnectionFail(DisconnectCause cause)
    {
        if (LeftMultiplayer)
        {
            LeftMultiplayer = false;
            return;
        }

        connectingText.text = "Seems like there isn't a connection to the internet.";
        CancelButton.gameObject.SetActive(true);
        connectingUI.GetComponent<UIscript>().StartOpenings();
        loading.gameObject.SetActive(false);
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        GameObject friendsobj = GameObject.Find("FriendManager");
        PUNFriends friendscript = friendsobj.GetComponent<PUNFriends>();

        friendscript.photonView.RPC("SendHostData", PhotonTargets.All, UsernameScript.username.username + UsernameScript.username.userId);
    }
    
    
}
