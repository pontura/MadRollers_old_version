using UnityEngine;
using System.Collections;

public class CompetitionSelection : MonoBehaviour {

    public void GotoGame()
    {
        print("Play");
        Data.Instance.LoadLevel("Game");
    }
}
