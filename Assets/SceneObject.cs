using UnityEngine;
using System.Collections;

public class SceneObject : MonoBehaviour {

    public Shadow shadow;

    //sirve para quitar el objeto de pantalla màs tarde...
    public int size_z = 0;
    public int id;

    [HideInInspector]
    public Transform characterTransform;

    [HideInInspector]
    public bool isActive;
    public int score;

    [HideInInspector]
    public CharactersManager charactersMmanager;

    private Transform[] childs;

    void Start()
    {
        
    }
    public void LateUpdate()
    {
        if (!isActive) return;
        if (!charactersMmanager) return;
        if (charactersMmanager.getDistance() == 0) return;

        float distance = charactersMmanager.getDistance();

        if (transform.localPosition.y < -10)
            Pool();
        else if (distance > transform.position.z + size_z + 8)
            Pool();
        else
            OnSceneObjectUpdate();
    }
    public void Restart(Vector3 pos)
    {
       // characterTransform = GameObject.FindGameObjectWithTag("Player").transform;

        //if (shadow)
        //{
        //    SceneObject shadowNew = ObjectPool.instance.GetObjectForType(shadow.name, false);
        //    shadowNew.Restart(pos);
        //    shadowNew.GetComponent<Shadow>().Init(this);
        //}
        OnRestart(pos);
    }
    public void setRotation(Vector3 rot)
    {
        if (transform.localEulerAngles == rot) return;
        transform.localEulerAngles = rot;
    }
    public void lookAtCharacter()
    {
       // transform.LookAt(characterTransform);
    }
    public void Pool()
    {
        isActive = false;
        Vector3 newPos = new Vector3(2000, 0, 2000);
        transform.position = newPos;       
        ObjectPool.instance.PoolObject(this);
        OnPool();
    }
    public virtual void OnSceneObjectUpdate()
    {
        SendMessage("OnSceneObjectUpdated", SendMessageOptions.DontRequireReceiver);
    }
    public virtual void OnRestart(Vector3 pos)
    {
        if(!charactersMmanager)
         charactersMmanager = Game.Instance.GetComponent<CharactersManager>();

        gameObject.SetActive(true);
        transform.position = pos;
        isActive = true;
        SendMessage("OnSceneObjectRestarted", SendMessageOptions.DontRequireReceiver);
    }
    public virtual void changeMaterial(string materialName)
    {

    }
    public virtual void OnPool()
    {
    }
    public virtual void onDie()
    {
    }
    public virtual void setScore()
    {
        if(score>0)
            Data.Instance.events.OnScoreOn(transform.localPosition, score);
    }    
}
