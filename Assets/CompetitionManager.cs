using UnityEngine;
using System.Collections;

public class CompetitionManager : MonoBehaviour {

	public GameObject avatars;
    public ProfilePicture avatar1;
    public ProfilePicture avatar2;
    private CharacterBehavior characterBehavior;
    private bool isCompetition;
    public float distance;
    public int nextGoalDistance;

    void Start()
    {
        if (Data.Instance.playMode == Data.PlayModes.STORY)
        {
            avatars.SetActive(false);
        }
        else
        {
            isCompetition = true;
            if (Data.Instance.userData.facebookId != "0")
                avatar1.SetPicture(Data.Instance.userData.facebookId);

            characterBehavior = GetComponent<CharactersManager>().character;

            SetGoal();
        }
    }
    void SetGoal()
    {
        Hiscores.Hiscore goalHiscore = Social.Instance.hiscores.GetMyNextGoal();
        nextGoalDistance = goalHiscore.score;
        avatar2.SetPicture(goalHiscore.facebookID);
        Data.Instance.missions.MissionActive.distance = nextGoalDistance;
    }
    void Update()
    {
        if (!isCompetition) return;

        distance = characterBehavior.distance;
    }
}
