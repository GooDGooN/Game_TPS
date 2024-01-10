using CharacterNamespace;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalEnums;


public abstract class CharacterBaseFSM
{
    protected CharacterStateController characterStateController;
    protected CharacterProperty characterInfo;
    public CharacterBaseFSM(CharacterStateController characterStateController, CharacterProperty characterInfo)
    {
        this.characterStateController = characterStateController;
        this.characterInfo = characterInfo;
    }

    public abstract void StateEnter();
    public abstract void StateExit();
    public abstract void StateUpdate();
    public abstract void StateFixedUpdate();
}
