using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

[System.Serializable]
public class Settings
{
    public bool musicon, soundon, acceptchallenges;
}

public class SettingsScript : MonoBehaviour {
    public static bool MusicOn = true, SoundOn = true, AcceptChallenges = true;

    public GameObject archingSettingUI;

    public void Start()
    {
        if (!Load())
        {
            Debug.Log("Couldn't load settings.");
        }

        if (!MusicOn)
        {
            MusicScript.SettingsTurnMusicOff();
        }
    }

    public void SetupSettings()
    {
        archingSettingUI.GetComponent<ToggleUpdater>().ResetToggles();
        archingSettingUI.GetComponent<UIscript>().StartOpenings();
    }

    public void TakedownSettings()
    {
        archingSettingUI.GetComponent<UIscript>().BeginDeactiving();
        Save();
    }

    public void ToggleSound()
    {
        SoundOn = !SoundOn;
        archingSettingUI.GetComponent<ToggleUpdater>().ResetToggles();
    }

    public void ToggleMusic()
    {
        MusicOn = !MusicOn;
        archingSettingUI.GetComponent<ToggleUpdater>().ResetToggles();
        if (MusicOn)
        {
            MusicScript.SettingsTurnMusicOn();
        }
        else
        {
            MusicScript.SettingsTurnMusicOff();
        }
    }

    public void ToggleChallenges()
    {
        AcceptChallenges = !AcceptChallenges;
        archingSettingUI.GetComponent<ToggleUpdater>().ResetToggles();
    }

    public static void Save()
    {
        Settings settings = new Settings();

        settings.musicon = MusicOn;
        settings.soundon = SoundOn;
        settings.acceptchallenges = AcceptChallenges;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedSettings.gd");
        bf.Serialize(file, (Settings)settings);
        file.Close();
    }

    public static bool Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedSettings.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedSettings.gd", FileMode.Open);
            Settings settings = new Settings();
            settings = (Settings)bf.Deserialize(file);
            file.Close();

            MusicOn = settings.musicon;
            SoundOn = settings.soundon;
            AcceptChallenges = settings.acceptchallenges;

            return true;
        }
        else
        {
            return false;
        }
    }
}
