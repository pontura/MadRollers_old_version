using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Summary : MonoBehaviour {

    public GameObject panel;
    public GameObject panela;
    public GameObject panelb;
    public GameObject panelc;
    public Text meters;
    public Text hearts;
    public Text heartsTotal;
    public Text heartsTotal2;
    public Text heartsTotal3;
    public Text heartsToRevive;
    public Text Continue;
    public Text ladoBField;
    public Text ladoCField;
    public Button ContinueButton;
    private int countDown;
    public Animation anim;
    int totalHearts;
    int newHearts;
    private int heartsToReviveNum = 250;
    private bool cancelCountDown;

    void Start()
    {
        panel.SetActive(false);
        panela.SetActive(false);
        panelb.SetActive(false);
        panelc.SetActive(false);
        Data.Instance.events.OnAvatarFall += Init;
        Data.Instance.events.OnAvatarCrash += Init;
        countDown = 9;
    }
    void OnDestroy()
    {
        Data.Instance.events.OnAvatarFall -= Init;
        Data.Instance.events.OnAvatarCrash -= Init;
    }
    void Init(CharacterBehavior cb)
    {
        Invoke("SetOn", 1);
        meters.text = (int)cb.distance + " mts";
    }
    void SetOn()
    {
        if (Data.Instance.playMode == Data.PlayModes.STORY)
        {
            Restart();
            return;
        }
        totalHearts = GetComponent<HearsManager>().total;
        if (heartsToReviveNum > totalHearts && Data.Instance.SummaryHasBeenDisplayedOnce)
        {
            Game.Instance.ResetLevel();
            return;
        }
        Data.Instance.SummaryHasBeenDisplayedOnce = true;

        panel.SetActive(true);
        panela.SetActive(true);
        
        
        
        newHearts = GetComponent<HearsManager>().newHearts;
        heartsToRevive.text = "x" + heartsToReviveNum.ToString();
        hearts.text = "+" + newHearts.ToString();

        heartsTotal.text = totalHearts.ToString();
        heartsTotal2.text = totalHearts.ToString();
        heartsTotal3.text = totalHearts.ToString();

        Invoke("CountDown", 0.5f);

        StartCoroutine(Play(anim, "popupOpen", false, null));

	}
    void CountDown()
    {
        if (cancelCountDown) return;
        if (countDown < 2)
        {
            Restart();
            return;
        }
        countDown--;
        Continue.text = countDown.ToString();
        Invoke("CountDown", 0.5f);
    }
	public void Revive()
    {
        cancelCountDown = true;
        if (heartsToReviveNum < totalHearts)
            if (Data.Instance.hasContinueOnce)
            {
                ReviveConfirma();
            }
            else
            {
                Data.Instance.hasContinueOnce = true;
                ladoB();
            }
        else
            ladoC();
    }
    public void Restart()
    {
        Game.Instance.ResetLevel();
    }
    void ladoB()
    {
        StartCoroutine(Play(anim, "popupReverse", false, ActiveB));
        ladoBField.text = "Seguro quieres invertir " + heartsToReviveNum  + " en revivir?";
    }
    void ActiveB()
    {
        StartCoroutine(Play(anim, "popupReverseB", false, null));
        panela.SetActive(false);
        panelb.SetActive(true);
    }
    void ladoC()
    {
        StartCoroutine(Play(anim, "popupReverse", false, ActiveC));
        ladoCField.text = "Necesitas " + heartsToReviveNum + " corazones por lo menos y no los tienes...";
    }
    void ActiveC()
    {
        StartCoroutine(Play(anim, "popupReverseB", false, null));
        panela.SetActive(false);
        panelc.SetActive(true);
    }
    public void ReviveConfirma()
    {
        Data.Instance.events.OnUseHearts(heartsToReviveNum);
        Data.Instance.events.OnSoundFX("consumeHearts");
        
        panela.SetActive(false);
        panelb.SetActive(false);
        panelc.SetActive(false);
        panel.SetActive(false);

        Invoke("ReviveTimeOut", 1);

    }
    void ReviveTimeOut()
    {
        Game.Instance.Revive();
    }
    private IEnumerator Play(this Animation animation, string clipName, bool useTimeScale, Action onComplete)
    {

        //We Don't want to use timeScale, so we have to animate by frame..
        if (!useTimeScale)
        {
            AnimationState _currState = animation[clipName];
            bool isPlaying = true;
            float _progressTime = 0F;
            float _timeAtLastFrame = 0F;
            float _timeAtCurrentFrame = 0F;
            float deltaTime = 0F;


            animation.Play(clipName);

            _timeAtLastFrame = Time.realtimeSinceStartup;
            while (isPlaying)
            {
                _timeAtCurrentFrame = Time.realtimeSinceStartup;
                deltaTime = _timeAtCurrentFrame - _timeAtLastFrame;
                _timeAtLastFrame = _timeAtCurrentFrame;

                _progressTime += deltaTime;
                _currState.normalizedTime = _progressTime / _currState.length;
                animation.Sample();

                if (_progressTime >= _currState.length)
                {
                    if (_currState.wrapMode != WrapMode.Loop)
                    {
                        isPlaying = false;
                    }
                    else
                    {
                        _progressTime = 0.0f;
                    }

                }

                yield return new WaitForEndOfFrame();
            }
            yield return null;
            if (onComplete != null)
            {
                onComplete();
            }
        }
        else
            animation.Play(clipName);
    }  
}
