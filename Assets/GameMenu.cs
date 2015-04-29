using UnityEngine;
using System.Collections;
using System;

public class GameMenu : MonoBehaviour {

    public void Init()
    {
        Game.Instance.Pause();
        StartCoroutine(Play(GetComponent<Animation>(), "GameMenuOpen", false, null));
	}

    public void Close()
    {
        StartCoroutine(Play(GetComponent<Animation>(), "GameMenuClose", false, Reset));
    }
    public void Restart()
    {
        Data.Instance.resetProgress();
        Game.Instance.GotoMainMenu();
    }
    public void ChangeLevels()
    {
        Game.Instance.GotoLevelSelector();
    }
    private void Reset()
    {
        Game.Instance.UnPause();
        Data.Instance.events.OnCloseMainmenu();
        gameObject.SetActive(false);
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
