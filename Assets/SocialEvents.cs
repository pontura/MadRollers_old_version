using UnityEngine;
using System.Collections;

public static class SocialEvents
{
    public static System.Action<string> OnFacebookNewUserLogged = delegate { };
    public static System.Action OnFacebookIdAdded = delegate { };
    public static System.Action<string, string> OnFacebookUserLoaded = delegate { };
    public static System.Action<string, int, int, bool> OnSetUserData = delegate { };

    public static System.Action OnHiscoresLoaded = delegate { };
    public static System.Action OnGameStart = delegate { };
    public static System.Action<bool> OnGamePaused = delegate { };

    public static System.Action<int> OnHiscore = delegate { };
    public static System.Action<int> OnMissionStart = delegate { };
    public static System.Action OnInterfacesStart = delegate { };
}
