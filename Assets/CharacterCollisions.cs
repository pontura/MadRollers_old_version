using UnityEngine;
using System;

public class CharacterCollisions : MonoBehaviour {

    private CharacterBehavior characterBehavior;
    private Player player;
    private bool hitted;

	void Start()
	{
        characterBehavior = gameObject.transform.parent.GetComponent<CharacterBehavior>();
        player = gameObject.transform.parent.GetComponent<Player>();
	}
	void OnTriggerEnter(Collider other) {
        if (other.tag == "wall") 
		{
            if (player.fxState == Player.fxStates.NORMAL)
            {
                characterBehavior.data.events.AddExplotion(transform.position);
                characterBehavior.Hit();
            }
            else
                other.GetComponent<WeakPlatform>().breakOut(transform.position);
        }
        if (other.tag == "destroyable") 
		{
            if (player.fxState == Player.fxStates.NORMAL)
                characterBehavior.HitWithObject(other.transform.position);
            breakBreakable(other.GetComponent<Breakable>(), other.transform.position);
        }
        else if (other.tag == "floor" && !hitted)
        {
            if (transform.position.y - other.transform.position.y < 0.5f)
            {
                characterBehavior.Hit();
            }
            else
            {
                hitted = true;
                characterBehavior.SuperJumpByBumped(1000, 0.5f);
                Invoke("resetHits", 1);
            }
            if (other.GetComponent<WeakPlatform>())
                other.GetComponent<WeakPlatform>().breakOut(characterBehavior.transform.position);           
        }
        else if (
            other.tag == "enemy"
            && characterBehavior.state != CharacterBehavior.states.JUMP
            && characterBehavior.state != CharacterBehavior.states.DOUBLEJUMP
            && characterBehavior.state != CharacterBehavior.states.SHOOT
            )
        {
            if (player.fxState == Player.fxStates.NORMAL && characterBehavior.state != CharacterBehavior.states.JETPACK)
                characterBehavior.Hit();
            other.GetComponent<MmoCharacter>().Die();
        }
    }
    void resetHits()
    {
        hitted = false;
    }
    void breakBreakable(Breakable breakable, Vector3 position)
    {
        try {
                breakable.breakOut(position);
            }
            catch (Exception e)  {
                print("error" + e);
            }  
    }
}
