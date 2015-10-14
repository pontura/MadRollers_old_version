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
    public void Revive()
    {
        UnPause();
        gameCamera.Init();
        
        CharacterBehavior cb = level.charactersManager.character;
        
        Vector3 pos = cb.transform.position;
        pos.y = 10;
        pos.x = 1;
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
            Data.Instance.userData.OnHiscore(Data.Instance.userData.score);            
            Application.LoadLevel("NewHiscore");
            Data.Instance.replays = 0;
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
       // Application.LoadLevel("LevelSelector");
        Time.timeScale = 1;
        Data.Instance.LoadLevel("MainMenu");
    }
    public void GotoMainMenu()
    {
        Pause();
        Data.Instance.events.OnResetLevel();
        Time.timeScale = 1;
        Data.Instance.LoadLevel("MainMenu");
    }
    public void GotoContinue()
    {
        Pause();
        Data.Instance.events.OnResetLevel();
        Time.timeScale = 1;
        Data.Instance.LoadLevel("Continue");
    }
}
