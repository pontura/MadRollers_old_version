using UnityEngine;
using System.Collections;

public class MmoCharacter : SceneObject
{
    public AudioClip FXDeath;
    public AudioClip FXActive;

    public enum states
    {
        IDLE,
        DEAD,
        WALK,
        RUN,
        JUMP,
        WAIT_TO_JUMP,
        FLY
    }

    public states state;

    private Animation _animation;
    private ObjectPool ObjectPool;

	// Use this for initialization
   // public override void OnRestart(Vector3 pos)
    void Start()
    {
        ObjectPool = Data.Instance.sceneObjectsPool;
    }
    void OnEnable()
    {
        _animation = GetComponentInChildren<Animation>();
        
    }
    public override void OnRestart(Vector3 pos)
    {
        gameObject.GetComponent<Collider>().enabled = true;
        base.OnRestart(pos);
        state = states.IDLE;
    }
	public void Die() {
		if(state== states.DEAD) return;

        GetComponent<AudioSource>().clip = FXDeath;
        GetComponent<AudioSource>().Play();

        setScore();

		Missions missions = Data.Instance.GetComponent<Missions>();
		missions.SendMessage ("killGuy", 1);

        _animation.Play("enemyDie");
		state = states.DEAD;

		SendMessage("isDead", SendMessageOptions.DontRequireReceiver);
		
		Vector3 pos = transform.position;
		pos.y+= 2.1f;
		SendMessage("breakOut",pos);

        StartCoroutine(reset());
        gameObject.GetComponent<Collider>().enabled = false;
	}
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "destroyable" || other.gameObject.tag == "enemy")
            Die();
    }
	IEnumerator reset() {
        yield return new WaitForSeconds(0.5f);
        Data.Instance.events.OncharacterCheer();
        Pool();
	}
	public void run() {
        _animation.Play("enemyRun");
        state = states.RUN;
	}
    public void fly()
    {
        _animation.Play("enemyFly");
        state = states.FLY;
    }
    public void walk()
    {
        _animation.Play("enemyWalk");
        state = states.WALK;
    }
	public void idle() {
        _animation.Play("enemyIdle");
        state = states.IDLE;
	}
    public void waitToJump()
    {
        _animation.Play("enemyWaitJump");
        state = states.WAIT_TO_JUMP;
    }
	public void jump() {
        GetComponent<AudioSource>().clip = FXActive;
        GetComponent<AudioSource>().Play();
        _animation.Play("enemyJump");
        state = states.JUMP;
	}
    public void reachFloor()
    {
        //run();
        //SendMessage("OnReachFloor", SendMessageOptions.DontRequireReceiver);
    }
	// Update is called once per frame
    public override void OnSceneObjectUpdate()
    {
        base.OnSceneObjectUpdate();
        SendMessage("OnSceneObjectUpdated", SendMessageOptions.DontRequireReceiver);
	}
}
