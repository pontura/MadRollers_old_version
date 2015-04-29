using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimpleLogin : MonoBehaviour {

    [SerializeField]
    Text nameLabel;
    [SerializeField]
    Text passLabel;

    void Start()
    {
        Data.Instance.events.OnSetUserData += OnSetUserData;
    }
    void OnDestroy()
    {
        Data.Instance.events.OnSetUserData -= OnSetUserData;
    }
    void OnSetUserData(string id, int facebookId, int hiscore, bool saveIt)
    {
        Fade.LoadLevel("MainMenu", 1, 1, Color.black);
    }
    public void Registry()
    {
        if (nameLabel.text.Length > 1 && passLabel.text.Length > 1)
            RegistryOk();
    }
    void RegistryOk()
    {
        Data.Instance.GetComponent<DataController>().CreateUser(nameLabel.text, "0", Data.Instance.userData.hiscore);        
    }
    public void Back()
    {
        Fade.LoadLevel("Registry", 1, 1, Color.black);
    }
}
