using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreSignal : SceneObject
{
    public TextMesh field;

    public override void OnRestart(Vector3 pos)
    {
        base.OnRestart(pos);
        field.text = "A";

        Hashtable tweenData = new Hashtable();
        tweenData.Add("y", pos.y+3);
        tweenData.Add("time", 0.5f);
        tweenData.Add("easeType", iTween.EaseType.easeOutQuad);
        tweenData.Add("onComplete", "Pool");

        iTween.MoveTo(gameObject, tweenData);
    }
    public void SetScore(int qty)
    {
        field.text = "+" + qty.ToString();
    }
}
