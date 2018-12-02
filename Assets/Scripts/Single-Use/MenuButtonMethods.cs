using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtonMethods : MonoBehaviour {

    public void PlayBtnSound()
    {
        SoundManager.current.PlayBtnClickSound();
    }

    public void MenuButtonMultiplayer()
    {
        SceneManager.LoadScene(2);
    }

    public void MenuButtonSingleplayer()
    {
        SceneManager.LoadScene(1);
    }

    public void PlayBoughtItem()
    {
        SoundManager.current.PlayBoughtItem();
    }

    public void PlayLevelSelect()
    {
        SoundManager.current.PlayLevelSelect();
    }

    public void PlayInfoSound()
    {
        SoundManager.current.PlayInfoButtonSound();
    }
    
    public void GoToShop()
    {
        SceneManager.LoadScene(5);
    }

    public void PlayToggleSound()
    {
        SoundManager.current.PlayToggleSound();
    }

    public void goBacktoSingleplayerMenu()
    {
        SceneManager.LoadScene(1);
    }
    
    public void PauseTimer()
    {
        GameObject.FindGameObjectWithTag("CountDownTimer").GetComponent<TimerScript>().Stoptimer();
    }

    public void StartTimer()
    {
        GameObject.FindGameObjectWithTag("CountDownTimer").GetComponent<TimerScript>().StartTimer();
    }

    public void showCredits()
    {

    }

    public void hideCredits()
    {

    }

    public void goToRoomWithDimmer(int i)
    {
        DimmerScript.RoomToGoTo = i;
        GameObject.Find("Leaver").GetComponent<DimmerScript>().Begin();
    }

    public void GoodbyeDis()
    {
        PhotonNetwork.Disconnect();
    }

    public void setLeavingRoom()
    {
        PUNNetworkManager.LeftMultiplayer = true;
    }

    public void GoToLevel(int level)
    {
        SingleGameManager.currentLevel = level;
        SingleGameManager.maxEnemies = level * 2;
        goToRoomWithDimmer(4);
    }

    public void GoToMenu()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }

    public void ToggleSound()
    {
        GameObject.Find("SettingsManager").GetComponent<SettingsScript>().ToggleSound();
    }

    public void ToggleMusic()
    {
        GameObject.Find("SettingsManager").GetComponent<SettingsScript>().ToggleMusic();
    }

    public void ToggleChallenges()
    {
        GameObject.Find("SettingsManager").GetComponent<SettingsScript>().ToggleChallenges();
    }

    public void OpenCredits()
    {
        GameObject.Find("CreditsManager").GetComponent<CreditsScript>().SetupCredits();
    }

    public void CloseCredits()
    {
        GameObject.Find("CreditsManager").GetComponent<CreditsScript>().TakeDownCredits();
    }

    public void OpenSettings()
    {
        GameObject.Find("SettingsManager").GetComponent<SettingsScript>().SetupSettings();
    }

    public void CloseSettings()
    {
        GameObject.Find("SettingsManager").GetComponent<SettingsScript>().TakedownSettings();
    }

    public void Rematch()
    {
        GameObject.Find("GameManager").GetComponent<PUNGameManager>().PressedRematch();
    }

    public void MenuButtonGoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void CreateGame(Text txt)
    {
        if (PhotonNetwork.inRoom)
            return;

        PUNNetworkManager.FriendInviteRoom = false;
        GameObject networkManager = GameObject.Find("NetworkManager");
        networkManager.GetComponent<PUNNetworkManager>().showWaitingUI();
        networkManager.GetComponent<PUNNetworkManager>().CreateRoom(UsernameScript.username.username + UsernameScript.username.userId + txt.text);
    }

    public void CreateGameAddFriend()
    {
        if (PhotonNetwork.inRoom)
            return;

        //Get the name.
        GameObject friends = GameObject.Find("FriendManager");
        GameObject friendsNameText = GameObject.Find("FriendInputField");
        InputField friendsName = friendsNameText.GetComponent<InputField>();

        if (friendsName.text == UsernameScript.username.username + UsernameScript.username.userId)
        {
            friends.GetComponent<PUNFriends>().YourName();
            return;
        }

        foreach (string name in PUNFriends.friendsList)
        {
            if(name == friendsName.text)
            {
                friends.GetComponent<PUNFriends>().DuplicateName();
                return;
            }
        }

        RoomInfo[] rooms = PhotonNetwork.GetRoomList();

        for (int i = 0; i < rooms.Length; i++)
        {
            if(friendsName.text == rooms[i].Name)
            {
                friends.GetComponent<PUNFriends>().TryAgainLater();
                return;
            }
        }

        // Make the game.
        GameObject networkManager = GameObject.Find("NetworkManager");
        PUNNetworkManager.FriendInviteRoom = true;
        networkManager.GetComponent<PUNNetworkManager>().CreateRoom(friendsName.text);
        
        // Set the screen up.
        GameObject friendmanager = GameObject.Find("FriendManager");
        friendmanager.GetComponent<PUNFriends>().HostAddingScreen();
    }

    public void JoinRoom(string FriendsName)
    {
        GameObject networkManager = GameObject.Find("NetworkManager");
        networkManager.GetComponent<PUNNetworkManager>().JoinRoom(FriendsName);
    }

    public void Disconnect()
    {
        DimmerScript.isInRoom = true;
        DimmerScript.RoomToGoTo = 2;
        GameObject.Find("Leaver").GetComponent<DimmerScript>().Begin();
    }

    public void CancelFriendRequest()
    {
        GameObject.Find("FriendManager").GetComponent<PUNFriends>().photonView.RPC("Cancel", PhotonTargets.All);
    }

    public void DEBUGGINGChangeUserID()
    {
        UsernameScript.randomizeUsernameID();
    }

    public void MenuDisconnection()
    {
        PhotonNetwork.Disconnect();
    }

    public void DisconenctFromWaitingUI()
    {
        if (PhotonNetwork.inRoom)
        {
            GameObject.Find("FriendManager").GetComponent<PUNFriends>().HostCancelled();
            PhotonNetwork.LeaveRoom();
        }
        GameObject networkManager = GameObject.Find("NetworkManager");
        networkManager.GetComponent<PUNNetworkManager>().hideWaitingUI();
    }

    public void GoOnlineMode()
    { 
        PhotonNetwork.offlineMode = false;
    }

    public void HideFriendUI()
    {
        GameObject FriendManager = GameObject.Find("FriendManager");
        FriendManager.GetComponent<PUNFriends>().HideFriendUI();
    }

    public void BeginFriendProcess()
    {
        GameObject FriendManager = GameObject.Find("FriendManager");
        FriendManager.GetComponent<PUNFriends>().ShowFriendUI();
    }

    public void AcceptFriendButton()
    {
        GameObject FriendManager = GameObject.Find("FriendManager");
        if (PhotonNetwork.room.PlayerCount <= 1)
        {
            FriendManager.GetComponent<PUNFriends>().NotEnoughPeopleFriends();
        }
        else
        {
            FriendManager.GetComponent<PUNFriends>().photonView.RPC("RPCAcceptButton", PhotonTargets.All);
        }
    }

    public void RejectFriendButton()
    {
        GameObject FriendManager = GameObject.Find("FriendManager");
        if(PhotonNetwork.room.PlayerCount <= 1)
        {
            FriendManager.GetComponent<PUNFriends>().NotEnoughPeopleFriends();
        }
        else
        {
            FriendManager.GetComponent<PUNFriends>().photonView.RPC("RPCRejectButton", PhotonTargets.All);
        }
    }

    public void Acceptchallenger()
    {
        GameObject.Find("FriendManager").GetComponent<PUNFriends>().ChallengerAccept();
    }

    public void RejectChallenger()
    {
        GameObject.Find("FriendManager").GetComponent<PUNFriends>().ChallengerReject();
        PUNNetworkManager.BeginRepeating();
    }
}
