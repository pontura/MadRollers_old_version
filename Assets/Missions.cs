using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Missions : MonoBehaviour {

    public Mission test_area;

	public Mission[] missions;
    public Competitions competitions;
	public int MissionActiveID = 0;

    public Mission MissionActive;
	private float missionCompletedPercent = 0;

	private ProgressBar progressBar;    

    private states state;
    private enum states
    {
        INACTIVE,
        ACTIVE
    }

    private Text name_txt;
    private Text desc_txt;
	//private Transform background;
	private Level level;
	private bool showStartArea;
    private Data data;
    private int lastDistance = 0;
    private int distance;

    public void Init()
    {
        data = Data.Instance;
        Data.Instance.events.OnScoreOn += OnScoreOn;
    }
    void OnDestroy()
    {
        Data.Instance.events.OnScoreOn -= OnScoreOn;
    }

    public void OnDisable()
    {
        data.events.OnListenerDispatcher -= OnListenerDispatcher;
    }
    private void OnListenerDispatcher(string message)
    {        
       if (message == "ShowMissionName")
            activateMissionByListener();        
    }
	public void Init (int _MissionActiveID, Level level) {

       
        data.events.OnListenerDispatcher += OnListenerDispatcher;
        state = states.INACTIVE; 

		this.missionCompletedPercent = 0;
		this.MissionActiveID = _MissionActiveID-1;

        this.level = level;
        progressBar = level.missionBar;

        name_txt = level.missionName;
        desc_txt = level.missionDesc;

        if (data.DEBUG && test_area)
        {
            MissionActive = test_area;
            MissionActive.reset();
        }
        else
        {
          //  print("busca : " + MissionActiveID);
            MissionActive = GetActualMissions()[MissionActiveID];
            MissionActive.reset();
        }

        if (MissionActive.isCompetition)
        {
            _MissionActiveID = Random.Range(1, GetActualMissions().Length + 1);
          //  print("_________bUSCA MISION RNDOM: " + _MissionActiveID);
        }


	}
    public Mission[] GetActualMissions()
    {
        if (Data.Instance.playMode == Data.PlayModes.COMPETITION)
            return competitions.GetMissions();
        else return missions;

    }
	public AreasManager getAreasManager()
	{
		return MissionActive.GetComponent<AreasManager>();
	}
	public void Complete()
	{
        data.events.MissionComplete();
        state = states.INACTIVE;        
	}
	public void StartNext()
	{
        if (MissionActiveID == GetActualMissions().Length)
        {
            MissionActiveID = Random.Range(2, GetActualMissions().Length-1);
        }
        MissionActive = GetActualMissions()[MissionActiveID];
		MissionActive.reset();
		MissionActiveID++;
	}
    private void activateMissionByListener()
    {
        state = states.ACTIVE;
        if (MissionActive.Hiscore > 0)
        {
            name_txt.text = MissionActive.avatarHiscore;
            desc_txt.text = "SCORE: " + MissionActive.Hiscore; 
        }
        else
        {
            name_txt.text = "MISSION " + MissionActiveID;
            desc_txt.text = MissionActive.description.ToUpper();
        }
        
        MissionActive.points = 0;
        lastDistance = (int)Game.Instance.GetComponent<CharactersManager>().getMainCharacter().distance;
    }




    private void OnScoreOn(Vector3 pos, int qty)
    {
        if (MissionActive.Hiscore > 0)
        {
            addPoints(qty);
            setMissionStatus(MissionActive.Hiscore);
        }
    }
    //lo llama el player
    public void updateDistance(float qty)
    {
        if (state == states.INACTIVE) return;
        distance = (int)qty - lastDistance;
        if (MissionActive.distance > 0)
        {
            setPoints(distance);
            setMissionStatus(MissionActive.distance);
        }
    }
	public void killGuy (int qty) {
		if(MissionActive.guys > 0)
		{
            addPoints(qty);		
			setMissionStatus(MissionActive.guys);
		}
	}
	public void killPlane() {
		if(MissionActive.planes > 0)
		{
            addPoints(1);		
			setMissionStatus(MissionActive.planes);
		}
	}
	public void killBomb(int qty) {
		if(MissionActive.bombs > 0)
		{
            addPoints(qty);
			setMissionStatus(MissionActive.bombs);
		}
	}

	public void getHeart (int qty) {
		if(MissionActive.hearts > 0)
		{
            addPoints(qty);
			setMissionStatus(MissionActive.hearts);
		}
	}
    void addPoints(float qty)
    {
        if (state == states.INACTIVE) return;
        MissionActive.addPoints(qty);
    }
    void setPoints(float points)
    {
        if (state == states.INACTIVE) return;
        MissionActive.setPoints((int)points);
    }
	void setMissionStatus(int total)
	{
        if (state == states.INACTIVE) return;
		missionCompletedPercent = MissionActive.points * 100 / total;
		progressBar.setProgression(missionCompletedPercent);
		if(missionCompletedPercent >= 100)
		{
            lastDistance = distance;
			level.Complete();
            progressBar.reset();
		}
	}
}
