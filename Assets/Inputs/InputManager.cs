﻿using UnityEngine;
using System.Collections;

public class InputManager {

    private static InputType[] inputs;
    private static bool initialized;
    private const int PLAYERS_COUNT = 2;

    
    public static bool getStart(int id = 0)
    {
        if (!initialized) Init();
        return inputs[id].getStart();
    }
    public static float getHorizontal(int id = 0)
    {
        if (!initialized) Init();
        return inputs[id].getHorizontal();
    }
    public static bool getOpenMenu(int id = 0)
    {
        if (!initialized) Init();
        return inputs[id].getOpenMenu();
    }
    public static bool getFire(int id = 0)
    {
         if (!initialized) Init();
         return inputs[id].getFire();
    }
    public static bool getJump(int id = 0)
    {
        if (!initialized) Init();
        return inputs[id].getJump();
    }

    static void Init()
    {
        inputs = new InputType[PLAYERS_COUNT];
        for (int a = 0; a < PLAYERS_COUNT; a++)
        {
            inputs[a] = new InputKeyboard(a);
        }
        initialized = true;
    }
}
