using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharactersManager : MonoBehaviour {

    public CharacterBehavior character;
  //  public EnergyBar energyBar;
    public List<CharacterBehavior> characters;
    private Vector3 characterPosition = new Vector3(0,0,0);

    private float separationX  = 2;

    public void Init()
    {
        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnAvatarFall += OnAvatarFall;

        Vector3 pos = new Vector3(1, 10, 1);
        int qty= 1;
      //  if (Data.Instance.mode == Data.modes.JOYSTICK) qty = 2;
        for (int a = 0; a < qty; a++)
        {
            pos.x = a * separationX;
            addCharacter(pos, a);
        }
    }
    void OnDestroy()
    {
        Data.Instance.events.OnAvatarCrash -= OnAvatarCrash;
        Data.Instance.events.OnAvatarFall -= OnAvatarFall;
    }
    void OnAvatarFall(CharacterBehavior characterBehavior)
    {
        killCharacter(characterBehavior.GetComponent<Player>().id);
    }
    void OnAvatarCrash(CharacterBehavior characterBehavior)
    {
        killCharacter(characterBehavior.GetComponent<Player>().id);
    }
    //void Update()
    //{
    //    if (Data.Instance.mode == Data.modes.JOYSTICK)
    //    {
    //        if (InputManager.getStart(0))
    //            if (!existsPlayer(0))
    //                addNewCharacter(0);

    //        if (InputManager.getStart(1))
    //            if (!existsPlayer(1))
    //                addNewCharacter(1);
    //    }
    //}
    private bool existsPlayer(int id)
    {
        bool exists = false;
        characters.ForEach((cb) =>
        {
            if (cb.player.id == id) exists = true;
        });
        return exists;
    }
    private void addNewCharacter(int id)
    {
        Vector3 pos = characters[0].transform.position;
        pos.y += 3;
        if (characters[0].transform.position.x > 0) pos.x += 1; else pos.x -= 1;
        Game.Instance.GetComponent<CharactersManager>().addCharacter(pos, id);
    }
    
    
    public void addCharacter(Vector3 pos, int id)
    {
        
        character = Instantiate(character, Vector3.zero, Quaternion.identity) as CharacterBehavior;

       // EnergyBar newEnergyBar = Instantiate(energyBar, Vector3.zero, Quaternion.identity) as EnergyBar;
       // newEnergyBar.Init(character.GetComponent<CharacterBehavior>());

        character.GetComponent<Player>().Init(id);

        character.GetComponent<Player>().id = id;

        characters.Add(character);
        character.transform.position = pos;
    }
    public void killCharacter(int id)
    {
        characters.ForEach((cb) =>
        {
            if (cb.player.id == id)
            {
                characters.Remove(cb);
                
                if (characters.Count == 0)
                {
                    StartCoroutine(restart(cb));
                }
            }
        });
        
    }
    IEnumerator restart(CharacterBehavior cb)
    {
        yield return new WaitForSeconds(0.05f);
        Data.Instance.events.OnAvatarDie(cb);
        yield return new WaitForSeconds(1.32f);
       // Destroy(cb.GetComponent<Player>().energyBar.gameObject);
        Destroy(cb.gameObject);
        Game.Instance.ResetLevel();
    }
    public bool isSecondPlayer(CharacterBehavior cb)
    {
        if (characters.Count == 0)
            return false;
        if (characters[0].player.id == cb.player.id)
            return false;
        return true;
    }
    public CharacterBehavior getMainCharacter()
    {
        return character;

        //if (characters.Count <= 0)
        //{
        //    print("[ERROR] No hay más characters y sigue pidiendo...");
        //    return null;
        //}
        //return characters[0];
    }
    public Vector3 getPositionMainCharacter()
    {
        return getMainCharacter().transform.position;
    }
    public Vector3 getPosition()
    {
        if (characters.Count == 2)
        {

            Vector3 pos1 = characters[0].transform.localPosition;
            Vector3 pos2 = characters[1].transform.localPosition;
            characterPosition = new Vector3((pos1.x + pos2.x) / 2, pos1.y + 2.8f, pos1.z - 1.4f);

            return characterPosition;
        }
        else if (characters.Count == 0) return characterPosition;
        else
            characterPosition = characters[0].transform.position;

        return characterPosition;
    }
    public int getTotalCharacters()
    {
        return characters.Count;
    }
    public float getDistance()
    {
        if (characters.Count == 0) 
            return 0;
        else 
            return characters[0].distance;
    }
}
