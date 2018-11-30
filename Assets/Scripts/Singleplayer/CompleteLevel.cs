using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LevelHolder
{
    public bool[] oldlevels;
}

public class CompleteLevel : MonoBehaviour {

    public int Level;

    public static bool calledOnce = false;

    public GameObject alternateButton;

    public static bool AllCompletedOnce = false;

	// Use this for initialization
	void Start () {
        if (!calledOnce)
        {
            calledOnce = true;
            if (!Load())
            {
                Debug.Log("Couldn't Load levels.");
            }
        }

        bool currentLevel;

        currentLevel = SingleGameManager.Levels[Level];

        if(currentLevel == true && alternateButton != null)
        {
            this.gameObject.SetActive(false);
            alternateButton.SetActive(true);
        }
	}

    public static void Save()
    {
        LevelHolder lh = new LevelHolder();
        lh.oldlevels = new bool[11];
        for(int i = 0; i < 11; i++)
        {
            lh.oldlevels[i] = SingleGameManager.Levels[i];
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedLevels.gd");
        bf.Serialize(file, (LevelHolder)lh);
        file.Close();
    }

    public bool Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedLevels.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedLevels.gd", FileMode.Open);
            LevelHolder lh = (LevelHolder)bf.Deserialize(file);
            file.Close();

            setLevels(lh);

            return true;
        }
        else
        {
            return false;
        }
    }

    public static void setLevels(LevelHolder lh)
    {
        SingleGameManager.Levels = lh.oldlevels;
    }
}
