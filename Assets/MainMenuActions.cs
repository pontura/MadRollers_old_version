using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuActions : MonoBehaviour {

    public Text PlayLabel;

	void Start () {
        Data.Instance.GetComponent<Tracker>().TrackScreen("Main Menu");
        Data.Instance.events.OnInterfacesStart();

        if (Application.platform == RuntimePlatform.Android)
            Data.Instance.mode = Data.modes.ACCELEROMETER;

        FacebookScene facebookScene = GetComponent<FacebookScene>();

        facebookScene.Init( Data.Instance.userData.isPlayerDataLogged() );
    }    
    public void MissionsScene()
    {
        Fade.LoadLevel("LevelSelector", 1, 1, Color.black);
    }
    public void Registry()
    {
        UserData userData = Data.Instance.GetComponent<UserData>();
        if (userData.userId == 0)
            Fade.LoadLevel("Registry", 1, 1, Color.black);
    }
    public void About()
    {
        Fade.LoadLevel("About", 1, 1, Color.black);
    }
    public void AddFacebookIDToMyAccount()
    {
        if (Data.Instance.GetComponent<UserData>().facebookId == "")
        {
            Fade.LoadLevel("AddFacebookLogin", 1, 1, Color.black);
        }
    }

}
