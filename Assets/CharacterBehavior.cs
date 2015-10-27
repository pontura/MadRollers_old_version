
using UnityEngine;
using System.Collections;

public class CharacterBehavior : MonoBehaviour {

    public Animation _animation_hero;
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
        CRASH,
        JETPACK,
        JETPACK_OFF
    }

    private int MAX_JETPACK_HEIGHT = 20;
    private float speedRun = 18f;
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
	
	// Use this for initialization
	void Start () {
        data = Data.Instance;       
        missions = Data.Instance.GetComponent<Missions>();
		player = GetComponent<Player>();

        data.events.OnAvatarProgressBarEmpty += OnAvatarProgressBarEmpty;
        data.events.OncharacterCheer += OncharacterCheer;
	}
    void OnDestroy ()
    {
        data.events.OnAvatarProgressBarEmpty -= OnAvatarProgressBarEmpty;
        data.events.OncharacterCheer -= OncharacterCheer;
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

        if (_animation_hero)
            _animation_hero.Play("shoot");

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
        Invoke("ResetShoot", 0.3f);
	}
    void ResetShoot()
    {
        if (floorCollitions.state == CharacterFloorCollitions.states.ON_FLOOR)
            Run();
        else
            state = states.DOUBLEJUMP;
    }
	public void UpdateByController () {
        Vector3 goTo = Vector3.forward * speedRun * Time.deltaTime;

        
        if (state == states.JETPACK)
        {
            player.OnAvatarProgressBarUnFill(0.25f * Time.deltaTime);
            rigidbody.velocity = Vector3.zero;
            rigidbody.useGravity = false;
            
            Vector3 pos = transform.position;

            if (pos.y < MAX_JETPACK_HEIGHT)
            {
                pos.y += 6 * Time.deltaTime;
                transform.position = pos;
            }
        }
        else
        {
            rigidbody.mass = 100;
            rigidbody.useGravity = true;
        }
        

        transform.Translate(goTo);

		distance = transform.position.z;
        missions.updateDistance(distance);
		lastDistance= distance;

        if (transform.position.y < heightToFall)
		{
            Fall();
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
    public void Revive()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().freezeRotation = true;
        Run();
    }
	public void Run()
	{
		if(state == states.RUN) return;
		state = states.RUN;
        _animation_hero.Play("run");
	}
    public void JumpPressed()
    {
        if (player.transport != null)
            Jetpack();
    }
    public void AllButtonsReleased()
    {
        if (player.transport != null)
            JetpackOff();
    }
    void OnAvatarProgressBarEmpty()
    {
        if(state == states.JETPACK)
            JetpackOff();
    }
    public void Jetpack()
    {
        if (state == states.JETPACK) return;

        _animation_hero.Play("shoot");

        
        floorCollitions.OnAvatarFly();
        state = states.JETPACK;
        player.transport.SetOn();
    }
    public void JetpackOff()
    {
        print("JetpackOff");
        floorCollitions.OnAvatarFalling();

        if (player.transport)
            player.transport.SetOff();

        state = states.FALL;
    }
	public void Jump()
	{
        if (player.transport != null)   return;

        if (state == states.JUMP)
        {
            SuperJump(superJumpHeight);
            return;
        }
        else if(state != states.RUN && state != states.SHOOT)
        {
            return;
        }
        if (!player.canJump) return;

        floorCollitions.OnAvatarJump();
        GetComponent<AudioSource>().clip = jumpClip;
        GetComponent<AudioSource>().Play();


		if(state == states.JUMP) return;

        data.events.AvatarJump();

        GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);

        _animation_hero.Play("jump");
		state = states.JUMP;
		return;
	}
	public void SuperJump(float _superJumpHeight)
	{
        if (!player.canJump) return;
        GetComponent<AudioSource>().clip = jump2Clip;
        GetComponent<AudioSource>().Play();
        data.events.AvatarJump();

            _animation_hero.Play("doubleJump");

        GetComponent<Rigidbody>().AddForce(new Vector3(0, (_superJumpHeight ) - (GetComponent<Rigidbody>().velocity.y * (jumpHeight / 10)), 0), ForceMode.Impulse);
		state = states.DOUBLEJUMP;
		return;
	}

	public void SuperJumpByBumped(int force , float offsetY, bool dir_forward)
	{
        data.events.AvatarJump();
        Vector3 pos = transform.localPosition;
        pos.y += offsetY;
        transform.localPosition = pos;
        SuperJump(force);

        GetComponent<AudioSource>().clip = jump3Clip;
        GetComponent<AudioSource>().Play();

        if (!dir_forward)
            _animation_hero.Play("rebota");
        else
            _animation_hero.Play("superJump");

	}
    public void Fall()
    {
        GetComponent<AudioSource>().clip = FXFall;
        GetComponent<AudioSource>().Play();
        Data.Instance.events.OnAvatarFall(this);
        Die();
    }

    public void HitWithObject(Vector3 objPosition)
    {
        Hit();
        //if (state == states.CRASH) return;
        //print("CRASH");

        //GetComponent<AudioSource>().clip = FXCrash;
        //GetComponent<AudioSource>().Play();

        //state = states.CRASH;
        //Vector3 thrownPos = new Vector3((objPosition.x - transform.position.x)*2000, 1000, -1000);
        //GetComponent<Rigidbody>().AddForce(thrownPos, ForceMode.Impulse);
        //_animation.Play("Crash");
        //Invoke("CrashReset", 1f);
    }
    public void Hit()
    {
        SaveDistance();

        GetComponent<AudioSource>().clip = FXCrash;
        GetComponent<AudioSource>().Play();

        Data.Instance.events.OnAvatarCrash(this);

        state = states.CRASH;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().AddForce(new Vector3(0, 1500, 0), ForceMode.Impulse);
        GetComponent<Rigidbody>().freezeRotation = false;
        //removeColliders();

        _animation_hero.Play("hit");
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
            Time.timeScale += 0.001f + Time.deltaTime;
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
       // _animation.Play("FallDown");
	}
	
	
	public void burned(float damage)
	{
        //player.removeEnergy(damage);
		SuperJump( jumpHeight );
	}
	
	
}

