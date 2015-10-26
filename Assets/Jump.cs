using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {

    private int distanceFromAvatar = 20;    
    public float jumpHeight = 10;

   // private float sec = 0;
    private MmoCharacter mmoCharacter;

    public void Start()
    {
        mmoCharacter = GetComponent<MmoCharacter>();
        mmoCharacter.waitToJump();
    }
    void OnDisable()
    {
        Destroy(gameObject.GetComponent("Jump"));
    }
    void OnSceneObjectRestarted()
    {
        
	}
    public void OnSceneObjectUpdated()
	{
        if (mmoCharacter.state != MmoCharacter.states.JUMP && mmoCharacter.distanceFromCharacter < distanceFromAvatar)
        {
            mmoCharacter.jump();
            GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpHeight * 100, 0), ForceMode.Impulse);
        }
        
	}
}
