using UnityEngine;
using System.Collections;

public class RegistryScreen : MonoBehaviour {

    void Start()
    {
        Data.Instance.events.OnFacebookUserLoaded += OnFacebookUserLoaded;
        Data.Instance.events.OnFacebookNewUserLogged += OnFacebookNewUserLogged;
    }
    void OnDestroy()
    {
        Data.Instance.events.OnFacebookUserLoaded -= OnFacebookUserLoaded;
        Data.Instance.events.OnFacebookNewUserLogged -= OnFacebookNewUserLogged;
    }
    void OnFacebookUserLoaded(string id)
    {
        Data.Instance.GetComponent<DataController>().CheckIfFacebookIdExists(id);
        // trae: Data.Instance.events.OnFacebookNewUserLogged += OnFacebookNewUserLogged;

        Back();
    }
    void OnFacebookNewUserLogged(string facebookID)
    {
        Data.Instance.GetComponent<DataController>().CreateUserByFacebookID(facebookID);
    }
    public void FacebookLogin()
    {
        // Si estas en la web y te llegó el facebook id anteriormente:
        if (Data.Instance.userData.facebookId != "")
            OnFacebookNewUserLogged(Data.Instance.userData.facebookId);
        if (Application.isWebPlayer)
            Application.ExternalCall("FBLogin");
        else
            Data.Instance.GetComponentInChildren<FBHolder>().Login();

    }
    public void SimpleLogin()
    {
        Fade.LoadLevel("SimpleLogin", 1, 1, Color.black);
    }
    public void Back()
    {
        Fade.LoadLevel("MainMenu", 1, 1, Color.black);
    }
}
