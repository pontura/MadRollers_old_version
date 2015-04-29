using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionScene : MonoBehaviour {

    public Text PlayLabel;
    public Text title;
    public Text changeMission;

	void Start () {
        Data.Instance.GetComponent<Tracker>().TrackScreen("Main Menu");
        Data.Instance.events.OnInterfacesStart();

        if (Application.platform == RuntimePlatform.Android)
            Data.Instance.mode = Data.modes.ACCELEROMETER;

        if (Data.Instance.levelUnlockedID == 0)
        {
            title.text = "YOUR FIRST MISSION... BABY!";
            changeMission.enabled = false;
        }
        else 
        {
            title.text = "CURRENT MISSION: " + Data.Instance.levelUnlockedID;
        }
    }
    public void Play()
    {

        if (Data.Instance.levelUnlockedID == 0)
            Data.Instance.levelUnlockedID = 1;

        Data.Instance.missionActive = Data.Instance.levelUnlockedID;
        Fade.LoadLevel("Game", 1, 1, Color.black);
    }
    public void SelectLevels()
    {
        Fade.LoadLevel("LevelSelector", 1, 1, Color.black);
    }
    public void Back()
    {
        Fade.LoadLevel("MainMenu", 1, 1, Color.black);
    }
}
