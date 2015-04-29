using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fade : MonoBehaviour
{
    private static Fade m_Instance = null;
    private Image masker;
    private string m_LevelName = "";
    private int m_LevelIndex = 0;
    private bool m_Fading = false;

    private static Fade Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = (new GameObject("Fade")).AddComponent<Fade>();
            }
            return m_Instance;
        }
    }
    public static bool Fading
    {
        get { return Instance.m_Fading; }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        m_Instance = this;
		masker = GetComponentInChildren<Image>();
		masker.color = new Color(0,0,0,0);
	}

    private IEnumerator FadeStart(float aFadeOutTime, float aFadeInTime, Color aColor)
    {

        float t = 0;
		while (t < 1)
		{
			yield return new WaitForEndOfFrame();
			t+=Time.deltaTime;
			masker.color = new Color(0,0,0,t);
		}

        if (m_LevelName != "")
            Application.LoadLevel(m_LevelName);
        else
            Application.LoadLevel(m_LevelIndex);     
		while (t > 0f)
		{
			yield return new WaitForEndOfFrame();
			t-=Time.deltaTime;
			masker.color = new Color(0,0,0,t);
		}

        m_Fading = false;
    }
    private void StartFade(float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        m_Fading = true;
        StartCoroutine(FadeStart(aFadeOutTime, aFadeInTime, aColor));
    }

    public static void LoadLevel(string aLevelName, float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        //if (Fading) return;
        Instance.m_LevelName = aLevelName;
        Instance.StartFade(aFadeOutTime, aFadeInTime, aColor);
    }
    public static void LoadLevel(int aLevelIndex, float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        if (Fading) return;
        Instance.m_LevelName = "";
        Instance.m_LevelIndex = aLevelIndex;
        Instance.StartFade(aFadeOutTime, aFadeInTime, aColor);
    }
}