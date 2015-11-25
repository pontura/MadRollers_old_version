using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

    const string PREFAB_PATH = "Prefabs/Game";
    public GameCamera gameCamera;

    static Game mInstance = null;

	private float pausedSpeed = 0.005f;
	private float pausedMiniumSpeed = 0.05f;
	private bool paused;
	private bool unpaused;

    public MoodManager moodManager;


    public Level level;

    public static Game Instance
    {
        get
        {
            if (mInstance == null)
            {
                Debug.LogError("Algo llama a Game antes de inicializarse");
            }
            return mInstance;
        }
    }
    void Awake()
    {
        mInstance = this;

        GetComponent<CharactersManager>().Init();

        level.Init();
        gameCamera.Init();
        
    }
    void Start()
    {
        Data.Instance.events.OnGamePaused += OnGamePaused;
        Data.Instance.events.OnGameStart();
        Init();
        Data.Instance.GetComponent<Tracker>().TrackScreen("Game Screen");
        Data.Instance.events.SetSettingsButtonStatus(false);
    }
    void OnDestroy()
    {
        Data.Instance.events.OnGamePaused -= OnGamePaused;
    }
	private void Init()
	{
        Data.Instance.events.MissionStart(Data.Instance.missionActive);
        Data.Instance.events.OnGamePaused(false);
	}
    public void Revive()
    {
        Data.Instance.events.OnGamePaused(false);
        gameCamera.Init();
        
        CharacterBehavior cb = level.charactersManager.character;
        
        Vector3 pos = cb.transform.position;
        pos.y = 40;
        pos.x = 0;
        cb.transform.position = pos;

        cb.Revive();
    }
    //pierdo y arranca de ni
    public void ResetLevel()
	{
        Data.Instance.events.OnResetLevel();
        Data.Instance.replays++;

        if (Data.Instance.playingTutorial && Data.Instance.levelUnlockedID > 3)
        {
            Data.Instance.LoadLevel("TrainingReady");
        }else
        if (Data.Instance.userData.hiscore < Data.Instance.userData.score)
        {
           // Data.Instance.userData.OnHiscore(Data.Instance.userData.score);            
           // Application.LoadLevel("NewHiscore");
            Data.Instance.replays = 0;
            Data.Instance.LoadLevel("Game");
        } 
        //else if (Data.Instance.replays == Data.Instance.totalReplays)
        //    GotoContinue();
        else
            Data.Instance.LoadLevel("Game");
	}
    //IEnumerator  restart()
    //{
    //    yield return new WaitForSeconds(1f);
    //    Debug.Log("_____________Restart");
    //    Application.LoadLevel("Game");
    //}

    public void OnGamePaused(bool paused)
    {
        if (paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void GotoLevelSelector()
    {
       // Pause();
        Data.Instance.events.OnResetLevel();
       // Application.LoadLevel("LevelSelector");
        Time.timeScale = 1;
        Data.Instance.LoadLevel("MainMenu");
    }
    public void GotoMainMenu()
    {
      //  Pause();
        Data.Instance.events.OnResetLevel();
        Time.timeScale = 1;
        Data.Instance.LoadLevel("MainMenu");
    }
    public void GotoContinue()
    {
       // Pause();
        Data.Instance.events.OnResetLevel();
        Time.timeScale = 1;
        Data.Instance.LoadLevel("Continue");
    }
}
