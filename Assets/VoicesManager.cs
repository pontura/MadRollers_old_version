﻿using UnityEngine;
using System.Collections;

public class VoicesManager : MonoBehaviour
{
    private bool firstMission = true;
    
    public AudioClip[] missionComplete;
    public AudioClip[] firstMissionStart;
    public AudioClip[] newMission;
    public AudioClip[] missions;
    public AudioClip[] avatarCrash;
    public AudioClip[] avatarFall;

    public AudioClip MissionHearts;
    public AudioClip MissionDistance;
    public AudioClip MissionKill1;
    public AudioClip MissionKill;
    public AudioClip MissionDestroy;
    public AudioClip MissionJump;
    public AudioClip MissionDoubleJump;
    public AudioClip MissionBomb1;
    public AudioClip MissionBombs;

    public AudioClip invencibleOn;
    public AudioClip invencibleOff;

    // Use this for initialization
    public void Init()
    {
        Data.Instance.events.OnMissionComplete += OnMissionComplete;
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnAvatarFall += OnAvatarFall;
        Data.Instance.events.OnAvatarChangeFX += OnAvatarChangeFX;
    }
    private void OnMissionComplete(int id)
    {
        PlayRandom(missionComplete);
    }
    private void OnAvatarCrash(CharacterBehavior cb)
    {
        PlayRandom(avatarCrash);
    }
    private void OnAvatarFall(CharacterBehavior cb)
    {
        PlayRandom(avatarFall);
    }
    private void OnAvatarChangeFX(Player.fxStates state)
    {
        if(state == Player.fxStates.NORMAL)
            PlayClip(invencibleOff);
        else
            PlayClip(invencibleOn);
    }
    private void OnListenerDispatcher(string message)
    {
        if (message == "ShowMissionId")
        {
            if (firstMission)
            {
                PlayRandom(firstMissionStart);
                firstMission = false;
            }
            else
                PlayRandom(newMission);
        }
        else if (message == "ShowMissionName")
            PlayMission();
    }
    void PlayMission()
    {
        int id = Data.Instance.missionActive;
        Mission MissionActive = Data.Instance.GetComponent<Missions>().MissionActive;
        if (MissionActive.hearts>0)
            PlayClip(MissionHearts);
        else if (id == 2)
            PlayClip(MissionJump);
        else if (id == 3)
            PlayClip(MissionDoubleJump);
        else if (MissionActive.distance > 0)
            PlayClip(MissionDistance);
        else if (MissionActive.guys == 1)
            PlayClip(MissionKill1);
        else if (MissionActive.guys > 1)
            PlayClip(MissionKill);
        else if (MissionActive.bombs == 1)
            PlayClip(MissionBomb1);
        else if (MissionActive.bombs > 0)
            PlayClip(MissionBombs);
        else if (MissionActive.distance > 0)
            PlayClip(MissionDistance);
       else if (MissionActive.distance > 0)
            PlayClip(MissionDistance);

    }
    void PlayRandom(AudioClip[] clips)
    {
        int rand = Random.Range(0, clips.Length);
        PlayClipInLibrary(clips[rand].name, clips); 
    }
    private void PlayClipInLibrary(string clip_name, AudioClip[] clipLibrary)
    {
        bool exists = false;
        foreach (AudioClip audioClip in clipLibrary)
        {
            if (audioClip.name == clip_name)
            {
                PlayClip( audioClip );
                exists = true;
            }
        }
        if (!exists) Debug.LogError("No esta agregado la voz: " + clip_name + " en " + clipLibrary);
    }
    void PlayClip(AudioClip audioClip)
    {
       // print("______voice CLIP : " + audioClip.name);
        GetComponent<AudioSource>().clip = audioClip;
        GetComponent<AudioSource>().Play();
    }
}
