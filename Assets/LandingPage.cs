using UnityEngine;
using System.Collections;

public class LandingPage : MonoBehaviour {

	void Update () {
        if (Input.anyKeyDown)
        {
            Fade.LoadLevel("MainMenu", 1, 1, Color.black);
        }
	}
}
