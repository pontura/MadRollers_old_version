using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Game game;
	private Gui gui;
	private CharacterBehavior character;

    public GameObject particles;

    public int id; //numero de player;

    [HideInInspector]
    public Weapon weapon;

    public Weapon[] weapons;
    public GameObject weaponContainer;

    public fxStates fxState;

    public bool canJump = true;
    public bool canShoot = true;

    private Vector3 _scale;
    

    public enum fxStates
    {
        NORMAL,
        SUPER
    }

    public float energy = 90;
    private Material originalMaterial;

    public EnergyBar energyBar;

    void OnDestroy()
    {
        Data.Instance.events.OnMissionStart -= OnMissionStart;
        Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;
        Data.Instance.events.OnAvatarGetItem -= OnAvatarGetItem;        
    }
    public void Init(int id, EnergyBar energyBar)
    {
        _scale = transform.localScale;
        if (id == 0)
            originalMaterial = Resources.Load("Materials/BolaHead", typeof(Material)) as Material;
        else
            originalMaterial = Resources.Load("Materials/Piel2", typeof(Material)) as Material;

        setStartingState();
        Invoke("setStartingState2", 1);

        Data.Instance.events.OnMissionStart += OnMissionStart;
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
        Data.Instance.events.OnAvatarGetItem += OnAvatarGetItem;      

        this.id = id;
        this.energyBar = energyBar;
        weapon = Instantiate(weapons[0] as Weapon, Vector3.zero, Quaternion.identity) as Weapon;
        weapon.transform.parent = weaponContainer.transform;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localScale = Vector3.one*2;
        particles.SetActive(false);
    }
    private void OnAvatarGetItem(string item)
    {
        canShoot = true;
        weapon.setOn();
    }
   public void UpdateByController()
    {

        if (fxState == fxStates.SUPER)
        {
            removeEnergy(Time.deltaTime * 6);
            if (energy < 0)
                setNormalState();
        } else
        {
            if (energy > 100)
            {
                energy = 100;
                setSuperState();
            }
        }

        float newEnergy = energy / 100.0f;


        if (newEnergy != energyBar.fillValue)
            energyBar.setEnergy(newEnergy);

    }
   private void OnListenerDispatcher(string message)
    {
        if (message == "ShowMissionName")
            OnMissionStart(Data.Instance.missionActive);
   }
   public void OnMissionStart(int missionID)
   {

       if (Data.Instance.DEBUG)
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
      // Data.Instance.events.OnAvatarChangeFX(Player.fxStates.SUPER);
       fxState = fxStates.SUPER;
       Material oldMat = Resources.Load("Materials/Piel", typeof(Material)) as Material;
       GetComponent<MaterialsChanger>().changeMaterial(oldMat, originalMaterial);
       gameObject.layer = LayerMask.NameToLayer("SuperFX");
   }
   private void setStartingState2()
   {
       // Data.Instance.events.OnAvatarChangeFX(Player.fxStates.SUPER);
       fxState = fxStates.NORMAL;
       gameObject.layer = LayerMask.NameToLayer("Character");
       GetComponent<Animation>().Stop();
   }
    private void setNormalState()
    {
        transform.localScale = _scale;
        Data.Instance.events.OnAvatarChangeFX(Player.fxStates.NORMAL);
        fxState = fxStates.NORMAL;
       // Material oldMat = Resources.Load("Materials/Ropa", typeof(Material)) as Material;     
       // GetComponent<MaterialsChanger>().changeMaterial(oldMat, originalMaterial);
        gameObject.layer = LayerMask.NameToLayer("Character");
        GetComponent<Animation>().Stop();
        particles.SetActive(false);
    }
    private void setSuperState()
    {
        Data.Instance.events.OnAvatarChangeFX(Player.fxStates.SUPER);
        fxState = fxStates.SUPER;
        //Material newMat = Resources.Load("Materials/Ropa", typeof(Material)) as Material;
        //GetComponent<MaterialsChanger>().changeMaterial(originalMaterial, newMat);
        gameObject.layer = LayerMask.NameToLayer("SuperFX");
        GetComponent<Animation>().Play("AvatarSpecialFX");
        particles.SetActive(true);
    }

    public void resetEnergy()
    {
        energy = 100;
    }
    public void removeEnergy(float qty)
    {
        energy -= qty * 2;        
    }
    public void addEnergy(int qty)
    {
        if (fxState == fxStates.SUPER)
            energy += qty/2;
        else
            energy += qty;

       energyBar.Animate();
    }
    
}
