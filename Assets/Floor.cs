using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour
{

    [SerializeField]
    GameObject[] areas;
    public int z_length;
    private bool isMoving;

    private CharactersManager charactersManager;

    public void Init(CharactersManager charactersManager)
    {
        isMoving = true;
        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnAvatarFall += OnAvatarCrash;
        this.charactersManager = charactersManager;
    }
    void OnDEstroy()
    {
        Data.Instance.events.OnAvatarCrash -= OnAvatarCrash;
        Data.Instance.events.OnAvatarFall -= OnAvatarCrash;
    }
    void OnAvatarCrash(CharacterBehavior cb)
    {
        isMoving = false;
    }
    void Update()
    {
        if (!isMoving) return;
        if (!charactersManager) return;

        //if (charactersManager.getDistance() > transform.localPosition.z+10)
        //{
        Vector3 pos = transform.localPosition;
        pos.z = charactersManager.getDistance();
        transform.localPosition = pos;
        foreach (GameObject area in areas)
        {
            pos = area.transform.localPosition;
            pos.z -= 0.05f;

            if (pos.z < -z_length)
                pos.z = z_length;

            area.transform.localPosition = pos;
        }


    }
}
