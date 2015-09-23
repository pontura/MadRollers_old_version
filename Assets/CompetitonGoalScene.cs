using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CompetitonGoalScene : MonoBehaviour {

    public Text username1;
    public Text score1;

    public Text username2;
    public Text score2;

    public ProfilePicture avatar1;
    public ProfilePicture avatar2;

	void Start () {

        Data.Instance.events.OnGamePaused(true);

        Hiscores.Hiscore goalHiscore = Social.Instance.hiscores.GetMyNextGoal();
        username1.text = goalHiscore.username;
        score1.text = goalHiscore.score.ToString() + " Mts";
        avatar1.SetPicture(goalHiscore.facebookID);

        username2.text = Data.Instance.userData.username;
        score2.text = Social.Instance.hiscores.GetMyScore().ToString();
        avatar2.SetPicture(Data.Instance.userData.facebookId);

        Invoke("StartGame", 3.5f);
	}
    void StartGame()
    {
        Data.Instance.LoadLevel("Game");
    }
}
