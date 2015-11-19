using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RainManager : MonoBehaviour {

    private CharacterBehavior characterBehavior;
    private bool isCompetition;
    private float offset = 400;
    private float restaOffset = 20;
    private float min_offset = 150;
    private float distanceToAdd = 700;

    void Start()
    {
        if (Data.Instance.playMode == Data.PlayModes.COMPETITION)
        {
            isCompetition = true;
            characterBehavior = GetComponent<CharactersManager>().character;
        }
    }
    void Update()
    {
        if (!isCompetition) return;

        if (characterBehavior.distance > distanceToAdd)
        {
            distanceToAdd = characterBehavior.distance + (offset * 2);
            offset -= restaOffset;
            if (offset < min_offset) offset = min_offset;
            AddSceneObject(new Vector3(0, 0, characterBehavior.distance + 100), "Bomb1_real");
        }
    }
    public void AddSceneObject(Vector3 position, string sceneObjectName)
    {
        Vector3 newPos = position;
        newPos.x = Random.Range(-6, 6);
        SceneObject obj = ObjectPool.instance.GetObjectForType(sceneObjectName, true);
        if (obj)
        {
            obj.Restart(newPos);
        }
    }
}

