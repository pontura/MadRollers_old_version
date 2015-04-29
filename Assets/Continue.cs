using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class Continue : MonoBehaviour {

	delegate void DelayedMethod();
	private int num = 9;
	public Text countdown_txt;
	private float speed = 0.5f;
	
	IEnumerator WaitAndDo(float time, DelayedMethod method)
	{
		yield return new WaitForSeconds(time);
		method();
	}

	public void DoStuff()
	{
		num--;
		if(num==0)
		{
            Fade.LoadLevel("MainMenu", 1, 1, Color.black);
			return;
		}
		countdown_txt.text = num.ToString();
		StartCoroutine(WaitAndDo(speed, DoStuff));
	}	

	void Start () {
		StartCoroutine(WaitAndDo(speed, DoStuff));
	}

	void Update () {
        if (Input.anyKeyDown)
        {
            StopCoroutine("WaitAndDo");
            Fade.LoadLevel("Game", 1, 1, Color.black);
		}
	}

}
