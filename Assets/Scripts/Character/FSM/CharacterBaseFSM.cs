using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseFSM
{
    protected PlayerControl playerControl;
    protected PlayerStateController playerStateController;

    public PlayerBaseFSM(PlayerControl target, PlayerStateController stateController)
    {
        playerControl = target;
        playerStateController = stateController;
    }

    public abstract void StateEnter();
    public abstract void StateExit();
    public abstract void StateUpdate();
    public abstract void StateFixedUpdate();
}
