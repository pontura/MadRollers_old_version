using UnityEngine;
using System.Collections;

public class Breakable : MonoBehaviour {

    public ExplotionType explotionType;
    public enum ExplotionType
    {
        SIMPLE,
        BOMB,        
        ENEMY
    }
	public GameObject particle;
    public bool isOn;
	public float NumOfParticles = 30;
	private Vector3 position;
	public Breakable[] childs;
	public bool dontKillPlayers;
	public bool dontDieOnHit;
    private Vector3 originalPosition;
    public System.Action OnBreak = delegate { };
    public int score;

    public void Start()
    {               
        isOn = true;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "explotion")
        {
            breakOut(transform.position);
        }
    }
	public void breakOut (Vector3 position) {
        if (!isOn) return;

        setScore();

        OnBreak();

        Data.Instance.events.OnAddObjectExplotion(transform.position, (int)explotionType);
        
		foreach (Breakable breakable  in childs)
		{
			if(breakable) breakable.hasGravity();
		}
		this.position = position;

        StartCoroutine(breakerTimer());

        isOn = false;

        SendMessage("Die", SendMessageOptions.DontRequireReceiver);
        
	}
    private void setScore()
    {
        if (score > 0)
            Data.Instance.events.OnScoreOn(transform.position, score);
    }
	public void hasGravity() {
		dontKillPlayers = true;
		//if(!gameObject) return;
		if(!gameObject.GetComponent<Rigidbody>())
		{
			gameObject.AddComponent<Rigidbody>();
		}
		gameObject.GetComponent<Rigidbody>().isKinematic = false;
		gameObject.GetComponent<Rigidbody>().useGravity = true;

        Vector3 newPosition = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.5f, 1.1f), Random.Range(0, 0.9f));
        gameObject.GetComponent<Rigidbody>().AddForce((Time.deltaTime * newPosition) * 2000, ForceMode.Impulse);
		
		StartCoroutine(makeItTrigger());
		
		if(childs.Length>0)
		{
			foreach (Breakable breakable  in childs)
			{
				if(breakable)
					breakable.hasGravity();
			}
		}
	}
	IEnumerator makeItTrigger() {
		yield return new WaitForSeconds(0.5f);
		GetComponent<Collider>().isTrigger = true;
	}
	IEnumerator breakerTimer() {
		breaker();
		yield return new WaitForSeconds(0.03f);
		breaker();
		yield return new WaitForSeconds(0.03f);
		breaker();
		if(dontDieOnHit)
		{
			dontKillPlayers = true;
		} else {
            Destroy(gameObject);
		}
	}
	
	private void breaker(){
		for (int i = 0; i < NumOfParticles/3; i++) {
			position.y+=0.05f;
			position.x+=Random.Range(-0.5f,0.5f);
			position.z+=0.1f;
			//Instantiate(particle, position, Quaternion.identity);
            if (!particle) return;
            SceneObject newSO = ObjectPool.instance.GetObjectForType(particle.name, true);
            if (newSO)
            {
                newSO.Restart(position);
                newSO.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1000, 0), ForceMode.Impulse);
            }
            else
            {
                //Debug.Log("__________no existe la particula de breaker: " + particle.name);
            }
		}
	}
}
