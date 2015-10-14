using UnityEngine;
using System.Collections;

public class CharacterFloorCollitions : MonoBehaviour {

    private CharacterBehavior characterBehavior;
    private Vector3 offset = new Vector3(0, 2f, 0);
    private int skip = ~((1 << 9) | (1 << 10) | (1 << 11) | (1 << 12) | (1 << 13) | (1 << 14) | (1 << 15) | (1 << 16) | (1 << 17) | (1 << 18) | (1 << 19));
    private states state;
    private enum states
    {
        ON_FLOOR,
        ON_AIR,
        SHOOTING,
        ON_FLY
    }

	void Start () {
        characterBehavior = gameObject.transform.parent.GetComponent<CharacterBehavior>();
        Data.Instance.events.OnAvatarJump += OnAvatarJump;
        Data.Instance.events.OnAvatarShoot += OnAvatarShoot;
	}
    public void OnDestroy()
    {
        Data.Instance.events.OnAvatarJump -= OnAvatarJump;
        Data.Instance.events.OnAvatarShoot -= OnAvatarShoot;
    }
    public void OnAvatarFly()
    {
        state = states.ON_FLY;
        characterBehavior.transform.localEulerAngles = new Vector3(0, 0, 0);
       // Invoke("WaitToResetCollliders", 0.5f);
    }
    public void OnAvatarFalling()
    {
        state = states.ON_AIR;
    }
    public void OnAvatarJump()
    {
        state = states.ON_AIR;
        characterBehavior.transform.localEulerAngles = new Vector3(0, 0, 0);
    }
    public void OnAvatarShoot()
    {
        state = states.SHOOTING;
    }
    private int resetShooting;
    void Update ()
    {
        if (characterBehavior.state == CharacterBehavior.states.DEAD 
            || characterBehavior.state == CharacterBehavior.states.CRASH
            || characterBehavior.state == CharacterBehavior.states.FALL) 
            return;

        if (state == states.ON_FLY) return;

        if (state == states.ON_FLOOR)
        {
            Vector3 pos = characterBehavior.transform.localPosition;
            RaycastHit hit;

            if (Physics.Raycast(pos + offset, -Vector3.up, out hit, offset.y, skip))
            {
                Vector3 newPos = characterBehavior.transform.localPosition;
                newPos.y = hit.point.y;
                characterBehavior.transform.localPosition = newPos;
                characterBehavior.GetComponent<Rigidbody>().velocity = Vector3.zero;

                float RotationY = characterBehavior.transform.localEulerAngles.y;
                float RotationZ = characterBehavior.transform.localEulerAngles.z;
                characterBehavior.transform.up = hit.normal;
                characterBehavior.transform.localEulerAngles = new Vector3(characterBehavior.transform.localEulerAngles.x, RotationY, RotationZ);
            }
            resetShooting = 0;
        }
        if (state == states.SHOOTING)
        {
            resetShooting++;
            if (resetShooting > 20)
            {
                state = states.ON_FLOOR;
                characterBehavior.Run();
            }
        }
    }
    

	void OnTriggerEnter(Collider other) {

        if (characterBehavior.state == CharacterBehavior.states.DEAD
            || characterBehavior.state == CharacterBehavior.states.CRASH
            || characterBehavior.state == CharacterBehavior.states.FALL)
            return;

        if (state == states.ON_FLY) return;

        if (other.tag == "floor" && state != states.ON_FLOOR)
        {
            state = states.ON_FLOOR;
            characterBehavior.Run();
          //  characterBehavior.setRotation(other.transform.localEulerAngles);
        }
        else
        {
            if (other.tag == "enemy")
            {
                if (characterBehavior.state == CharacterBehavior.states.JUMP ||
                    characterBehavior.state == CharacterBehavior.states.DOUBLEJUMP)
                {
                    other.GetComponent<MmoCharacter>().Die();
                    characterBehavior.SuperJumpByBumped(900);
                }
            }
        }
		
	}
}
