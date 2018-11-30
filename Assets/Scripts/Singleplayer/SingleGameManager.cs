using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class SingleGameManager : MonoBehaviour {

    public static bool[] Levels = new bool[11];
    public GameObject[] trainUis = new GameObject[10];

    public Sprite greenTrain;

    public static int maxEnemies = 0;
    public static int currentLevel = 0;

    public int currentEnemy = 0;

    public GameObject archingLevel;
    public Text coinText;

    public bool gameEnded = false;

    public GameObject particlePrefab;

    public GameObject youdiedText;

    public GameObject tutorial;

    public void Start()
    {
        trainUis[currentLevel - 1].SetActive(true);

        if (currentLevel == 1)
        {
            tutorial.GetComponent<UIscript>().StartOpenings();

            GameObject timer = GameObject.FindGameObjectWithTag("CountDownTimer");
            timer.GetComponent<TimerScript>().Stoptimer();
        }
    }

    public void Update()
    {
        if (gameEnded)
        {
            if (archingLevel.GetComponent<UIscript>().Opened)
            {
                Invoke("showTrainUI", 1f);
                gameEnded = false;
            }
        }
    }

    public void ResetGame()
    {
        GameObject timer = GameObject.FindGameObjectWithTag("CountDownTimer");
        
        timer.GetComponent<TimerScript>().ResetTimer();
    }
    
    public void endGame()
    {
        Invoke("maketheendgame", 1f);
        gameEnded = true;
    }

    public void maketheendgame()
    {
        archingLevel.GetComponent<UIscript>().StartOpenings();
    }

    public void showTrainUI()
    {
        makeATrainGreen();
    }

    public void makeATrainGreen()
    {
        Image[] trains = trainUis[currentLevel - 1].GetComponentsInChildren<Image>();
        
        foreach(Image image in trains)
        {
            if(image.sprite != greenTrain)
            {
                image.sprite = greenTrain;
                Instantiate(particlePrefab, image.transform);
                break;
            }
        }

        SoundManager.current.PlayRoundWin();

        currentEnemy++;
        
        int randomAmount = Random.Range(10, 19);

        if (BuyButtons.boughtShield)
        {
            randomAmount *= 2;
        }

        coinText.text = "+" + randomAmount;
        Coins.Money += randomAmount;
        
        if (currentEnemy == maxEnemies)
        {
            if (!Levels[currentLevel])
            {
                int total = currentLevel * 100;
                Coins.Money += total;
                total += randomAmount;
                coinText.text = "+" + total;
            }

            SoundManager.current.PlayGameWin();

            Invoke("levelComplete", 2f);
        }
        else
        {
            Invoke("closeTrainUI", 3f);
        }
        
        Coins.Save();
    }

    public bool CheckLevelCompletion(int cl)
    {
        if(Levels[cl] == true)
        {
            return false;
        }

        bool AllMapsDone = true;
        for(int i = 1; i < 11; i++)
        {
            if(Levels[i] == false && i != cl)
            {
                AllMapsDone = false;
            }
        }

        if(AllMapsDone == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void levelComplete()
    {
        DimmerScript.RoomToGoTo = 1;
        GameObject.Find("Leaver").GetComponent<DimmerScript>().Begin();

        if (CheckLevelCompletion(currentLevel))
        {
            CompleteLevel.AllCompletedOnce = true;
        }

        Levels[currentLevel] = true;

        CompleteLevel.Save();
    }

    public void closeTrainUI()
    {
        archingLevel.GetComponent<UIscript>().BeginDeactiving();
        
        Invoke("ResetGame", 2f);
    }

    public void playerDied()
    {
        youdiedText.gameObject.SetActive(true);
    }
}
