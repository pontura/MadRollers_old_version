using UnityEngine;
using System.Collections;

public class GrabbableMissile : GrabbableItem {

    public override void OnSceneObjectUpdate()
    {
        if (hitted)
        {
            if (sec == 0)
            {
                _collider.isTrigger = true;
            }
            sec++;
            Vector3 position = transform.position;
            Vector3 characterPosition = player.transform.position;
            characterPosition.y += 1.5f;
            characterPosition.z += 1.5f;
            transform.position = Vector3.MoveTowards(position, characterPosition, 15 * Time.deltaTime);
            if (sec > 13)
            {
                Data.Instance.events.OnAvatarGetItem("missile");
                Pool();               
            }
        }
    }
}
