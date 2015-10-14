using UnityEngine;
using System.Collections;

public class MissionsTopPanel : MonoBehaviour
{
    void Start()
    {
        if (Data.Instance.playMode == Data.PlayModes.STORY)
        {
            Data.Instance.events.OnMissionComplete += OnMissionComplete;
            Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
            GetComponent<Animation>().Play("MissionTopOff");
        } else
            GetComponent<Animation>().Play("MissionTopOpen");
    }
    void OnDisable()
    {
        Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;
        Data.Instance.events.OnMissionComplete -= OnMissionComplete;
    }
    private void OnMissionComplete(int levelID)
    {
        GetComponent<Animation>().Play("MissionTopClose");
    }
    private void OnListenerDispatcher(string message)
    {
       if (message == "ShowMissionName" )
            GetComponent<Animation>().Play("MissionTopOpen");
    }
}
