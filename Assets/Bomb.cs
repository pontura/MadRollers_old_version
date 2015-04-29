using UnityEngine;
using System.Collections;

public class Bomb : SceneObject {

	public float start_Y = 5;
	public float speed = 0.2f;
    public int activationArea = 100;

    public Breakable breakable;

    public AudioClip soundFX;
    private bool alive;

    public override void OnRestart(Vector3 pos)
    {
        pos.y = start_Y;
        base.OnRestart(pos);
        breakable.OnBreak += OnBreak;
        alive = true;
        
    }
    private void OnBreak()
    {
        GetComponent<AudioSource>().Stop();
        setScore();
        Missions missions = Data.Instance.GetComponent<Missions>();
        missions.SendMessage("killBomb", 1);
        alive = false;        

        if(isActive)
            StartCoroutine(waitToPool());

        isActive = false;

    }
    IEnumerator waitToPool()
    {
        GetComponent<TrailRenderer>().enabled = false;
        yield return new WaitForSeconds(3);
        if (isActive)
            Pool();
    }
    public override void onDie()
    {
        if (!alive) return;
        if (!isActive) return;
        
        addExplotion(0.2f);
        
        Pool();
    }
    public override void OnSceneObjectUpdate()
    {
        if (!isActive) return;

        if (transform.localPosition.y < 5.5f && !GetComponent<AudioSource>().isPlaying)
            startAudio();

        if (charactersMmanager.getDistance() + activationArea < transform.position.z) return;
       
        //if(alive)
        //{
            Vector3 pos = base.gameObject.transform.position;
			pos.y -= speed * Time.deltaTime;
            base.gameObject.transform.position = pos;
        //} 
	}

	void addExplotion(float _y)
	{
        if (!alive) return;
        if (!isActive) return;
        Data.Instance.events.AddExplotion(transform.position);
        //Data.Instance.events.AddWallExplotion(transform.position);
	}
    public override void OnPool()
    {
        GetComponent<AudioSource>().Stop();
    }
    private void startAudio()
    {
        GetComponent<AudioSource>().clip = soundFX;
        GetComponent<AudioSource>().Play();
    }
}
