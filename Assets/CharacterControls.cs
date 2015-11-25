﻿using UnityEngine;
using System.Collections;

public class CharacterControls : MonoBehaviour {

    CharacterBehavior characterBehavior;
    Player player;
    private float rotationY;
    private float rotationZ = 0;
    private float turnSpeed = 2.8f;
    private float speedX = 9f;
    private bool mobileController;
    private bool ControlsEnabled = false;
    private CharactersManager charactersManager;

	void Start () {
        characterBehavior = GetComponent<CharacterBehavior>();
        player = GetComponent<Player>();
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            mobileController = true;
        StartCoroutine(enabledMovements());

        charactersManager = Game.Instance.GetComponent<CharactersManager>();
	}
    IEnumerator enabledMovements()
    {
        yield return new WaitForSeconds(0.5f);
        ControlsEnabled = true;
    }
	// Update is called once per frame
	void Update () {
        if (characterBehavior.state == CharacterBehavior.states.CRASH || characterBehavior.state == CharacterBehavior.states.DEAD) return;
       // transform.Translate(0, 0, Time.deltaTime * speedRun);

        // los players siguen al player 1ero
        //if (charactersManager.isSecondPlayer(characterBehavior))
        //{
        //    Vector3 pos = charactersManager.getPositionMainCharacter();
        //    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, pos.z);
        //}
        

       // if (!ControlsEnabled) return;


        if (mobileController)
            moveByAccelerometer();
        else
        {
            if (InputManager.getFire(player.id))
            {
                characterBehavior.CheckFire();
            }
            if (InputManager.getJump(player.id))
            {
                characterBehavior.Jump();
            } else
            if (Input.GetButton("Jump1"))
            {
                characterBehavior.JumpPressed();
            }
            else
            {
                characterBehavior.AllButtonsReleased();
            }
           
            moveByKeyboard();
        }

        if (Time.deltaTime == 0) return;
        characterBehavior.UpdateByController();
        player.UpdateByController();
	}

    private void moveByAccelerometer()
    {



        if (Input.touchCount > 0)
        {
            var touch = Input.touches[0];
           if (touch.position.x < Screen.width / 2)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                    characterBehavior.Jump();
                else
                {
                    characterBehavior.JumpPressed();
                }
            }
            else if (touch.position.x > Screen.width / 2)
            {
                characterBehavior.CheckFire();
            }
        } else
        {
            characterBehavior.AllButtonsReleased();
        } 


        if (Time.deltaTime == 0) return;
        transform.localRotation = Quaternion.Euler(transform.localRotation.x, Input.acceleration.x * 50, rotationZ);
       // transform.Translate(0, 0, Time.deltaTime * characterBehavior.speed);

    }

    private void moveByKeyboard()
    {
        
        float newPosX = InputManager.getHorizontal(player.id) * speedX;

        if (newPosX == 0)
        {
            rotationY = 0;
        }
        else
            if (newPosX > 0)
                rotationY += turnSpeed;
            else if (newPosX < 0)
                rotationY -= turnSpeed;
            else if (rotationY > 0)
                rotationY -= turnSpeed;
            else if (rotationY < 0)
                rotationY += turnSpeed;

        if (rotationY > 30) rotationY = 30;
        else if (rotationY < -30) rotationY = -30;

        if (Time.deltaTime == 0) return;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rotationY, rotationZ);


    }
}
