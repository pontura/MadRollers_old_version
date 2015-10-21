using UnityEngine;
using System.Collections;

public class GrabbableItem : SceneObject
{

	public int energy = 1;
    //[HideInInspector]
    public bool hitted;
    [HideInInspector]
    public float sec = 0;

     [HideInInspector]
    public Collider TriggerCollider;
     [HideInInspector]
    public Collider FloorCollider;

    [HideInInspector]
    public Player player;
   // public AudioClip heartClip;

    public override void OnRestart(Vector3 pos)
    {
        if (gameObject.GetComponent<TrailRenderer>())
            gameObject.GetComponent<TrailRenderer>().enabled = true;

        TriggerCollider = gameObject.GetComponent<SphereCollider>();
        FloorCollider = gameObject.GetComponent<BoxCollider>();

        TriggerCollider.enabled = true;
        FloorCollider.enabled = true;

        base.OnRestart(pos);
        hitted = false;
        sec = 0;
    }
    public override void OnPool()
    {

    }
    public override void OnSceneObjectUpdate()
    {
		if(hitted)
		{
			sec++;
			Vector3 position = transform.position;
            Vector3 characterPosition = player.transform.position;
			characterPosition.y+=1.7f;
			characterPosition.z+=1.7f;
			transform.position = Vector3.MoveTowards(position, characterPosition, 18 * Time.deltaTime);
			if(sec>12)
			{
                Data.Instance.events.OnScoreOn(Vector3.zero, 10);
                Data.Instance.events.OnGrabHeart();
                Data.Instance.GetComponent<MusicManager>().addHeartSound();
                player = null;
                Pool();
			}
		}
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;
		if(other.gameObject.CompareTag("Player"))
		{
            if (other.transform.GetComponent<Player>())
                player = other.transform.GetComponent<Player>();
            else
                player = other.transform.parent.GetComponent<Player>();

            if (gameObject.GetComponent<TrailRenderer>())
                gameObject.GetComponent<TrailRenderer>().enabled = false;
			hitted = true;
            TriggerCollider.enabled = false;
            FloorCollider.enabled = false;
		}
	}
    
}
