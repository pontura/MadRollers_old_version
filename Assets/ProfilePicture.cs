﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProfilePicture : MonoBehaviour {

   public void Init(Hiscores.Hiscore data)
    {
        setPictre(data.facebookID);
    }
   public void setPictre(string facebookID)
   {
       StartCoroutine(GetPicture(facebookID));
   }
   IEnumerator GetPicture(string facebookID)
    {
        if (facebookID == "") 
            yield break;

        print("Busca " + facebookID);

        WWW receivedData = new WWW("https" + "://graph.facebook.com/" + facebookID + "/picture?width=128&height=128");
        yield return receivedData;
        if (receivedData.error == null)
        {
            GetComponent<Image>().sprite = Sprite.Create(receivedData.texture, new Rect(0, 0, 128, 128), Vector2.zero);
        }
        else
        {
            Debug.Log("ERROR trayendo imagen");
        }

    }
}
