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
        Data.Instance.events.OnGameStart();
        Init();
        Data.Instance.GetComponent<Tracker>().TrackScreen("Game Screen");
    }

	private void Init()
	{
        Data.Instance.events.MissionStart(Data.Instance.missionActive);
        UnPause();
	}

    //pierdo y arranca de ni
    public void ResetLevel()
	{
        Data.Instance.events.OnResetLevel();
        //StartCoroutine (restart ());
        Data.Instance.replays++;
        if (Data.Instance.replays == Data.Instance.totalReplays)
            GotoContinue();
        else
            Fade.LoadLevel("Game", 1, 1, Color.black);
	}
    //IEnumerator  restart()
    //{
    //    yield return new WaitForSeconds(1f);
    //    Debug.Log("_____________Restart");
    //    Application.LoadLevel("Game");
    //}

	public void Pause () {
        Time.timeScale = 0;
        Data.Instance.events.OnGamePaused(true);
	}
    public void UnPause()
    {
        if (Time.timeScale == 1) return;
        Time.timeScale = 1;
        Data.Instance.events.OnGamePaused(false);
    }
    public void GotoLevelSelector()
    {
        Pause();
        Data.Instance.events.OnResetLevel();
        Debug.Log("GotoLevelSelector");
       // Application.LoadLevel("LevelSelector");
        Time.timeScale = 1;
        Fade.LoadLevel("LevelSelector", 1, 1, Color.black);
    }
    public void GotoMainMenu()
    {
        Pause();
        Data.Instance.events.OnResetLevel();
        Time.timeScale = 1;
        Fade.LoadLevel("MainMenu", 1, 1, Color.black);
    }
    public void GotoContinue()
    {
        Pause();
        Data.Instance.events.OnResetLevel();
        Time.timeScale = 1;
        Fade.LoadLevel("Continue", 1, 1, Color.black);
    }
}
