using UnityEngine;

public abstract class CardBaseState
{
    public abstract void EnterState(GameStateManager game);
    public abstract void UpdateState(GameStateManager game);
    public abstract void ChangeState(GameStateManager game);

}
