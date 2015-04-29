using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelSelector : MonoBehaviour {

    public int levelUnlockedID;
    public MissionButton uiButton;
    [SerializeField]
    GameObject container;
	private Data data;
	private Vector2 buttonSize = new Vector2(200,40);
    public float separationY;
	private float _x;
	private float _y;
	private Missions missions;
	private int missionID;
    private float StartScrollPosition;
    private MissionButton lastButton;
    public bool showJoystickMenu;

    private float starting_Y;
    private float separationX = 150;
    public float containerHeight = 0;

	void Start () {
        starting_Y = container.transform.localPosition.y;
        
        Data.Instance.events.OnInterfacesStart();

		data = GameObject.Find("data").GetComponent<Data>();		
		missions = GameObject.Find("data").GetComponent<Missions>();


       // GameObject container = GameObject.Find("Container") as GameObject;

		missionID = 0;
		_y =0 ;

        //si jugas con joystick
        //if (!showJoystickMenu)
        //    GameObject.Find("ContainerJoystick").gameObject.SetActive(false);


		foreach(Mission mission in missions.missions)
		{
            //si jugas con joystick
            if (showJoystickMenu) return;
                 
            Vector3 position = new Vector3(0, 0, 0);
            MissionButton button = Instantiate(uiButton, position, Quaternion.identity) as MissionButton;
            
            //UIButtonKeys keys = button.GetComponent<UIButtonKeys>();

            button.transform.SetParent(container.transform) ;
            button.transform.localScale = new Vector3(1,1,1);
            button.transform.eulerAngles = new Vector3(0, 0, Random.Range(-3,3));
            position.y = _y;

            if (missionID % 2 == 0 )
            {
                position.x = 0;
            }
            else
            {
                position.x = separationX;
                _y -= separationY;
                containerHeight = -_y;
            }

            button.transform.localPosition = position;

            if (missionID > data.levelUnlockedID && !Data.Instance.DEBUG)
            {
                button.disableButton();
            }
            
            button.Init(missionID, mission.description.ToUpper());

            missionID++;
            lastButton = button;
            
       
		}
        levelUnlockedID = data.levelUnlockedID;
        SetScroll( data.levelUnlockedID );
        
	}
    void SetScroll(float levelID)
    {
        if (levelID < 6) return;
        Vector3 pos = container.transform.localPosition;
        pos.y = (levelUnlockedID / 2 * separationY);
        container.transform.localPosition = pos;
    }
    float lastID = 0;

    void Update()
    {
        Vector3 pos = container.transform.localPosition;
        if (pos.y < starting_Y)
            pos.y = starting_Y;
        else if (pos.y > containerHeight - starting_Y)
            pos.y = containerHeight - starting_Y;
        container.transform.localPosition = pos;
    }
    //void Update()
    //{
    //    if (Data.Instance.mode == Data.modes.JOYSTICK)
    //    {
    //        if (UICamera.selectedObject != null)
    //        {
    //            GameObject mb = UICamera.selectedObject;
    //            float id = mb.GetComponent<MissionButton>().id;
    //            if (lastID == id) return;

    //            lastID = id;
    //            StartScrollPosition = (float)id / (float)(missions.missions.Length - 1);
    //            SetScroll(1 - StartScrollPosition);

    //        }
    //    }
    //}
    //void onchange()
    //{
    //    if (Data.Instance.mode == Data.modes.JOYSTICK)
    //    {
    //        float v = (float)StartScrollPosition;
    //        scrollBar.value = v;
    //    }
    //}
    public void loadLevel(int num)
    {
        print("loadLevel " + num);
        data.missionActive = num+1;
        Fade.LoadLevel("Game", 1, 1, Color.black);
    }
    public void Back()
    {
        Fade.LoadLevel("MainMenu", 1, 1, Color.black);
    }
}
