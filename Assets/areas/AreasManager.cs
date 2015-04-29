using UnityEngine;
using System.Collections;

public class AreasManager : MonoBehaviour {

	public Area startingArea;
	public int num = 0;

	public Area[] testingAreas;	
	public AreaSet testingAreaSet;	

	public AreaSet[] areaSets;
	public Area[] skyAreas;

	public int activeAreaSetID = 1;

	private AreaSet areaSet;

    void Start()
    {
        num = 0;
    }
	public void Init(int _activeAreaSetID)
	{
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
		areaSet = areaSets[activeAreaSetID];
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

        if (!areaSet)
        {
            areaSet = areaSets[0];
            areaSet.id = 0;
        }
		Area area;

		if(startingArea)
		{
			area = getStartingArea();
			num = 0;
		} else 
		if(testingAreas.Length > 0)
			area = testingAreas[Random.Range(0, testingAreas.Length)];
		else
		if(testingAreaSet)
		{
			areaSet = testingAreaSet;
			area = areaSet.getArea();
			changeCameraOrientation();
		}
		else
		{
            if (activeAreaSetID < areaSets.Length && num >= areaSet.totalAreasInSet)
            {
                setNewAreaSet();
                activeAreaSetID++;
                num = 0;
            }
           
            area = areaSet.getArea();
		}
		return area;
	}
	public Area getRandomSkyArea () {
		Area area;
		area = skyAreas[Random.Range(0, skyAreas.Length)];
		return area;
	}
	public Area getStartingArea()
	{
		return startingArea;
	}
}
