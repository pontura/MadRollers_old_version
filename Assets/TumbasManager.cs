﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TumbasManager : MonoBehaviour {

    private CharacterBehavior characterBehavior;
    private bool isCompetition;
    public float distance;
    public int hiscoreID;
    private List<Hiscores.Hiscore> hiscore;
    private int offset = 2;

    void Start()
    {
        if (Data.Instance.playMode == Data.PlayModes.COMPETITION)
        {
            isCompetition = true;
            characterBehavior = GetComponent<CharactersManager>().character;
            hiscore = Social.Instance.hiscores.levels[0].hiscore;
        }
        hiscoreID = 0;

    }
    void Update()
    {
        if (!isCompetition) return;

        if (characterBehavior.distance + offset < hiscore[hiscoreID].score)
        {
            hiscoreID++;
            Data.Instance.events.OnAddTumba(new Vector3(0, 0, characterBehavior.distance + offset), hiscore[hiscoreID].username, hiscore[hiscoreID].facebookID);
        }
    }
}