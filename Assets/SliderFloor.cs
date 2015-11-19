using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SliderFloor : MonoBehaviour {

    public float scrollSpeed = 0.5F;
    public Renderer rend;

    void Start()
    {
        if (transform.parent.transform.localEulerAngles.y > 90) scrollSpeed *= -1;
    }
    void Update()
    {
        float offset = Time.time * scrollSpeed * 3;
        rend.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "floor" && other.tag != "grabbable")
        {
            GameObject go = null;
            if (other.tag == "Player")
            {
                go = other.transform.parent.gameObject;
            }
            else
            {
                go = other.gameObject;
            }
            if (go == null) return;
            if (go.GetComponent<SliderEffect>())
            {
                go.GetComponent<SliderEffect>().speed = scrollSpeed;
            }
            else
            {
                go.AddComponent<SliderEffect>().speed = scrollSpeed;
            }
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "enemy" )
        {
            GameObject go = other.gameObject;
            if (go.GetComponent<SliderEffect>())
            {
                go.GetComponent<SliderEffect>().speed = scrollSpeed;
            }
            else
            {
                go.AddComponent<SliderEffect>().speed = scrollSpeed;
            }
        }
    }
}
