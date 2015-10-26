using UnityEngine;
using System.Collections;

public class PowerupsManager : MonoBehaviour {

    public SceneObject Invencible;

    void Start()
    {
        Data.Instance.events.OnAddPowerUp += OnAddPowerUp;
    }    
    public void OnDestroy()
    {
        Data.Instance.events.OnAddPowerUp -= OnAddPowerUp;
    }
    void OnAddPowerUp(Vector3 pos)
    {
        SceneObject newSO = ObjectPool.instance.GetObjectForType(Invencible.name, true);
        if (newSO)
        {
            int force = 600;
            pos.y += 1.2f;
            newSO.Restart(pos);
            newSO.transform.localEulerAngles = Vector3.zero;
            Vector3 direction = ((newSO.transform.forward * force) + (Vector3.up * (force * 1.8f)));
            newSO.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Acceleration);
        }
    }
}
