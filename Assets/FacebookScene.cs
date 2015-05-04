using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class FacebookScene : MonoBehaviour {
    
    public ProfilePicture profilePicture;
    public GameObject MyAvatar;
    public GameObject FB_Username;

    public GameObject ButtonCompite;
    public GameObject ButtonPlay;

    public Text hiscore;
    private List<object> scoresList = null;

    [SerializeField] FriendButton friendButton;

    private Dictionary<string, string> profile = null;

    public void Init(bool IsLoggedIn)
    {
        if (IsLoggedIn)
        {
            SetActive(true);
          
        }
        else
        {
            Data.Instance.events.OnFacebookUserLoaded += OnFacebookUserLoaded;
            SetActive(false);
        }
    }
    void OnDestroy()
    {
        Data.Instance.events.OnFacebookUserLoaded -= OnFacebookUserLoaded;
    }
    void SetActive(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            FB_Username.GetComponent<Text>().text = Data.Instance.userData.username;
            hiscore.text = Data.Instance.userData.hiscore.ToString();
            profilePicture.setPictre(Data.Instance.userData.facebookId);

            ButtonCompite.SetActive(false);
            //Vector3 pos = ButtonPlay.transform.localPosition;
            //pos.y = -165;
            //ButtonPlay.transform.localPosition = pos;
        }
        else
        {
            MyAvatar.SetActive(false);
        }
    }
    void OnFacebookUserLoaded(string id)
    {
        SetActive(true);
    }
   

    //public void QueryScores()
    //{
    //    FB.API("app/scores/?fields=score,user.limit(30)", Facebook.HttpMethod.GET, ScoresCallback);
    //}
    //public void SetScores(int score)
    //{
    //    var scoreData = new Dictionary<string, string>();
    //    scoreData["score"] = score.ToString();
    //    FB.API("/me/scores", Facebook.HttpMethod.POST, delegate(FBResult result)
    //    {
    //        Debug.Log("score submit result" + result.Text);
    //    }, scoreData
    //    );
    //}
    //void ScoresCallback(FBResult result)
    //{
    //    Debug.Log("ScoresCallback : " + result.Text);
    //    DebugText.text = "";

    //    scoresList = Util.DeserializeScores(result.Text);

    //    int _x = -141;
    //    foreach (object score in scoresList)
    //    {
    //        var entry = (Dictionary<string, object>)score;
    //        var user = (Dictionary<string, object>)entry["user"];
    //       DebugText.text += " user:" + user["name"] + ":" + entry["score"] + " id: " + user["id"];
            

    //        FriendButton button = Instantiate(friendButton) as FriendButton;
    //        button.transform.SetParent(container.transform);
    //        button.transform.localPosition = new Vector3(_x, 0, 0);            
    //        _x += 80;
    //        string _username = user["name"].ToString();
    //        string _score = entry["score"].ToString();
    //        print(user["id"] + " ------ " + fbHolder.id);
    //        if (user["id"].ToString() == fbHolder.id)
    //        {
    //            fbHolder.score = int.Parse(_score);
    //            hiscore.text = _score;
    //        }
    //        button.Init(_username, _score);

    //    }
    //}
}
