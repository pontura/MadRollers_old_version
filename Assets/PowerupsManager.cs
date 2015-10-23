using UnityEngine;
using System.Collections;

public class PowerupsManager : MonoBehaviour {

    public SceneObject Invencible;

    void Start()
    {
	    Data.Instance.events.OnAddExplotion += OnAddExplotion;
    }    
    public void OnDestroy()
    {
        Data.Instance.events.OnAddExplotion -= OnAddExplotion;
    }
    void OnAddExplotion(Vector3 pos)
    {
        SceneObject newSO = ObjectPool.instance.GetObjectForType(Invencible.name, true);
        if (newSO)
        {
            int force = 800;
            pos.y += 1.2f;
            newSO.Restart(pos);
            newSO.transform.localEulerAngles = Vector3.zero;
            Vector3 direction = ((newSO.transform.forward * force) + (Vector3.up * (force * 1.8f)));
            newSO.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Acceleration);
        }
    }
}
