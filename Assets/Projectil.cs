using UnityEngine;
using System.Collections;

public class Projectil : SceneObject {

	public int speed = 7;
	public int myRange = 3;
	public int damage = 10;

	private float myDist;
	private bool exploted;

	public AudioClip fire;
	public AudioClip coli;

    private Vector3 rotation;
    private Level level;
	
	void Start () {

	}
    public override void OnRestart(Vector3 pos)
    {
        level = Game.Instance.level;
        base.OnRestart(pos);
       // rotationX = -30;
        myDist = 0;
        exploted = false;
        pos.z += 1;
        transform.position = pos;
        AudioSource.PlayClipAtPoint(fire, pos);
        GetComponent<AudioSource>().Play();
    }
    
    public override void OnSceneObjectUpdate()
    {
		Vector3 pos = transform.position;
		myDist += Time.deltaTime * speed;
        rotation = transform.localEulerAngles;
        rotation.y = 0;
        if (pos.y < - 0.8) Destroy();
        else
		if(myDist >= myRange)
		{
            rotation.x += 30 * Time.deltaTime;
            transform.localEulerAngles = rotation;
		}
		pos += transform.forward * 50  * Time.deltaTime;
		transform.position = pos;
	}
	void OnTriggerEnter(Collider other) 
	{

        if (!isActive) return;
		if(exploted) return;

		switch (other.tag)
		{
            case "wall":
                addExplotionWall();
                Destroy();
                AudioSource.PlayClipAtPoint(coli, other.gameObject.transform.position);
                break;
			case "floor":
				addExplotion(0.2f);
				Destroy();
				AudioSource.PlayClipAtPoint(coli,other.gameObject.transform.position);
				break;
			case "enemy":
				MmoCharacter enemy= other.gameObject.GetComponent<MmoCharacter>();
				if(enemy.state ==  MmoCharacter.states.DEAD) return;

				enemy.Die ();
				Destroy();
				AudioSource.PlayClipAtPoint(coli,other.gameObject.transform.position);
				break;
			case "destroyable":
				other.gameObject.SendMessage("breakOut",other.gameObject.transform.position, SendMessageOptions.DontRequireReceiver);
                Destroy();
				AudioSource.PlayClipAtPoint(coli,other.gameObject.transform.position);
				break;
		}
	}
	void addExplotion(float _y)
	{
        if (!isActive) return;
		exploted = true;        
        Data.Instance.events.AddExplotion(transform.position);
	}
    void addExplotionWall()
    {
        if (!isActive) return;
        exploted = true;
        Data.Instance.events.AddWallExplotion(transform.position);
    }
    void Destroy()
    {
        Pool();
    }
}
