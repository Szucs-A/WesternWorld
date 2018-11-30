using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class PUNFriends : Photon.PunBehaviour
{
    // AddingFriendUI
    public Image OverarchingUIImage;
    public Image OverarchingChallengeUI;
    public GameObject archingWaitingText, archingWaitingCancelBtn;

    public Button AddFriendBtn, BackBtn, CancelBtn, acceptBtn, rejectBtn, challengeAcceptBtn, challengeRejectBtn;
    public InputField nameInputField;
    public Text hostText, clientText, ChallengerNameText, feedbackHostText, feedbackClientText, feedbackChallengeText;

    // Friendslist.
    public GameObject scrollParent;
    public GameObject friendPrefab;
    private Vector3 currentPosition;
    public Text FriendFeedbackText;

    public static List<string> friendsList;

    private string hostName;

    public void Start()
    {
        friendsList = new List<string>();
        currentPosition = new Vector3(0, 110, 0);

        Load();

        UpdateList();
    }

    [System.Serializable]
    public class FriendsList
    {
        public List<string> savedfriendlist;
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedFriendList.gd");
        FriendsList fl = new FriendsList();
        fl.savedfriendlist = friendsList;
        bf.Serialize(file, (FriendsList)fl);
        file.Close();
    }

    public bool Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedFriendList.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedFriendList.gd", FileMode.Open);
            FriendsList fl = new FriendsList();
            fl = (FriendsList)bf.Deserialize(file);
            friendsList = fl.savedfriendlist;
            file.Close();

            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddFriend(string friend)
    {
        friendsList.Add(friend);

        UpdateList();
    }

    public void ClearList()
    {
        GameObject[] panels = GameObject.FindGameObjectsWithTag("FriendPanel");

        for(int i = 0; i < panels.Length; i++)
        {
            Destroy(panels[i].gameObject);
        }
    }

    public void UpdateList()
    {
        ClearList();

        foreach(string friend in friendsList)
        {
            // creating a new panel.
            GameObject newPanel = Instantiate(friendPrefab, currentPosition, Quaternion.identity);
            newPanel.transform.SetParent(scrollParent.transform, false);
            SetUpPanel(friend, newPanel);
            newPanel.GetComponent<RectTransform>().anchoredPosition = currentPosition;
            currentPosition.y -= 110;
        }
    }
    
    public void SetUpPanel(string name, GameObject panel)
    {
        panel.GetComponentInChildren<Text>().text = name;
    }

    public void HideFriendUI()
    {
        OverarchingUIImage.GetComponent<UIscript>().BeginDeactiving();
    }

    public void ShowFriendUI()
    {
        OverarchingUIImage.GetComponent<UIscript>().StartOpenings();
        resetAllBtns();
    }

    public void resetAllBtns()
    {
        hostText.text = "";
        clientText.text = "";
        AddFriendBtn.gameObject.SetActive(true);
        nameInputField.gameObject.SetActive(true);
        BackBtn.gameObject.SetActive(true);
        CancelBtn.gameObject.SetActive(false);
        acceptBtn.gameObject.SetActive(false);
        rejectBtn.gameObject.SetActive(false);
        feedbackClientText.gameObject.SetActive(false);
        feedbackHostText.text = "";
        feedbackHostText.gameObject.SetActive(false);
        FriendFeedbackText.text = "";
    }

    public void HostAddingScreen()
    {
        AddFriendBtn.gameObject.SetActive(false);
        nameInputField.gameObject.SetActive(false);
        BackBtn.gameObject.SetActive(false);
        CancelBtn.gameObject.SetActive(true);
        FriendFeedbackText.text = "Sending friend request.";
    }

    public void clientAddingScreen()
    {
        AddFriendBtn.gameObject.SetActive(false);
        nameInputField.gameObject.SetActive(false);
        BackBtn.gameObject.SetActive(false);
        CancelBtn.gameObject.SetActive(false);
        acceptBtn.gameObject.SetActive(true);
        rejectBtn.gameObject.SetActive(true);
        FriendFeedbackText.text = "Wants to be your friend.";
    }

    [PunRPC]
    public void Cancel()
    {
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.LeaveRoom();
            DelayedCloseFriendUI();
        }
        else
        {
            FriendCancelled();
        }
    }

    public void FriendDisconnected()
    {
        PUNNetworkManager.BeginRepeating();
        PhotonNetwork.LeaveRoom();
        FriendFeedbackText.text = "Other player disconnected.";
        Debug.Log("OMGCmon");
        acceptBtn.gameObject.SetActive(false);
        rejectBtn.gameObject.SetActive(false);
        Invoke("DelayedCloseFriendUI", 2f);
    }

    public void FriendCancelled()
    {
        PUNNetworkManager.BeginRepeating();
        PhotonNetwork.LeaveRoom();
        FriendFeedbackText.text = "Other player cancelled the friend request.";
        acceptBtn.gameObject.SetActive(false);
        rejectBtn.gameObject.SetActive(false);
        Invoke("DelayedCloseFriendUI", 2f);
    }

    [PunRPC]
    public void SendHostData(string HostName)
    {
        if (PhotonNetwork.isMasterClient)
        {
            hostText.text = nameInputField.text;
            clientText.text = UsernameScript.username.username + UsernameScript.username.userId;
            FriendFeedbackText.text = "Awaiting other player's response.";
            //Host Text
            feedbackHostText.gameObject.SetActive(true);
        }
        else
        {
            hostName = HostName;
            hostText.text = hostName;
            clientText.text = UsernameScript.username.username + UsernameScript.username.userId;
            //Host Text
            feedbackHostText.gameObject.SetActive(true);
        }
    }

    public void CloseDown()
    {
        if (PhotonNetwork.isMasterClient)
        {
            FriendFeedbackText.text = "Other player rejected your request.";
            CancelBtn.gameObject.SetActive(false);
            Invoke("DelayedCloseFriendUI", 2f);
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            PUNNetworkManager.BeginRepeating();
            PhotonNetwork.LeaveRoom();
            HideFriendUI();
        }
    }

    public void AcceptCloseDown()
    {
        if (PhotonNetwork.isMasterClient)
        {
            FriendFeedbackText.text = "Player accepted your friend request.";
            CancelBtn.gameObject.SetActive(false);
            Invoke("DelayedCloseFriendUI", 2f);
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            PUNNetworkManager.BeginRepeating();
            PhotonNetwork.LeaveRoom();
            HideFriendUI();
        }
    }

    public void DelayedCloseFriendUI()
    {
        HideFriendUI();
    }

    public void showChallengerUI(string name)
    {
        challengeRejectBtn.gameObject.SetActive(true);
        challengeAcceptBtn.gameObject.SetActive(true);
        feedbackChallengeText.text = "Challenges you!";
        OverarchingChallengeUI.GetComponent<UIscript>().StartOpenings();
        ChallengerNameText.text = name;
    }

    public void hideChallengerUI()
    {
        OverarchingChallengeUI.GetComponent<UIscript>().BeginDeactiving();
    }

    public void ChallengerAccept()
    {
        //CHANGED THIS
        //GameObject.Find("NetworkManager").GetComponent<PUNNetworkManager>().JoinRoom(ChallengerNameText.text + UsernameScript.username.username + UsernameScript.username.userId);

        if (PhotonNetwork.room.PlayerCount <= 1)
        {
            feedbackChallengeText.text = "Seems like the other player lost connection.";
            PhotonNetwork.LeaveRoom();
            challengeAcceptBtn.gameObject.SetActive(false);
            challengeRejectBtn.gameObject.SetActive(false);
            Invoke("hideChallengerUI", 2f);
        }
        else
        {
            this.photonView.RPC("ChallengeAccepted", PhotonTargets.MasterClient);
            DimmerScript.blackScreenIt = true;
            GameObject.Find("Leaver").GetComponent<DimmerScript>().Begin();
        }
    }

    [PunRPC]
    public void ChallengeAccepted()
    {
        DimmerScript.goingToRoom = true;
        GameObject.Find("Leaver").GetComponent<DimmerScript>().Begin();
    }

    public void HostCancelled()
    {
        this.photonView.RPC("HostDidCancel", PhotonTargets.Others);
    }

    [PunRPC]
    public void HostDidCancel()
    {
        Invoke("hideChallengerUI", 2f);
        Invoke("BeginRepeating", 2f);
        feedbackChallengeText.text = "Other player backed down.";
        challengeAcceptBtn.gameObject.SetActive(false);
        challengeRejectBtn.gameObject.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }

    public void BeginRepeating()
    {
        PUNNetworkManager.BeginRepeating();
    }

    public void ChallengerReject()
    {
        if(PhotonNetwork.room.PlayerCount <= 1)
        {
            feedbackChallengeText.text = "Seems like the other player lost connection.";
            challengeAcceptBtn.gameObject.SetActive(false);
            challengeRejectBtn.gameObject.SetActive(false);
            PhotonNetwork.LeaveRoom();
            Invoke("hideChallengerUI", 2f);
        }
        else
        {
            this.photonView.RPC("RejectChallenge", PhotonTargets.MasterClient);
            PhotonNetwork.LeaveRoom();
            hideChallengerUI();
        }
    }

    [PunRPC]
    public void RejectChallenge()
    {
        archingWaitingText.GetComponentInChildren<Text>().text = "Opponent declined your challenge.";
        archingWaitingCancelBtn.SetActive(false);
        Invoke("HideWaitingUI", 2f);
        PhotonNetwork.LeaveRoom();
    }

    // Called after saying that the other person rejected the challenge.
    public void HideWaitingUI()
    {
        archingWaitingText.GetComponent<UIscript>().BeginDeactiving();
    }

    public void NotEnoughPeopleFriends()
    {
        FriendFeedbackText.text = "Could not reach the other player, cancelling request.";
        acceptBtn.gameObject.SetActive(false);
        rejectBtn.gameObject.SetActive(false);
        Invoke("NotEnoughPeopleCloseDown", 2f);
    }

    public void NotEnoughPeopleCloseDown()
    {
        PUNNetworkManager.BeginRepeating();
        PhotonNetwork.LeaveRoom();
        HideFriendUI();
    }

    [PunRPC]
    public void RPCRejectButton()
    {
        CloseDown();
    }

    [PunRPC]
    public void RPCAcceptButton()
    {
        if (PhotonNetwork.isMasterClient)
        {
            AddFriend(nameInputField.text);
        }
        else
        {
            AddFriend(hostText.text);
        }

        Save();
        
        AcceptCloseDown();
    }

    public void DuplicateName()
    {
        FriendFeedbackText.text = "That person is already on your friends list.";
    }

    public void TryAgainLater()
    {
        FriendFeedbackText.text = "That person is currently busy, try again later.";
    }

    public void YourName()
    {
        FriendFeedbackText.text = "You cannot add yourself, silly.";
    }
}
