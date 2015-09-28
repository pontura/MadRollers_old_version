using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

    public AudioClip interfaces;
    public AudioClip MainTheme;
    public AudioClip IndestructibleFX;
    public AudioClip heartClip;
    public AudioClip deathFX;

    private float heartsDelay = 0.1f;

	// Use this for initialization
	public void Init () {
        Data.Instance.events.OnMissionStart += OnMissionStart;
        Data.Instance.events.OnInterfacesStart += OnInterfacesStart;
        Data.Instance.events.OnAvatarChangeFX += OnAvatarChangeFX;
        Data.Instance.events.OnAvatarDie += OnAvatarDie;
        Data.Instance.events.OnGamePaused += OnGamePaused;
        Data.Instance.events.SetVolume += SetVolume;
	}
    void SetVolume(float vol)
    {
        GetComponent<AudioSource>().volume = vol;
    }
    private void playSound(AudioClip _clip, bool looped = true)
    {        
        if (GetComponent<AudioSource>().clip.name == _clip.name) return;
        stopAllSounds();
        GetComponent<AudioSource>().clip = _clip;
        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().loop = looped;
    }
    void OnGamePaused(bool paused)
    {
        if(paused)
            GetComponent<AudioSource>().Stop();
        else
            GetComponent<AudioSource>().Play();
    }
    void OnInterfacesStart()
    {
        playSound( interfaces );
    }
    void OnMissionStart(int id)
    {
        playSound(MainTheme);
	}
    void OnAvatarChangeFX(Player.fxStates state)
    {
        if (state == Player.fxStates.NORMAL)
            playSound(MainTheme);
        else
            playSound(IndestructibleFX);
    }
    void OnAvatarDie(CharacterBehavior player)
    {
        playSound(deathFX, false);
    }
    void stopAllSounds()
    {
        GetComponent<AudioSource>().Stop();
    }

    float nextHeartSoundTime;
    public void addHeartSound()
    {
        if (Time.time >= nextHeartSoundTime)
        {
          GetComponent<AudioSource>().PlayOneShot(heartClip);
          nextHeartSoundTime = Time.time + heartsDelay;
        }
    }
}
