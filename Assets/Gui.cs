using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Gui : MonoBehaviour {

    [SerializeField]
    GameMenu gameMenu;

    [SerializeField]
    Text scoreLabel;

    [SerializeField]
    LevelComplete levelComplete;

	private Data data;   

	private int barWidth = 200;
    private bool MainMenuOpened = false;

    private Events events;
	 
	void Start()
	{
        gameMenu.gameObject.SetActive(false);
        events = Data.Instance.events;
        levelComplete.gameObject.SetActive(false);
        Data.Instance.events.OnSetFinalScore += OnSetFinalScore;
        Data.Instance.events.OnCloseMainmenu += OnCloseMainmenu;
        Data.Instance.events.OnMissionComplete += OnMissionComplete;
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
    }
    void OnDestroy()
    {
        Data.Instance.events.OnSetFinalScore -= OnSetFinalScore;
        Data.Instance.events.OnCloseMainmenu -= OnCloseMainmenu;
        Data.Instance.events.OnMissionComplete -= OnMissionComplete;
        Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;

        events = null;
        levelComplete = null;
        scoreLabel = null;
    }
    void OnListenerDispatcher(string message)
    {
       // if (message == "ShowMissionName")
            levelComplete.gameObject.SetActive(false);  
    }
    void OnMissionComplete(int num)
    {
        levelComplete.gameObject.SetActive(true);
        levelComplete.Init(num);
    }
    void OnSetFinalScore(Vector3 pos, int _score)
    {
        scoreLabel.text = _score.ToString();
    }
    void Update()
    {
        if (InputManager.getOpenMenu(0))
        {
            OpenMainMenu();
        }
    }
    private void OnCloseMainmenu()
    {
        MainMenuOpened = false;
    }
   public void OpenMainMenu()
    {
        print("OpenMainMenu " + MainMenuOpened);
        if (MainMenuOpened) return;
        MainMenuOpened = true;
        gameMenu.gameObject.SetActive(true);
        gameMenu.Init();
    }
}
