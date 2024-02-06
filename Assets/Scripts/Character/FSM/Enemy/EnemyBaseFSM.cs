using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseFSM : CharacterBaseFSM
{
    public EnemyBaseFSM(CharacterStateController characterStateController, CharacterProperty characterInfo) : base(characterStateController, characterInfo)
    {
        this.characterStateController = characterStateController;
        this.characterInfo = characterInfo;
    }
    protected PlayerControl player => PlayerControl.Instance;

}
