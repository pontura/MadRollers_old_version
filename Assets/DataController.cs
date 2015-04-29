using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class DataController : MonoBehaviour
{
    private string secretKey = "mySecretKey";
    const string URL = "https://www.madrollers.com/";
    private string getUserIdByFacebookId_URL = URL + "getUserIdByFacebookId.php?";
    private string createUser_URL = URL + "createUser.php?";
    private string addFacebookId = URL + "addFacebookId.php?";
    private string addScore_URL = URL + "setHiscore.php?";
    private string getHighscores_URL = URL + "getHiscores.php";

    public void Init()
    {
        Data.Instance.events.OnHiscore += OnHiscore;
        GetHiscores();
      //  Data.Instance.events.OnFacebookUserLoaded += OnFacebookUserLoaded;
    }
    void OnHiscore(int score)
    {
        StartCoroutine(PostScores(GetComponent<UserData>().userId, score));
    }
    public void CheckIfFacebookIdExists(string facebookId)
    {
        StartCoroutine(CheckIfUserExistsOnLocalDB(facebookId));
    }
    //creo el user en la base local:
    IEnumerator CheckIfUserExistsOnLocalDB(string facebookId)
    {
        if (facebookId == "0")
            yield break;
        print("__________sigio: facebookId: " + facebookId);

        string post_url = getUserIdByFacebookId_URL + "facebookId=" + facebookId;
        print("OnFacebook : " + post_url);
        WWW receivedData = new WWW(post_url);
        yield return receivedData;
        if (receivedData.error != null)
            print("There was an error in CheckIfUserExistsOnLocalDB: " + receivedData.error);
        else
        {
            try
            {
                print("recibo CheckIfUserExistsOnLocalDB data: " + receivedData.text);

                string[] userData = Regex.Split(receivedData.text, ":");
                string userName = userData[1];
                int userId = System.Int32.Parse(userData[2]);
                int hiscore = System.Int32.Parse(userData[3]);

                SetUserData(userName, facebookId, userId, hiscore);
                
            }
            catch
            {
                Data.Instance.events.OnFacebookNewUserLogged(facebookId);
                Debug.Log("DATA CONTROLLER: CheckIfUserExistsOnLocalDB error");
              //  CreateUser(GetComponent<UserData>().username, facebookId, GetComponent<UserData>().hiscore);
            }
        }
    }
    public void CreateUserByFacebookID(string facebookId)
    {
        UserData userData = GetComponent<UserData>();
        StartCoroutine(CreateUserRoutine(userData.username, facebookId, userData.hiscore));
    }
    //creo el user en la base local:
    public void CreateUser(string username, string facebookId, int hiscore)
    {
        StartCoroutine(CreateUserRoutine( username,  facebookId,  hiscore));
    }
    IEnumerator CreateUserRoutine(string username, string facebookId, int hiscore)
    {
        username = username.Replace(" ", "_");
        string hash = Md5Test.Md5Sum(username + facebookId + secretKey);
        string post_url = createUser_URL + "facebookId=" + facebookId + "&username=" + username + "&hiscore=" + hiscore.ToString() + "&hash=" + hash;
        print("CreateUser : " + post_url);
        WWW hs_post = new WWW(post_url);
        yield return hs_post;
        if (hs_post.error != null)
            print("No pudo crear el nuevo user: " + hs_post.error);
        else
        {
            print("user agregado: " + hs_post.text);
            int userId = int.Parse(hs_post.text);
            SetUserData(username, facebookId, userId, hiscore);
        }
    }
    
    void SetUserData(string userName, string facebookID, int userId, int hiscore)
    {
        Data.Instance.events.OnSetUserData(userName, userId, hiscore, true);
    }
    IEnumerator PostScores(int userId, int hiscore)
    {
        Debug.Log("SEt hiscore id: " + userId + " score= " + hiscore);
        string hash = Md5Test.Md5Sum(userId.ToString() + hiscore.ToString() + secretKey);
        string post_url = addScore_URL + "id=" + userId.ToString() + "&hiscore=" + hiscore + "&hash=" + hash;
        print("PostScores : " + post_url);
        WWW hs_post = new WWW(post_url);
        yield return hs_post;
        if (hs_post.error != null)
            print("There was an error posting the high score: " + hs_post.error);
        else
            print(hs_post.text);
    }

    public void AddFacebookIdToExistingAccount(int userId, string facebookId)
    {
        StartCoroutine(AddFacebookId(userId, facebookId));
    }
    IEnumerator AddFacebookId(int userId, string facebookId)
    {
        string hash = Md5Test.Md5Sum(userId.ToString() + facebookId + secretKey);
        string post_url = addFacebookId + "facebookId=" + facebookId + "&id=" + userId + "&hash=" + hash;
        print("AddFacebookId : " + post_url);
        WWW hs_post = new WWW(post_url);
        yield return hs_post;
        if (hs_post.error != null)
            Debug.Log("addFacebookID ERROR" + hs_post.error);
        else
        {
            Data.Instance.events.OnFacebookIdAdded();
        }
    }


    public void GetHiscores()
    {
        StartCoroutine(GetHiscoresRoutine());
    }
    IEnumerator GetHiscoresRoutine()
    {
        string post_url = getHighscores_URL;
        print("GetHiscoresRoutine : " + post_url);
        WWW receivedData = new WWW(post_url);
        yield return receivedData;
        if (receivedData.error != null)
            print("There was an error in getting hiscores: " + receivedData.error);
        else
        {
            Data.Instance.GetComponent<Hiscores>().Init(receivedData.text);
        }
    }

    //IEnumerator GetScores()
    //{
    //    WWW receivedData = new WWW(highscore_URL);
    //    yield return receivedData;

    //    if (receivedData.error != null)
    //        print("There was an error getting the high score: " + receivedData.error);
    //    else
    //    {
    //        string[] allData = Regex.Split(receivedData.text, "</n>");

    //        for (var i = 0; i < allData.Length - 1; i++)
    //        {
    //            string[] userData = Regex.Split(allData[i], ":");
    //            string facebookId = userData[0];
    //            string userName = userData[1];
    //            string score = userData[2];
    //        }

    //        Data.Instance.events.OnHiscoresLoaded();
    //    }

    //}

}

