
using UnityEngine;
using System.Collections;

public class CharacterBehavior : MonoBehaviour {

	private Animation _animation;    
    public float speed;
	private bool walking1;
	private bool walking2;

    public Collider[] colliders;
    public CharacterFloorCollitions floorCollitions;

    public AudioClip jumpClip;
    public AudioClip jump2Clip;
    public AudioClip jump3Clip;
    public AudioClip FXFall;
    public AudioClip FXCrash;
    public AudioClip FXCheer;

    public states state;
    public enum states
    {
        RUN,
        JUMP,
        DOUBLEJUMP,
        HITTED,
        SHOOT,
        DEAD,
        FALL,
        CRASH
    }

    private int heightToFall = -5;
	private float jumpHeight = 1300;
	public float superJumpHeight = 1200;
	private Vector3 movement;
	private float hittedTime;
	private float hittedSpeed;
	private Vector3 hittedPosition;
	
	public float distance; 
	public float lastDistance;
	public GameObject myProjectile;
	
	public Player player;

    public GameObject model;
	public Data data;
    private Missions missions;

    public TextureChanger textureChanger;
	
	// Use this for initialization
	void Start () {
        data = Data.Instance;
        data.events.OncharacterCheer += OncharacterCheer;
        missions = Data.Instance.GetComponent<Missions>();
		player = GetComponent<Player>();
        _animation = model.GetComponent<Animation>();
	}
    void OnDestroy ()
    {
         Data.Instance.events.OncharacterCheer -= OncharacterCheer;
    }
	private float lastShot;
    public void OncharacterCheer()
    {
        if (Random.Range(0, 8) < 2)
        {
            GetComponent<AudioSource>().clip = FXCheer;
            GetComponent<AudioSource>().Play();
        }
    }
	public void CheckFire()
	{
		if(!player.canShoot) return;

        _animation.Play("shoot");
        state = states.SHOOT;
            
        player.weapon.Shoot();
        data.events.OnAvatarShoot();

		if(lastShot+0.3f > Time.time) return;
		lastShot = Time.time;

		Vector3 pos = new Vector3(transform.position.x, transform.position.y+1.7f, transform.position.z+0.1f);

        SceneObject projectil = ObjectPool.instance.GetObjectForType(myProjectile.name, true);
        if (projectil)
        {
            projectil.Restart(pos);
            projectil.transform.localEulerAngles = transform.localEulerAngles;
        }
        else
        {
            print("no hay projectil");
        }
	}
	public void UpdateByController () {

		distance = transform.position.z;
        missions.updateDistance(distance);
		lastDistance= distance;

        if (transform.position.y < heightToFall)
		{
            Fall();
			return;
		}
	}


    public void setRotation(Vector3 rot)
	{
        if (transform.localEulerAngles == rot) return;
        transform.localEulerAngles = rot;
	}
	public void bump(float damage)
	{
		Die();
	}
	public void bumpWithStaticObject()
	{
        bumperCollision(new Vector3(transform.localRotation.x, transform.position.y, transform.position.z + 5), 1, 10, 10);
	}
	public void bumperCollision(Vector3 hittedPosition, float damage, float bumperSpeed, float bumperDelay)
	{

	}
	public void Run()
	{
		if(state == states.RUN) return;
		state = states.RUN;
		_animation.Play("run");
	}
	public void Jump()
	{
        if (!player.canJump) return;
        floorCollitions.OnAvatarJump();
        GetComponent<AudioSource>().clip = jumpClip;
        GetComponent<AudioSource>().Play();


		if(state == states.JUMP) return;

        data.events.AvatarJump();

        GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
		
		_animation.Play("jump");
		state = states.JUMP;
		return;
	}
	public void SuperJump(float _superJumpHeight)
	{
        if (!player.canJump) return;
        GetComponent<AudioSource>().clip = jump2Clip;
        GetComponent<AudioSource>().Play();
        data.events.AvatarJump();
        _animation.Play("doubleJump");

        GetComponent<Rigidbody>().AddForce(new Vector3(0, (_superJumpHeight ) - (GetComponent<Rigidbody>().velocity.y * (jumpHeight / 10)), 0), ForceMode.Impulse);
		state = states.DOUBLEJUMP;
		return;
	}
	public void SuperJumpByBumped(int force = 1600, float offsetY = 0.5f)
	{
        data.events.AvatarJump();
        Vector3 pos = transform.localPosition;
        pos.y += offsetY;
        transform.localPosition = pos;
        SuperJump(force);

        GetComponent<AudioSource>().clip = jump3Clip;
        GetComponent<AudioSource>().Play();

        _animation.Play("Rebota");
	}
    public void Fall()
    {
        GetComponent<AudioSource>().clip = FXFall;
        GetComponent<AudioSource>().Play();
        Data.Instance.events.OnAvatarFall(this);
        Die();
    }

    public void Hit()
    {
        if (state == states.CRASH) return;

        SaveDistance();

        textureChanger.Dead();

        GetComponent<AudioSource>().clip = FXCrash;
        GetComponent<AudioSource>().Play();

        Data.Instance.events.OnAvatarCrash(this);

        state = states.CRASH;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().AddForce(new Vector3(0, 1500, 0), ForceMode.Impulse);
        GetComponent<Rigidbody>().freezeRotation = false;
        //removeColliders();
        _animation.Play("Crash");
        Invoke("CrashReal", 0.3f);
    }
    void CrashReal()
    {
        Time.timeScale = 0.02f;
        StartCoroutine(lowCamera());
    }
    IEnumerator lowCamera()
    {
        while (Time.timeScale < 1)
        {
            Time.timeScale += 0.001f;
            yield return null;
        }
    }
    void SaveDistance()
    {
        if(Data.Instance.playMode == Data.PlayModes.COMPETITION)
            SocialEvents.OnFinalDistance(distance);
    }
	public void Die()
	{
		if(state == states.DEAD) return;

        SaveDistance();

		state = states.DEAD;
        _animation.Play("FallDown");

		
		
        player.energyBar.hide();
	}
	
	
	public void burned(float damage)
	{
        player.removeEnergy(damage);
		SuperJump( jumpHeight );
	}
    public void removeColliders()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }
	
	
}

