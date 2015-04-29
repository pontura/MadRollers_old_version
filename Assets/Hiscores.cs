using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Hiscores : MonoBehaviour {

    public List<Hiscore> hiscores;

    [Serializable]
    public class Hiscore
    {
        [SerializeField]
        public string username;
        public string facebookID;
        public int score;
    }

    public void Init(string receivedData)
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
            hiscores.Add(hiscore);
        }

        Data.Instance.events.OnHiscoresLoaded();
    }
}
