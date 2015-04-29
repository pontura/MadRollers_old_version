using UnityEngine;
using System.Collections;

public class Data : MonoBehaviour {

    public int levelUnlockedID = 0;
	public int missionActive = 0;
    public int totalReplays = 3;
    public int replays = 0;

    public UserData userData;

    public Events events;
    public ObjectPool sceneObjectsPool;
    static Data mInstance = null;
    public modes mode;

    public VoicesManager voicesManager;

    public bool DEBUG;
   

    public enum modes
    {
        ACCELEROMETER,
        KEYBOARD,
        JOYSTICK
    }

    public static Data Instance
    {
        get
        {
            if (mInstance == null)
            {
                Debug.LogError("Algo llama a DATA antes de inicializarse");
            }
            return mInstance;
        }
    }
	void Start () {

        mInstance = this;
		DontDestroyOnLoad(this);
		//setAvatarUpgrades();
        levelUnlockedID = PlayerPrefs.GetInt("levelUnlocked");
        events = GetComponent<Events>();
        userData = GetComponent<UserData>();
        userData.Init();

        //if (!userData.isPlayerDataLogged() || userData.facebookId == "")
            GetComponentInChildren<FBHolder>().Init();

        GetComponent<MusicManager>().Init();
        GetComponent<Tracker>().Init();
        GetComponent<Missions>().Init();
        GetComponent<DataController>().Init();

        if (Application.platform == RuntimePlatform.Android)
        {
            DEBUG = false;
            mode = modes.ACCELEROMETER;
            levelUnlockedID = PlayerPrefs.GetInt("levelUnlocked");
        }
        voicesManager.Init();
	}

    
	public void setMission(int num)
	{
        print("Set mission " + num + "   levelUnlockedID: " + levelUnlockedID);
        replays = 0;
        if (num > levelUnlockedID)
        {
            PlayerPrefs.SetInt("levelUnlocked", num-1);
            levelUnlockedID = num - 1;
        }
		missionActive = num;
	}
    public void resetProgress()
    {
        PlayerPrefs.SetInt("levelUnlocked", 0);
        levelUnlockedID = 0;
        userData.resetProgress();
    }
}
