using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {

    private int distanceFromAvatar = 20;    
    public float jumpHeight = 10;

   // private float sec = 0;
    private MmoCharacter mmoCharacter;

  //  private int lastState;

    void OnSceneObjectRestarted()
    {
        mmoCharacter = GetComponent<MmoCharacter>();
        mmoCharacter.waitToJump();
	}

    public void OnSceneObjectUpdated()
	{
        if (mmoCharacter.state != MmoCharacter.states.JUMP && mmoCharacter.charactersMmanager.getPosition().z > transform.position.z - distanceFromAvatar)
        {
            mmoCharacter.jump();
            GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpHeight * 100, 0), ForceMode.Impulse);
        }
        
	}
}
