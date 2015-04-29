using UnityEngine;
using System.Collections;

public class EnergyBar : MonoBehaviour {

    //public UISprite progress;
    public float fillValue = 0;
    private float rechargeTime = 5f;
    private CharacterBehavior characterBehavior;
    public Animation _animation;

    public void Init(CharacterBehavior target)
    {
        this.characterBehavior = target;
        gameObject.SetActive(true);
    }
    public void hide()
    {
        gameObject.SetActive(false);
    }
    void LateUpdate()
    {
        transform.position = characterBehavior.transform.position;
    }
    public void setEnergy( float qty)
    {
        this.fillValue = qty;
       // progress.fillAmount = fillValue;
    }
    public void Animate()
    {
        _animation.Play("EnergyBarActive");
        _animation["EnergyBarActive"].normalizedTime = 0;
    }
}
