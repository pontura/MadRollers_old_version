using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewHiscore : MonoBehaviour {

    public Text NewHiscoreText;

	void Start () {
        NewHiscoreText.text = Data.Instance.userData.hiscore.ToString();
	}
    public void Share()
    {
        Fade.LoadLevel("Game", 1, 1, Color.black);
    }
    public void Ok()
    {
        Fade.LoadLevel("Game", 1, 1, Color.black);
    }
    public void MainMenu()
    {
        Fade.LoadLevel("MainMenu", 1, 1, Color.black);
    }
}
