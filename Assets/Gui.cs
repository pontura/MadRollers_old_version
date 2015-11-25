using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Gui : MonoBehaviour {
    
    [SerializeField]
    Text scoreLabel;

    [SerializeField]
    LevelComplete levelComplete;

    public GameObject[] hideOnCompetitions;
    public GameObject helpPanel;
	private Data data;   

	private int barWidth = 200;
    private bool MainMenuOpened = false;

    private Events events;
	 
	void Start()
	{
        if (Data.Instance.playMode == Data.PlayModes.COMPETITION)
        {
            foreach (GameObject go in hideOnCompetitions)
            {
                Destroy(go);
            }
            return;
        }
        events = Data.Instance.events;
        levelComplete.gameObject.SetActive(false);
        Data.Instance.events.OnSetFinalScore += OnSetFinalScore;
        Data.Instance.events.OnMissionComplete += OnMissionComplete;
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnAvatarFall += OnAvatarCrash;
    }
    void OnDestroy()
    {
        Data.Instance.events.OnSetFinalScore -= OnSetFinalScore;
        Data.Instance.events.OnMissionComplete -= OnMissionComplete;
        Data.Instance.events.OnAvatarCrash -= OnAvatarCrash;
        Data.Instance.events.OnAvatarFall -= OnAvatarCrash;
        Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;

        events = null;
        levelComplete = null;
        scoreLabel = null;
    }
    void OnAvatarCrash(CharacterBehavior cb)
    {
        levelComplete.gameObject.SetActive(false); 
    }
    void OnListenerDispatcher(string message)
    {
        levelComplete.gameObject.SetActive(false);  
    }
    void OnMissionComplete(int num)
    {
        if (Data.Instance.playMode == Data.PlayModes.COMPETITION) return;
        levelComplete.gameObject.SetActive(true);
        levelComplete.Init(num);
    }
    void OnSetFinalScore(Vector3 pos, int _score)
    {
        scoreLabel.text = _score.ToString();
    }
    public void Settings()
    {
        Data.Instance.GetComponent<GameMenu>().Init();
    }
}
