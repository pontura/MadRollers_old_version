using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AreasManager : MonoBehaviour {

	public Area startingArea;
    private int num = 0;

    public bool isCompetition;

	public List<AreaSet> areaSets;
    public List<AreaSet> areaRandomSets;
	//public Area[] skyAreas;

	private int activeAreaSetID = 1;

	private AreaSet areaSet;

    void Start()
    {
        num = 0;
       // RandomizeAreaSetsByPriority();
      
    }
    public void RandomizeAreaSetsByPriority()
    {
       
        areaRandomSets = Randomize(areaRandomSets);
        areaRandomSets = areaRandomSets.OrderBy(x => x.competitionsPriority).ToList();
        areaRandomSets.Reverse();
    }
    private List<AreaSet> Randomize(List<AreaSet> toRandom)
    {
        Random.seed = Social.Instance.hiscores.GetMyScore();
        for (int i = 0; i < toRandom.Count; i++)
        {
            AreaSet temp = toRandom[i];
            int randomIndex = Random.Range(i, toRandom.Count);
            toRandom[i] = toRandom[randomIndex];
            toRandom[randomIndex] = temp;
        }
        return toRandom;
    }
	public void Init(int _activeAreaSetID)
	{
        areaRandomSets.Clear();
        foreach (AreaSet areaSet in areaSets)
        {
            areaRandomSets.Add(areaSet);
        }
        if (isCompetition)
            RandomizeAreaSetsByPriority();
		num = 0;
		activeAreaSetID = 0;
		setNewAreaSet();
	}
	
	private int getDifferentLevel()
	{
		return 2;
	}
	private void setNewAreaSet()
	{
        if(Data.Instance.playMode == Data.PlayModes.COMPETITION)
            Random.seed = Social.Instance.hiscores.GetMyScore();
        //if (areaSet.competitionsPriority == 0)
        //{
        //    activeAreaSetID = Random.Range(2, areaSets.Count - 1);
        //    activeAreaSetID++;
        //}

        areaSet = areaRandomSets[activeAreaSetID];
		//changeCameraOrientation();
        areaSet.id = 0;
       
	}
	private void changeCameraOrientation()
	{
		//GameCamera camera = GameObject.Find("Camera").GetComponent<GameCamera>();
		//camera.setOrientation( areaSet.getCameraOrientation(), 23);
	}
	public Area getRandomArea (bool startingArea) {
        num++;

      //  print(areaSet + "areaSets.Length: " + areaSets.Length + "  activeAreaSetID: " + activeAreaSetID + " num: " + num + " areaSet.totalAreasInSet " + areaSet.totalAreasInSet);
        if (!areaSet)
        {
            areaSet = areaRandomSets[0];
            areaSet.id = 0;
        }
		Area area;

        if (startingArea)
		{
			area = getStartingArea();
			num = 0;
		} 
		else
		{
            if (Data.Instance.playMode == Data.PlayModes.STORY)
            {
                if (activeAreaSetID < areaRandomSets.Count && num >= areaSet.totalAreasInSet)
                {
                    if (num >= areaSet.totalAreasInSet)
                    {
                        setNewAreaSet();
                        activeAreaSetID++;
                        num = 0;
                    }
                }
            } else 
            if (num >= areaSet.totalAreasInSet)
                {
                    Debug.Log("__setNewAreaSet__");
                    Data.Instance.events.OnSetNewAreaSet(activeAreaSetID);
                    setNewAreaSet();
                    activeAreaSetID++;
                    num = 0;
            }
           
            area = areaSet.getArea();
		}
		return area;
	}
	public Area getRandomSkyArea () {
		Area area = null;
		//area = skyAreas[Random.Range(0, skyAreas.Length)];
		return area;
	}
	public Area getStartingArea()
	{
		return startingArea;
	}
}
