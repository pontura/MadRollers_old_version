using UnityEngine;
using System.Collections;

public class Events : MonoBehaviour {

    //public System.Action<string> OnFacebookNewUserLogged = delegate { };
    //public System.Action OnFacebookIdAdded = delegate { };
    //public System.Action<string, string> OnFacebookUserLoaded = delegate { };
    //public System.Action<string, int, int, bool> OnSetUserData = delegate { };

    //public System.Action OnHiscoresLoaded = delegate { };

    //public System.Action<int> OnHiscore = delegate { };   


    public System.Action<float> SetVolume = delegate { };
    public System.Action<bool> OnFadeALittle = delegate { };
    public System.Action OnInterfacesStart = delegate { };
    public System.Action OnGameStart = delegate { };
    public System.Action<bool> OnGamePaused = delegate { };

    public System.Action<int, int> OnSetStarsToMission = delegate { };
    
    public void MissionStart(int levelID) { OnMissionStart(levelID); }
    public System.Action<int> OnMissionStart = delegate { };

    public System.Action<int> OnMissionComplete = delegate { };
    public void MissionComplete() { OnMissionComplete(Data.Instance.missionActive); }    

    public System.Action<string> OnListenerDispatcher = delegate { };
    public void ListenerDispatcher(string message) { OnListenerDispatcher(message); }

    public System.Action<Vector3, int> OnSetFinalScore = delegate { };
    public System.Action<Vector3, int> OnScoreOn = delegate { };
    public void ScoreSignalOn(Vector3 position, int score) { OnScoreOn(position, score); }

    public System.Action<Vector3> OnAddExplotion = delegate { };
    public void AddExplotion(Vector3 position) { OnAddExplotion(position); }

    public System.Action<Vector3, int> OnAddObjectExplotion = delegate { };
    public System.Action<Vector3, string, string> OnAddTumba = delegate { };  

    public System.Action<Vector3> OnAddWallExplotion = delegate { };
    public void AddWallExplotion(Vector3 position) { OnAddWallExplotion(position); }
    
    public System.Action OnOpenMainMenu = delegate { };
    public System.Action OnCloseMainmenu = delegate { };

    public System.Action OnResetLevel = delegate { };

    public System.Action<string> OnAvatarGetItem = delegate { };
    public System.Action<Player.fxStates> OnAvatarChangeFX = delegate { };
    public System.Action<CharacterBehavior> OnAvatarCrash = delegate { };
    public System.Action<CharacterBehavior> OnAvatarFall = delegate { };
    public System.Action<CharacterBehavior> OnAvatarDie = delegate { };
    public System.Action OnAvatarProgressBarEmpty = delegate { };

    public System.Action OncharacterCheer = delegate { };

    public System.Action OnAvatarJump = delegate { };
    public void AvatarJump() { OnAvatarJump(); }

    public System.Action OnAvatarShoot = delegate { };

    public System.Action OnCompetitionMissionComplete = delegate { };

    public System.Action<int> OnCurvedWorldIncreaseBend = delegate { };
    public System.Action<int> OnCurvedWorldTurn = delegate { };

    public System.Action<int> OnSetNewAreaSet = delegate { };
}
