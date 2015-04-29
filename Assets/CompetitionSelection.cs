using UnityEngine;
using System.Collections;

public class CompetitionSelection : MonoBehaviour {

    //public CompetitionButton uiButton;

    //private Data data;
    //private Vector2 buttonSize = new Vector2(200,40);
    //private float separationY = 140;
    //private float _x;
    //private float _y;
    //private Missions missions;
    //private int missionID;
    //public UIScrollBar scrollBar;
    //private float StartScrollPosition;
    //private CompetitionButton lastButton;
    //public bool showJoystickMenu;

    //void Start () {

    //    Data.Instance.events.OnInterfacesStart();

    //    data = GameObject.Find("data").GetComponent<Data>();		
    //    missions = GameObject.Find("data").GetComponent<Missions>();


    //    GameObject container = GameObject.Find("Container") as GameObject;

    //    missionID = 0;
    //    _y =0 ;

    //    //si jugas con joystick
    //    //if (!showJoystickMenu)
    //    //    GameObject.Find("ContainerJoystick").gameObject.SetActive(false);


    //    foreach(Mission mission in missions.missions)
    //    {
    //        if (mission.Hiscore > 0)
    //        {
    //            if (showJoystickMenu) return;

    //            Vector3 position = new Vector3(0, 0, 0);
    //            CompetitionButton button = Instantiate(uiButton, position, Quaternion.identity) as CompetitionButton;

    //            UIButtonKeys keys = button.GetComponent<UIButtonKeys>();

    //            button.transform.parent = container.transform;
    //            button.transform.localScale = new Vector3(1, 1, 1);
    //            button.transform.eulerAngles = new Vector3(0, 0, Random.Range(-3, 3));
    //            position.y = _y;
    //            button.transform.localPosition = position;

    //            if (missionID > data.levelUnlockedID && !Data.Instance.DEBUG)
    //            {
    //                button.disableButton();
    //            }

    //            _y += separationY;
    //            button.Init(missionID, mission.description, mission.avatarHiscore, mission.Hiscore);
    //            lastButton = button;  
    //        }
    //        missionID++;                      
    //    }
    //    SetScroll(1- ( (float)data.levelUnlockedID / (float)missionID) );        
    //}
    //void SetScroll(float levelID)
    //{
        
    //    StartScrollPosition = levelID;
    //    Invoke("onchange", 0.01f);
    //}
    //float lastID = 0;
    //void onchange()
    //{
    //    if (Data.Instance.mode == Data.modes.JOYSTICK)
    //    {
    //        float v = (float)StartScrollPosition;
    //        scrollBar.value = v;
    //    }
    //}
    //public void loadLevel(int num)
    //{
    //    data.missionActive = num+1;
    //    Fade.LoadLevel("Game", 1, 1, Color.black);
    //}
    //public void Back()
    //{
    //    Fade.LoadLevel("MainMenu", 1, 1, Color.black);
    //}
}
