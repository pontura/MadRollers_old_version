using UnityEngine;
using System.Collections;

public class GrabbableItem : SceneObject
{

	public int energy = 1;
    public bool hitted;
    public float sec = 0;
    public Collider _collider;
    public Player player;
    public AudioClip heartClip;

    public override void OnRestart(Vector3 pos)
    {
        if (gameObject.GetComponent<TrailRenderer>())
            gameObject.GetComponent<TrailRenderer>().enabled = true;

        _collider = gameObject.GetComponent<Collider>();
        _collider.isTrigger = false;
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
			if(sec==0)
			{
                _collider.isTrigger = true;
			}
			sec++;
			Vector3 position = transform.position;
            Vector3 characterPosition = player.transform.position;
			characterPosition.y+=1.7f;
			characterPosition.z+=1.7f;
			transform.position = Vector3.MoveTowards(position, characterPosition, 16 * Time.deltaTime);
			if(sec>12)
			{
                player.addEnergy(energy);
                Data.Instance.events.OnScoreOn(Vector3.zero, 10);
                //Missions missions = Data.Instance.GetComponent<Missions>();
				//missions.SendMessage ("getHeart", energy);
                Data.Instance.events.OnGrabHeart();
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
            Data.Instance.GetComponent<MusicManager>().addHeartSound();

            if (other.transform.GetComponent<Player>())
                player = other.transform.GetComponent<Player>();
            else
                player = other.transform.parent.GetComponent<Player>();

            if (gameObject.GetComponent<TrailRenderer>())
                gameObject.GetComponent<TrailRenderer>().enabled = false;
			hitted = true;
		}
	}
    
}
