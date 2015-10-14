﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HearsManager : MonoBehaviour {

    public Text label;
    public int total;
    public int newHearts = 0;
    public Animation anim;

	void Start () {
        newHearts = 0;
        total = PlayerPrefs.GetInt("totalHearts");
        Data.Instance.events.OnGrabHeart += OnGrabHeart;
        Data.Instance.events.OnAvatarDie += OnAvatarDie;
        Data.Instance.events.OnUseHearts += OnUseHearts;
	}
    void OnDestroy () {
        Data.Instance.events.OnGrabHeart -= OnGrabHeart;
        Data.Instance.events.OnAvatarDie -= OnAvatarDie;
        Data.Instance.events.OnUseHearts -= OnUseHearts;
	}
    void OnUseHearts(int qty)
    {
        total -= qty;
        SetHearts();
    }
    void OnAvatarDie(CharacterBehavior cb)
    {
        PlayerPrefs.SetInt("totalHearts", total);
    }
    void OnGrabHeart()
    {
        anim["UIHeartAnim"].normalizedTime = 0;
        anim.Play("UIHeartAnim");
        newHearts++;
        total++;
        SetHearts();
    }
    void SetHearts()
    {
        label.text = total.ToString();
    }
}