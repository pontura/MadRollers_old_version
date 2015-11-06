using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Game game;
	private Gui gui;

    public GameObject particles;

    public int id; //numero de player;
    public EnergyBar progressBar;

    [HideInInspector]
    public Weapon weapon;
    public Weapon[] weapons;
    public GameObject weaponContainer;

    [HideInInspector]
    public Transport transport;
    public Transport[] transports;
    public GameObject transportContainer;

    public fxStates fxState;

    public bool canJump = true;
    public bool canShoot = true;

    public enum fxStates
    {
        NORMAL,
        SUPER
    }

   // public float energy = 90;
    private Material originalMaterial;

 //   public EnergyBar energyBar;

    void OnDestroy()
    {
        Data.Instance.events.OnMissionStart -= OnMissionStart;
        Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;
        Data.Instance.events.OnAvatarGetItem -= OnAvatarGetItem;
        Data.Instance.events.OnAvatarProgressBarEmpty -= OnAvatarProgressBarEmpty;
    }
    public void Init(int id)
    {
        if (id == 0)
            originalMaterial = Resources.Load("Materials/BolaHead", typeof(Material)) as Material;
        else
            originalMaterial = Resources.Load("Materials/Piel2", typeof(Material)) as Material;

       // setStartingState();
      //  Invoke("setStartingState2", 1);

        Data.Instance.events.OnMissionStart += OnMissionStart;
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
        Data.Instance.events.OnAvatarGetItem += OnAvatarGetItem;
        Data.Instance.events.OnAvatarProgressBarEmpty += OnAvatarProgressBarEmpty;

        this.id = id;
       // this.energyBar = energyBar;
        weapon = Instantiate(weapons[0] as Weapon, Vector3.zero, Quaternion.identity) as Weapon;
        weapon.transform.parent = weaponContainer.transform;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localScale = Vector3.one*2;
        particles.SetActive(false);
        OnAvatarProgressBarEmpty();
    }
    public void OnAvatarProgressBarStart(Color color)
    {
        if (progressBar.isOn) return;
        progressBar.Init(color);
        progressBar.gameObject.SetActive(true);
    }
    public void OnAvatarProgressBarEmpty()
    {

        print("OnAvatarProgressBarEmpty " + fxState);
        progressBar.gameObject.SetActive(false);

        if (fxState == fxStates.SUPER)
        {
            setNormalState();
            return;
        }

        foreach (Transform child in transportContainer.transform)  Destroy(child.gameObject);

        transport = null;
    }
    public void OnAvatarProgressBarUnFill(float qty )
    {
        progressBar.UnFill(qty);
    }
    private void OnAvatarGetItem(Powerup.types item)
    {
        if (item == Powerup.types.MISSILE)
        {
            canShoot = true;
            weapon.setOn();
        }
        else if (item == Powerup.types.JETPACK)
        {
            transport = Instantiate(transports[0] as Transport, Vector3.zero, Quaternion.identity) as Transport;
            transport.transform.parent = transportContainer.transform;
            transport.transform.localPosition = Vector3.zero;
            transport.transform.localEulerAngles = Vector3.zero;
            transport.transform.localScale = Vector3.one;
            Data.Instance.events.AdvisesOn("JETPACK!");
            OnAvatarProgressBarStart(Color.green);
        }
        else if (item == Powerup.types.INVENSIBLE)
        {
            if (fxState == fxStates.SUPER) return;
            setSuperState();
            Data.Instance.events.AdvisesOn("INVENSIBLE!");
            OnAvatarProgressBarStart(Color.blue);
            progressBar.SetTimer(0.2f);
        }
    }
   public void UpdateByController()
    {
    }
   private void OnListenerDispatcher(string message)
    {
        if (message == "ShowMissionName")
            OnMissionStart(Data.Instance.missionActive);
   }
   public void OnMissionStart(int missionID)
   {

       if (Data.Instance.DEBUG || Data.Instance.playMode == Data.PlayModes.COMPETITION)
       {
           canJump = true;
           canShoot = true;
       }
       else
       {

           if (missionID > 1)
               canJump = true;
           if (missionID > 4)
               canShoot = true;

           if (missionID < 5)
               weapon.setOff();
           else
               weapon.setOn();
       }
   }
   private void setStartingState()
   {
       fxState = fxStates.SUPER;
       gameObject.layer = LayerMask.NameToLayer("SuperFX");
   }
   private void setStartingState2()
   {
       fxState = fxStates.NORMAL;
       gameObject.layer = LayerMask.NameToLayer("Character");
   }
    private void setNormalState()
    {
        Data.Instance.events.OnChangeMood(1);
        Data.Instance.events.OnAvatarChangeFX(Player.fxStates.NORMAL);
        fxState = fxStates.NORMAL;
        gameObject.layer = LayerMask.NameToLayer("Character");
        particles.SetActive(false);        
    }
    private void setSuperState()
    {
        Data.Instance.events.OnChangeMood(2);
        Data.Instance.events.OnAvatarChangeFX(Player.fxStates.SUPER);
        fxState = fxStates.SUPER;
        gameObject.layer = LayerMask.NameToLayer("SuperFX");
        particles.SetActive(true);
    }

    //public void resetEnergy()
    //{
    //    energy = 100;
    //}
    //public void removeEnergy(float qty)
    //{
    //    energy -= qty * 2;        
    //}
    //public void addEnergy(int qty)
    //{
    //    if (fxState == fxStates.SUPER)
    //        energy += qty/2;
    //    else
    //        energy += qty;

    // //  energyBar.Animate();
    //}
    
}
