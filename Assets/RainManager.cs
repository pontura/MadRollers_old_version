using UnityEngine;
using System.Collections;

public class RainManager : MonoBehaviour {

	public SceneObject[] objects;
    private float sec;
    private int delay = 4;
    private float randomX = 5;
    private float randomY = 1;

    public void updateByLevel(float distance)
    {
        sec+=Time.deltaTime;
        if(sec>delay)
        {
            addNewObject(distance);
            sec = 0;
        }
    }

    public SceneObject getRandomObject()
    {		
		return objects[Random.Range(0, objects.Length)];
	}
    public void addNewObject(float distance)
	{
        Vector3 pos = new Vector3(Random.Range(-randomX, randomX), Random.Range(-randomY, randomY), distance + 70);
        Game.Instance.level.addSceneObjectToScene(getRandomObject(), pos);
	}
}
