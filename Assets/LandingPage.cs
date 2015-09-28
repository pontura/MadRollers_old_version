using UnityEngine;
using System.Collections;

public class LandingPage : MonoBehaviour {

    public GameMenu gm;

	void Update () {
        if (Input.anyKeyDown)
        {
            gm.SetOn();
            Data.Instance.LoadLevel("MainMenu");
        }
	}
}
