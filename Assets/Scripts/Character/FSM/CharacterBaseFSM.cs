using CharacterNamespace;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterBaseFSM
{
    protected CharacterStateController characterStateController;
    protected CharacterProperty characterInfo;
    public CharacterBaseFSM(CharacterStateController characterStateController, CharacterProperty characterInfo)
    {
        this.characterStateController = characterStateController;
        this.characterInfo = characterInfo;
    }
    public virtual void StateEnter() { }
    public virtual void StateExit() { }
    public virtual void StateUpdate() { }
    public virtual void StateFixedUpdate() { }
}
