using UnityEngine;
using System.Collections;

public class AddFacebookAccount : MonoBehaviour {

    void Start()
    {
       SocialEvents.OnFacebookUserLoaded += OnFacebookUserLoaded;
        SocialEvents.OnFacebookIdAdded += OnFacebookIdAdded;
    }
    public void OnDestroy()
    {
       SocialEvents.OnFacebookUserLoaded -= OnFacebookUserLoaded;
        SocialEvents.OnFacebookIdAdded -= OnFacebookIdAdded;
    }
    public void AddAccount()
    {
        Data.Instance.GetComponentInChildren<FBHolder>().Login();
    }    
    void OnFacebookUserLoaded(string facebookID, string username)
    {
        Social.Instance.dataController.AddFacebookIdToExistingAccount( Data.Instance.GetComponent<UserData>().userId,  facebookID);
    }
    void OnFacebookIdAdded()
    {
        Back();
    }
    public void Back()
    {
        Fade.LoadLevel("MainMenu", 1, 1, Color.black);
    }
}
