using UnityEngine;
using System.Collections;

public class CurvedWorldManager : MonoBehaviour {

    public int MAX_BENDING = -40;
    public int Bending_Start;

    public float newBending;
    public float newTurn;

    private float bending;
    private float turn;


	public void Init () {
        Data.Instance.events.OnGameStart += OnGameStart;
        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnCurvedWorldIncreaseBend += OnCurvedWorldIncreaseBend;
        Data.Instance.events.OnSetNewAreaSet += OnSetNewAreaSet;
	}
    void OnSetNewAreaSet(int areaSet)
    {
        print("OnSetNewAreaSet :" + areaSet);
        if (areaSet < 2) return;

        Data.Instance.events.OnCurvedWorldIncreaseBend(-1);
        int rand = Random.Range(0, 16);
        if (rand > 10) newTurn = 0;
        else
            newTurn = (rand-5)*8;
    }
    void OnCurvedWorldTurn(int _newTurn)
    {
        this.newTurn = _newTurn;
    }
    void OnGameStart()
    {
        bending = Bending_Start;
        turn = 0;

        newTurn = turn;
        newBending = bending;

        VacuumShaders.CurvedWorld.CurvedWorld_GlobalController.get._V_CW_X_Bend_Size_GLOBAL = newBending;
        VacuumShaders.CurvedWorld.CurvedWorld_GlobalController.get._V_CW_Y_Bend_Size_GLOBAL = turn;
    }
    void OnCurvedWorldIncreaseBend(int _newBending)
    {
        this.newBending += _newBending;
        if (newBending < MAX_BENDING) newBending = MAX_BENDING;
    }
    void OnAvatarCrash(CharacterBehavior cb)
    {
        turn = newTurn;
    }
	
	void Update () {
        if (newTurn > turn) turn += 0.05f;
        else if (newTurn < turn) turn -= 0.05f;
       // if(bending!=newBending)
            VacuumShaders.CurvedWorld.CurvedWorld_GlobalController.get._V_CW_X_Bend_Size_GLOBAL = newBending;

        if (turn != newTurn)
            VacuumShaders.CurvedWorld.CurvedWorld_GlobalController.get._V_CW_Y_Bend_Size_GLOBAL = turn;

       
	}
}
