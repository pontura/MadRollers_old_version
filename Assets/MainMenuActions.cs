using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuActions : MonoBehaviour {

    public Text PlayLabel;
    public Text DebugText;
    public ProfilePicture WinnerPicture;
    private bool rankingLoaded;

	void Start () {
        DebugText.text = "username: " + Data.Instance.userData.username + " - id: " + Data.Instance.userData.facebookId + " - id: " + Data.Instance.userData.userId;
        Data.Instance.GetComponent<Tracker>().TrackScreen("Main Menu");
        Data.Instance.events.OnInterfacesStart();

        if (Application.platform == RuntimePlatform.Android)
            Data.Instance.mode = Data.modes.ACCELEROMETER;

        FacebookScene facebookScene = GetComponent<FacebookScene>();

        facebookScene.Init( Data.Instance.userData.isPlayerDataLogged() );
    }
    void Update()
    {
        if (rankingLoaded) return;
        if (Social.Instance.hiscores.levels.Count == 0) return;

        rankingLoaded = true;
        if (Social.Instance.hiscores.levels[0].hiscore[0].profilePicture)
        {
            WinnerPicture.SetLoadedPicture(Social.Instance.hiscores.levels[0].hiscore[0].profilePicture);
        }
        else
        {
            WinnerPicture.SetPicture(Social.Instance.hiscores.levels[0].hiscore[0].facebookID);
        }
    }
    public void Compite()
    {
        Data.Instance.playMode = Data.PlayModes.COMPETITION;
        Data.Instance.LoadLevel("Competitions");
        Data.Instance.levelUnlockedID = Data.Instance.competitions.GetUnlockedLevel();
    }
    public void MissionsScene()
    {
        Data.Instance.playMode = Data.PlayModes.STORY;
        Fade.LoadLevel("LevelSelector", 1, 1, Color.black);
        Data.Instance.levelUnlockedID = PlayerPrefs.GetInt("levelUnlocked_0");
    }
    public void Registry()
    {
        if (Data.Instance.userData.userId == 0)
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
    public void Logout()
    {
        if(FB.IsLoggedIn)
            FB.Logout();
        PlayerPrefs.DeleteAll();
        Fade.LoadLevel("MainMenu", 1, 1, Color.black);
    }

}
