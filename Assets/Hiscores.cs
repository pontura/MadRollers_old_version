using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Hiscores : MonoBehaviour {

    public List<LevelHiscore> levels;

    [Serializable]
    public class LevelHiscore
    {
        [SerializeField]
        public List<Hiscore> hiscore;
        public int id;
    }

    [Serializable]
    public class Hiscore
    {
        [SerializeField]
        public string username;
        public string facebookID;
        public int score;
    }

    void Start()
    {
        SocialEvents.OnHiscoresLoaded += OnHiscoresLoaded;
    }
    void OnHiscoresLoaded(string receivedData)
    {
        Debug.Log("Sores Init: " + receivedData);

        string[] allData = Regex.Split(receivedData, "</n>");

        for (var i = 0; i < allData.Length - 1; i++)
        {
            string[] userData = Regex.Split(allData[i], ":");
            Hiscore hiscore = new Hiscore();
            hiscore.facebookID = userData[0];
            hiscore.username = userData[1];
            hiscore.score = int.Parse(userData[2]);
          //  hiscores.Add(hiscore);
        }

      //  SocialEvents.OnHiscoresLoaded();
    }
}
