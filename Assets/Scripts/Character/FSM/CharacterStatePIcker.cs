using CharacterNamespace;

public class CharacterStatePIcker
{
    public static CharacterBaseFSM GetState(CharacterState state, CharacterStateController stateController)
    {
        if (stateController.TargetCharacter is PlayerControl)
        {
            return ReturnPlayerState(state, stateController);
        }
        if (stateController.TargetCharacter is EnemyProperty)
        {
            return ReturnEnemyState(state, stateController);
        }
        return null;
    }

    public static CharacterBaseFSM GetState(CharacterUpperState state, CharacterStateController stateController)
    {
        if (stateController.TargetCharacter is PlayerControl)
        {
            return ReturnPlayerUpperState(state, stateController);
        }
        if (stateController.TargetCharacter is EnemyProperty)
        {
            //return ReturnState(state, stateController);
        }
        return null;
    }

    #region PLAYER_RETURN
    private static CharacterBaseFSM ReturnPlayerState(CharacterState state, CharacterStateController stateController)
    {
        var player = stateController.TargetCharacter as PlayerControl;
        switch (state)
        {
            case CharacterState.Idle:
                return new PlayerIdleState(stateController, player);
            case CharacterState.MidAir:
                return new PlayerMidAirState(stateController, player);
            case CharacterState.Move:
                return new PlayerMoveState(stateController, player);
            case CharacterState.Dash:
                return new PlayerDashState(stateController, player);
            default: return null;
        }
    }
    private static CharacterBaseFSM ReturnPlayerUpperState(CharacterUpperState state, CharacterStateController stateController)
    {
        var player = stateController.TargetCharacter as PlayerControl;
        switch (state)
        {
            case CharacterUpperState.Normal:
                return new PlayerNormalUpperState(stateController, player);
            case CharacterUpperState.Firing:
                return new PlayerFireUpperState(stateController, player);
            case CharacterUpperState.Reloading:
                return new PlayerReloadUpperState(stateController, player);
            default: return null;
        }
    }
    #endregion

    #region ENEMY_RETURN
    private static CharacterBaseFSM ReturnEnemyState(CharacterState state, CharacterStateController stateController)
    {
        var enemy = stateController.TargetCharacter as EnemyProperty;

        if(state == CharacterState.Move)
        {
            switch(enemy.MyType)
            {
                case EnemyType.SlimeRabbit: return new SlimeRabbitMoveState(stateController, enemy);
            }
        }
        else if (state == CharacterState.Attack)
        {
            switch (enemy.MyType)
            {
                case EnemyType.SlimeRabbit: return new SlimeRabbitAttackState(stateController, enemy);
            }
        }

        return null;
    }
    #endregion
}

