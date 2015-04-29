using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Ranking : MonoBehaviour {

	// Use this for initialization
	void Start () {

        ProfilePicture[] profilePictures = GetComponentsInChildren<ProfilePicture>();
        foreach (ProfilePicture profilePicture in profilePictures)
            profilePicture.gameObject.GetComponent<Image>().enabled = false;

        if (Data.Instance.GetComponent<Hiscores>().hiscores.Count > 0)
            OnHiscoresLoaded();
        else
            Data.Instance.events.OnHiscoresLoaded += OnHiscoresLoaded;
	}
    void OnDestroy()
    {
        Data.Instance.events.OnHiscoresLoaded -= OnHiscoresLoaded;
    }
    void OnHiscoresLoaded()
    {
        List<Hiscores.Hiscore> hiscores = Data.Instance.GetComponent<Hiscores>().hiscores;
        ProfilePicture[] profilePictures = GetComponentsInChildren<ProfilePicture>();
        int a = 0;
        foreach (ProfilePicture profilePicture in profilePictures)
        {
            profilePicture.gameObject.GetComponent<Image>().enabled = true;
            if(a<hiscores.Count)
                profilePicture.Init(hiscores[a]);
            a++;
        }
    }
}
