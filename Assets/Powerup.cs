﻿using UnityEngine;
using System.Collections;

public class Powerup : GrabbableItem {

    public string grabbableName = "missile";

    public types type;
    public enum types
    {
        MISSILE,
        JETPACK,
        INVENSIBLE
    }

    public override void OnSceneObjectUpdate()
    {
        if (hitted)
        {
            if (sec == 0)
            {
               // _collider.isTrigger = true;
            }
            sec++;
            Vector3 position = transform.position;
            Vector3 characterPosition = player.transform.position;
            characterPosition.y += 1.5f;
            characterPosition.z += 1.5f;
            transform.position = Vector3.MoveTowards(position, characterPosition, 15 * Time.deltaTime);
            if (sec > 13)
            {
                Data.Instance.events.OnAvatarGetItem( type );
                Pool();               
            }
        }
    }
}
