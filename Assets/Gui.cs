using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Gui : MonoBehaviour {
    
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
        events = Data.Instance.events;
        levelComplete.gameObject.SetActive(false);
        Data.Instance.events.OnSetFinalScore += OnSetFinalScore;
        Data.Instance.events.OnMissionComplete += OnMissionComplete;
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
    }
    void OnDestroy()
    {
        Data.Instance.events.OnSetFinalScore -= OnSetFinalScore;
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
        if (Data.Instance.playMode == Data.PlayModes.COMPETITION) return;
        levelComplete.gameObject.SetActive(true);
        levelComplete.Init(num);
    }
    void OnSetFinalScore(Vector3 pos, int _score)
    {
        scoreLabel.text = _score.ToString();
    }
}
