using UnityEngine;
using System.Collections;

public class About : MonoBehaviour {

    void Start()
    {
        Data.Instance.GetComponent<Tracker>().TrackScreen("About Screen");
    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            Fade.LoadLevel("MainMenu", 1, 1, Color.black);
        }
    }
    public void Back()
    {
        Fade.LoadLevel("MainMenu", 1, 1, Color.black);
    }
}
