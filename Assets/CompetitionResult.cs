using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CompetitionResult : MonoBehaviour {

	public GameObject panel;
    public ProfilePicture avatar1;
    public ProfilePicture avatar2;
    public Text label;
    public ProgressBar ingameBar;
    public ProgressBar bar;
    public GameObject missionPanel;

    void Start()
    {
        panel.SetActive(false);

        if (Data.Instance.playMode == Data.PlayModes.STORY)
            return;

        Data.Instance.events.OnAvatarCrash += OnAvatarDie;
        Data.Instance.events.OnAvatarFall += OnAvatarDie;

    }
    void OnDestroy()
    {
        Data.Instance.events.OnAvatarCrash -= OnAvatarDie;
        Data.Instance.events.OnAvatarFall -= OnAvatarDie;
    }
    void OnAvatarDie(CharacterBehavior cb)
    {
        panel.SetActive(true);
        CompetitionManager cm = GetComponent<CompetitionManager>();
        avatar1.SetLoadedPicture(cm.avatar1.GetComponent<Image>().sprite.texture);
        avatar2.SetLoadedPicture(cm.avatar2.GetComponent<Image>().sprite.texture);
        bar.setProgression(ingameBar.progression);
        CharacterBehavior characterBehavior = GetComponent<CharactersManager>().character;
        label.text = (int)characterBehavior.distance + "m";
        missionPanel.SetActive(false);
    }
}
