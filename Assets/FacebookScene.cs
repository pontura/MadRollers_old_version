using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class FacebookScene : MonoBehaviour {
    
    public ProfilePicture profilePicture;
    public GameObject MyAvatar;
    public GameObject FB_Username;

    public GameObject ButtonCompite;
    public GameObject ButtonPlay;

    public Text hiscore;
    private List<object> scoresList = null;

    [SerializeField] FriendButton friendButton;

    private Dictionary<string, string> profile = null;

    public void Init(bool IsLoggedIn)
    {
        if (IsLoggedIn)
        {
            SetActive(true);
        }
        else
        {
            SocialEvents.OnFacebookUserLoaded += OnFacebookUserLoaded;
            SetActive(false);
        }
    }
    void OnDestroy()
    {
       SocialEvents.OnFacebookUserLoaded -= OnFacebookUserLoaded;
    }
    void SetActive(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            FB_Username.GetComponent<Text>().text = Data.Instance.userData.username;
            hiscore.text = Data.Instance.userData.hiscore.ToString();
            profilePicture.setPictre(Data.Instance.userData.facebookId);

           // ButtonCompite.SetActive(false);
        }
        else
        {
            MyAvatar.SetActive(false);
        }
    }
    void OnFacebookUserLoaded(string id, string username)
    {
        SetActive(true);
    }
}
