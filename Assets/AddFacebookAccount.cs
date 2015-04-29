using UnityEngine;
using System.Collections;

public class AddFacebookAccount : MonoBehaviour {

    void Start()
    {
        Data.Instance.events.OnFacebookUserLoaded += OnFacebookUserLoaded;
        Data.Instance.events.OnFacebookIdAdded += OnFacebookIdAdded;
    }
    public void OnDestroy()
    {
        Data.Instance.events.OnFacebookUserLoaded -= OnFacebookUserLoaded;
        Data.Instance.events.OnFacebookIdAdded -= OnFacebookIdAdded;
    }
    public void AddAccount()
    {
        Data.Instance.GetComponentInChildren<FBHolder>().Login();
    }    
    void OnFacebookUserLoaded(string facebookID)
    {
        Data.Instance.GetComponent<DataController>().AddFacebookIdToExistingAccount( Data.Instance.GetComponent<UserData>().userId,  facebookID);
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
