using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Advertisements;

[System.Serializable]
public class Username
{
    public string username;
    public int userId;
}

public class UsernameScript : MonoBehaviour {
    public static Username username;

    public GameObject usernameUI;
    public Button CreateUsernameButton;
    public InputField UsernameField;
    private string StephsCode = "01222016steph";
    private string AaronsCode = "01222016aaron";
    private string AaronsUsername = "Captain";
    private string StephsUsername = "Bucky";
    private int AaronsNumber = 100;
    private int StephsNumber = 100;
    public Text feedback;

    public static bool NewGame = false;

    public static bool playAds = true;

    public static void randomizeUsernameID()
    {
        username.userId = Random.Range(101, 999);
        username.userId = 880;
        setUsernameOnScreen();
    }

    public void startup()
    {
        if (!Load())
        {
            BeginUsername();
            NewGame = true;
        }

        bool loading = BuyButtons.Load();
        if (!loading)
        {

        }
    }

    public void Update()
    {
        if(playAds)
            PlayAds();
    }

    public void PlayAds()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
            playAds = false;
        }
    }

    public void BeginUsername()
    {
        usernameUI.GetComponent<UIscript>().StartOpenings();
    }

    public void CreateUsername()
    {
        username = new Username();
        username.username = UsernameField.text;
        if(username.username.Length <= 3 && username.username != StephsCode && username.username != AaronsCode)
        {
            feedback.text = "The username must be longer than 3 characters.";
        }
        else if(username.username.Length >= 10 && username.username != StephsCode && username.username != AaronsCode)
        {
            feedback.text = "The username must be less than 10 characters.";
        }
        else
        {
            username.userId = Random.Range(101, 999);

            if (username.username == StephsCode)
            {
                username.username = StephsUsername;
                username.userId = StephsNumber;
            }
            else if (username.username == AaronsCode)
            {
                username.username = AaronsUsername;
                username.userId = AaronsNumber;
            }

            Save();
            usernameUI.GetComponent<UIscript>().BeginDeactiving();
            Load();
        }
    }

    public static void setUsernameOnScreen()
    {
        GameObject UserText = GameObject.Find("UsernameText");
        UserText.GetComponent<Text>().text = "Welcome back " + username.username;
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedUsername.gd");
        bf.Serialize(file, (Username)username);
        file.Close();
    }

    public bool Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedUsername.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedUsername.gd", FileMode.Open);
            username = new Username();
            username = (Username)bf.Deserialize(file);
            file.Close();

            setUsernameOnScreen();

            return true;
        }
        else
        {
            return false;
        }
    }
}
