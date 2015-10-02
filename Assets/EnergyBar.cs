using UnityEngine;
using System.Collections;

public class EnergyBar : MonoBehaviour {

    public Transform bar;
    public float fillValue = 0;
    public bool isOn;

    
    public void Init()
    {
        isOn = true;
        fillValue = 1;
        SetScale();
    }
    public void SetOff()
    {
        isOn = false;
    }
    public void UnFill(float qty)
    {
        this.fillValue -= qty;
        SetScale();
    }
    void SetScale()
    {
        if (!isOn) return;
        Vector3 scale = bar.localScale;
        scale.x = fillValue;
        bar.localScale = scale;

        if (fillValue <= 0)
            Data.Instance.events.OnAvatarProgressBarEmpty();
    }
}
