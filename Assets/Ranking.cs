﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Ranking : MonoBehaviour {

    public GameObject container;
    public RankingLine rankingLine;
    private bool rankingLoaded;

	void Start () 
    {
        int levelID = Data.Instance.competitions.current;
	}
    void Update()
    {
        if (rankingLoaded) return;
        if (Social.Instance.hiscores.levels[0].hiscore.Count == 0) return;
        rankingLoaded = true;
        LoadRanking();
    }
    void LoadRanking()
    {
        foreach (Hiscores.Hiscore hiscore in Social.Instance.hiscores.levels[0].hiscore)
        {
            RankingLine newObj = Instantiate(rankingLine) as RankingLine;
            newObj.Init(hiscore.username, hiscore.score, hiscore.facebookID);
            newObj.transform.SetParent(container.transform);
            newObj.transform.localScale = Vector3.one;
        }
    }
}
