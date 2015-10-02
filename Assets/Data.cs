using UnityEngine;
using System.Collections;

public class Data : MonoBehaviour {
    
    public int levelUnlockedID = 0;
	public int missionActive = 0;
    public int totalReplays = 3;
    public int replays = 0;
    public float volume;

    [HideInInspector]
    public UserData userData;
    [HideInInspector]
    public Events events;
    [HideInInspector]
    public ObjectPool sceneObjectsPool;
    [HideInInspector]
    public Missions missions;
    [HideInInspector]
    public Competitions competitions;

    static Data mInstance = null;

    public modes mode;

    [HideInInspector]
    public VoicesManager voicesManager;

    public bool DEBUG;
    public int FORCE_LOCAL_SCORE;

    public PlayModes playMode;
    public enum PlayModes
    {
        STORY,
        COMPETITION
    }
    public enum modes
    {
        ACCELEROMETER,
        KEYBOARD,
        JOYSTICK
    }
    public bool playingTutorial;

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
        if (FORCE_LOCAL_SCORE > 0 )
            PlayerPrefs.SetInt("scoreLevel_1", FORCE_LOCAL_SCORE);

        if (Application.loadedLevelName != "01LandingPage")
        {
            Application.LoadLevel("01LandingPage");
            return;
        }
        mInstance = this;
		DontDestroyOnLoad(this);
        

		//setAvatarUpgrades();
       // levelUnlockedID = PlayerPrefs.GetInt("levelUnlocked_0");
        events = GetComponent<Events>();
        missions = GetComponent<Missions>();
        competitions = GetComponent<Competitions>();

        competitions.Init();
        userData.Init();

        if (!Application.isWebPlayer)
            GetComponentInChildren<FBHolder>().Init();

        GetComponent<MusicManager>().Init();
        GetComponent<Tracker>().Init();
        GetComponent<Missions>().Init();
        GetComponent<CurvedWorldManager>().Init();

        //GetComponent<DataController>().Init();
        levelUnlockedID = PlayerPrefs.GetInt("levelUnlocked");

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            DEBUG = false;
            mode = modes.ACCELEROMETER;            
        }
        voicesManager.Init();

        if (Application.isWebPlayer)
            Application.ExternalCall("OnUnityReady");

        events.SetVolume += SetVolume;
	}
    void SetVolume(float vol)
    {
        volume = vol;
    }
	public void setMission(int num)
	{
        //print("MODE: " + playMode + " Set NEW mission " + num + "   levelUnlockedID: " + levelUnlockedID);
        replays = 0;

        levelUnlockedID = num - 1;
        missionActive = num;

        if (playMode == PlayModes.COMPETITION)
        {
            SocialEvents.OnMissionReady(num);
        }
        else
        {
            if (num > levelUnlockedID)
            {
                PlayerPrefs.SetInt("levelUnlocked", num - 1);
            }
        }
	}
    public void resetProgress()
    {
        SocialEvents.OnCompetitionHiscore(1, 0, false);
        PlayerPrefs.DeleteAll();
        levelUnlockedID = 0;
        userData.resetProgress();
        Social.Instance.hiscores.Reset();        
    }
    public void LoadLevel(string levelName)
    {
        GetComponent<Fade>().LoadLevel(levelName, 0.6f, Color.black);
    }
}
