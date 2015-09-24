using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level : MonoBehaviour {

    public SceneObject explotion;
    public SceneObject wallExplotion;

    public SceneObject explotionEffect;
	public SceneObject explotionGift;
	public Area startingArea;
	public GameObject limitObject;
    public ScoreSignal scoreSignal;

    public ProgressBar missionBar;
    public Text missionName;
    public Text missionDesc;

	private AreasManager areasManager;
	private FloorManager floorManager;
	//private RainManager rainManager;
	
	private float lastDistanceToLoadLevel; 
	static Area areaActive;
	static float areasLength = 0;
	private int nextPlatformSpace = 30;
	public SceneObjectsBehavior sceneObjects;
	Game game;

	private Area skyArea;
	private Missions missions;	
	private bool showStartArea;
	private Data data;
    private bool playing;
    private int areasX;
    public CharactersManager charactersManager; 
   

    private void Awake()
    {
    }
    public void Init()
	{
        areasX = 0;
        playing = true;
        areaActive = null;
        
        
		data = Data.Instance;
        game = Game.Instance;
        missions = data.GetComponent<Missions>(); 		
		//rainManager = GetComponent<RainManager>();
        charactersManager = game.GetComponent<CharactersManager>();
        floorManager = GetComponent<FloorManager>();
        floorManager.Init(charactersManager);
        missions.Init(data.missionActive, this);
        areasManager = missions.getAreasManager();
        areasManager.Init(1);

        areasLength = 0;

        missions.StartNext();

        data.events.OnResetLevel += reset;
        data.events.OnSetFinalScore += OnSetFinalScore;
        data.events.OnAddExplotion += OnAddExplotion;
        data.events.OnAddWallExplotion += OnAddWallExplotion;
        data.events.OnAddObjectExplotion += OnAddObjectExplotion;
        data.events.OnAddTumba += OnAddTumba;

    }
    
    public void OnDestroy()
    {
        data.events.OnResetLevel -= reset;
        data.events.OnSetFinalScore -= OnSetFinalScore;
        data.events.OnAddExplotion -= OnAddExplotion;
        data.events.OnAddWallExplotion -= OnAddWallExplotion;
        data.events.OnAddObjectExplotion -= OnAddObjectExplotion;
        data.events.OnAddTumba -= OnAddTumba;
    }
	public void Complete()
	{
		showStartArea = true;
		missions.Complete();
		missions.StartNext();
		areasManager = missions.getAreasManager();
		areasManager.Init(0);
		data.setMission(missions.MissionActiveID);        
	}
	private void  reset()
	{
        if (!playing) return;
        playing = false;
        sceneObjects.PoolSceneObjectsInScene();

        //Init();
	}
    public void OnAddObjectExplotion(Vector3 position, int type)
    {
        SceneObject explpotionEffect;
        switch (type)
        {
            case 1:
                explpotionEffect = ObjectPool.instance.GetObjectForType("ExplotionEffectBomb", true); break;
            case 2:
                explpotionEffect = ObjectPool.instance.GetObjectForType("ExplotionEffectEnemy", true); break;
            default:
                explpotionEffect  = ObjectPool.instance.GetObjectForType("ExplotionEffectSimpleObject", true); break;
        }
        if (explpotionEffect)
            explpotionEffect.Restart(position);
    }
    public void OnAddExplotion(Vector3 position)
    {
        OnAddExplotion(position, explotion.name, explotionEffect.name, explotionGift.name);
    }
    public void OnAddWallExplotion(Vector3 position)
    {
        OnAddExplotion(position, wallExplotion.name, "ExplotionEffectWall", explotionGift.name);
    }
    public void OnAddExplotion(Vector3 position, string _name, string _explotionEffect, string _explotionGift)
	{
        Vector3 newPos = position;
        newPos.y -= 4;

        SceneObject explotionNew = ObjectPool.instance.GetObjectForType(_name, true);
        if (explotionNew)
            explotionNew.Restart(newPos);

        if (_explotionEffect != "")
        {
            SceneObject explpotionEffect = ObjectPool.instance.GetObjectForType(_explotionEffect, true);
            if (explpotionEffect)
                explpotionEffect.Restart(newPos);
        }
    
		newPos = position;
		newPos.y +=0.5f;
        for (int a = 0; a < 6; a++)
        {
            SceneObject newSO = ObjectPool.instance.GetObjectForType(_explotionGift, true);
            if (newSO)
            {
                newSO.Restart(newPos);
                newPos.x += a * 0.05f;
                newSO.GetComponent<Rigidbody>().AddForce(Vector3.forward * 700);
                newSO.GetComponent<Rigidbody>().AddForce(Vector3.up * 800);
            }
        }
	}
	private void  createNextArea(Area area)
	{
        if (areaActive)
            areasLength += areaActive.z_length / 2;
		areaActive = area;
        areasLength += area.z_length / 2;
               
        sceneObjects.replaceSceneObject(area, areasLength - 4, areasX);
        areasX += area.nextAreaX; 
	   
	}
	
	private void Update () {

        if (areasLength==0)
       {
           createNextArea(areasManager.getStartingArea());
       } else

            if (charactersManager.getDistance() > (areasLength - nextPlatformSpace)
		&&
        lastDistanceToLoadLevel != charactersManager.getDistance())
		{
            lastDistanceToLoadLevel = charactersManager.getDistance();	
			Area newArea;
			if(showStartArea)
			{
				newArea = areasManager.getRandomArea(true);
				showStartArea = false;
			} else 
			{
				newArea = areasManager.getRandomArea(false);
			}	
			createNextArea(newArea);
		}
	}
    public void FallDown(int fallDownHeight)
    {
        GameCamera camera = game.gameCamera;
        camera.fallDown(fallDownHeight);
    }
    public void OnSetFinalScore(Vector3 position, int score)
    {
        if (position == Vector3.zero) return;
        SceneObject newSO = Instantiate(scoreSignal, position, Quaternion.identity) as SceneObject;
        newSO.Restart(position);
        newSO.GetComponent<ScoreSignal>().SetScore(score);
    }
    public void addSceneObjectToScene(SceneObject _so, Vector3 position)
    {
        SceneObject so = Instantiate(_so, position, Quaternion.identity) as SceneObject;
        so.Restart(so.transform.position);
    }



    public void OnAddTumba(Vector3 position, string username, string facebookID)
    {
        print("_________________OnAddTumba");
        Vector3 newPos = position;
        newPos.y -= 4;

        SceneObject explotionNew = ObjectPool.instance.GetObjectForType("Tumba_real", true);
        if (explotionNew)
            explotionNew.Restart(newPos);
    }
}
