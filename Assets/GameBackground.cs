using UnityEngine;
using System.Collections;

public class GameBackground : MonoBehaviour {

    private CharactersManager charactersManager;

    void Start()
    {
        charactersManager = Game.Instance.GetComponent<CharactersManager>();
    }
	void Update () {
        Vector3 pos = charactersManager.getPosition();
        pos.x = 0;
        pos.y = 0;
        transform.position = pos;
	}
}
