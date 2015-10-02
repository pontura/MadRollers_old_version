﻿using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public bool isOn;
    public GameObject asset;

    virtual public void setOff()
    {
        isOn = false;
        asset.SetActive(false);
    }
    virtual public void setOn()
    {
        if (isOn) return;
        isOn = true;
        asset.SetActive(true);
        Rebuild();
    }
    virtual public void Shoot()
    {
        if (!isOn) return;
        Rebuild();
    }
    virtual public void Rebuild()
    {
        DestroyImmediate(GetComponent<iTween>());
        transform.localScale = Vector3.one*0.001f;

        iTween.ScaleTo(gameObject,
            iTween.Hash(
            "scale", Vector3.one * 2,
            "easetype", iTween.EaseType.easeInCubic,
            "time", 0.5f
            )
        );       
    }
}
