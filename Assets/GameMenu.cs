﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class GameMenu : MonoBehaviour {

    public GameObject popup;
    public GameObject popupReset;
    public Animation anim;
    public GameObject button;
    public Text soundsLabel;
    public GameObject soundOn;
    public GameObject soundOff;
    public bool sounds = true;

    void Start()
    {
        popupReset.SetActive(false);
        popup.SetActive(false);
        button.SetActive(false);
        soundsLabel.text = "SHHHH!!!";
    }
    public void SetOn()
    {
        button.SetActive(true);
    }
    public void Init()
    {
        Data.Instance.events.OnFadeALittle(true);
        //Time.timeScale = 0;
        Data.Instance.events.OnGamePaused(true);
        popup.SetActive(true);
        StartCoroutine(Play(anim, "GameMenuOpen", false, null));
	}
    public void ToogleSounds()
    {
        if(sounds)
        {
            soundsLabel.text = "MÚSICA!";
            soundOn.SetActive(false);
            soundOff.SetActive(true);
            Data.Instance.events.SetVolume(0);
        }else{
            soundsLabel.text = "SHHHH!!!";
            soundOn.SetActive(true);
            soundOff.SetActive(false);
            Data.Instance.events.SetVolume(1);
        }
        sounds = !sounds;
        Close();
    }
    public void Close()
    {
        Data.Instance.events.OnGamePaused(false);
        Data.Instance.events.OnFadeALittle(false);
        StartCoroutine(Play(anim, "GameMenuClose", false, Reset));
    }
    
    public void ChangeLevels()
    {
        Data.Instance.events.OnResetLevel();
        SocialEvents.OnGetHiscores(1);
        Data.Instance.LoadLevel("MainMenu");
        Close();
    }
    public void OpenResetPopup()
    {
        popupReset.SetActive(true);
        popup.SetActive(false);
        StartCoroutine(Play(popupReset.animation, "GameMenuOpen", false, Reset));
    }
    public void ResetGame()
    {
        Data.Instance.resetProgress();
        ChangeLevels();
        Data.Instance.events.OnFadeALittle(false);
        StartCoroutine(Play(popupReset.animation, "GameMenuClose", false, Reset));
    }
    public void CloseResetPopup()
    {
        Data.Instance.events.OnFadeALittle(false);
        StartCoroutine(Play(popupReset.animation, "GameMenuClose", false, Reset));
    }
    private void Reset()
    {
        
        Time.timeScale = 1;
        popup.SetActive(false);
       // Game.Instance.UnPause();
        Data.Instance.events.OnCloseMainmenu();
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
