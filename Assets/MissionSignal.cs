using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionSignal : MonoBehaviour {

	public Image bg;
    public Text field;
    private Missions AllMissions;

    public AudioClip FXVictory;
    private bool isClosing;

	// Use this for initialization
	void Start () {
        AllMissions = Data.Instance.GetComponent<Missions>();

        Data.Instance.events.OnMissionComplete += OnMissionComplete;
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
        SetOff();
	}
    void OnDestroy()
    {
        Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;
        Data.Instance.events.OnMissionComplete -= OnMissionComplete;
    }
    private void OnMissionComplete(int levelID)
    {
        SetOff();
    }
    void SetOff()
    {
        bg.enabled = false;
        field.enabled = false;
    }
    void SetOn()
    {
        bg.enabled = true;
        field.enabled = true;
    }
    private IEnumerator MissionComplete()
    {
        Open("MISSION COMPLETE!");
        yield return new WaitForSeconds(1);
        GetComponent<AudioSource>().clip = FXVictory;
        GetComponent<AudioSource>().Play();
        CloseAfter(2);
	}
    private void OnListenerDispatcher(string message)
    {
        isClosing = false;
        if (message == "ShowMissionId")
            MissionSignalOn();
        else if (message == "ShowMissionName")
            ShowMissionName();        
    }
    private void MissionSignalOn()
    {
        Open("MISSION " + AllMissions.MissionActiveID);
        CloseAfter(2);
    }
    private void ShowMissionName()
    {
        Open(AllMissions.missions[AllMissions.MissionActiveID - 1].description.ToUpper());
        CloseAfter(4);
    }
    private void Open(string text)
    {
        SetOn();
        GetComponent<Animation>().Play("missionsAlertOpen");
        GetComponent<Animation>()["missionsAlertOpen"].normalizedTime = 0;
        field.text = text;		
	}
    void CloseAfter(int delay)
    {
        isClosing = true;
        Invoke("Close", delay);
	}
    public void Close()
    {
        if (!isClosing) return;
        isClosing = false;
        GetComponent<Animation>().Play("missionsAlertClose");
        GetComponent<Animation>()["missionsAlertClose"].normalizedTime = 0;
    }
}
