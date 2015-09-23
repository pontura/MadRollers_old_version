﻿using UnityEngine;
using System.Collections;

public class CompetitionSelection : MonoBehaviour {

    public GameObject popup;

    void Start()
    {
        popup.SetActive(false);
        SocialEvents.OnFacebookUserLoaded += OnFacebookUserLoaded;
    }
    void OnDesrtroy()
    {
        popup.SetActive(false);
        SocialEvents.OnFacebookUserLoaded -= OnFacebookUserLoaded;
    }
    void OnFacebookUserLoaded(string a, string b)
    {
        Data.Instance.LoadLevel("MainMenu");
    }
    public void GotoGame()
    {
        if (Data.Instance.userData.isPlayerDataLogged())
        {
            StartGame();
        }
        else
        {
            popup.SetActive(true);
            popup.animation.Play("GameMenuOpen");
        }
    }
    public void Login()
    {
        SocialEvents.FBLogin();
    }
    public void StartGame()
    {
        Data.Instance.LoadLevel("CompetitionGoal");
    }
    public void ClosePopup()
    {
        Invoke("CloseReady", 1);
        popup.animation.Play("GameMenuClose");
    }
    public void Back()
    {
        Data.Instance.LoadLevel("MainMenu");
    }
    void CloseReady()
    {
        popup.SetActive(false);
    }
}
