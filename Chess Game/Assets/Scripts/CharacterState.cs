using System.Collections;
using System.Collections.Generic;
using Animancer;
using Animancer.FSM;
using Animancer.Samples.StateMachines;
using UnityEngine;


public abstract class CharacterState : StateBehaviour
{
    [SerializeField]
    private Character _Character;
    public Character Character => _Character;

    //below allows us to set the reference the correct character for all states
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();

        gameObject.GetComponentInParentOrChildren(ref _Character);
    }
#endif//assign the character state for every state (looks for the player object and make sure that it is assigned, even from the parent if needed) 

    public virtual CharacterStatePriority Priority => CharacterStatePriority.Low;
    public virtual bool CanInterruptSelf => false;

    public override bool CanExitState
    {
        get
        {
            // There are several different ways of accessing the state change details:
            // var nextState = StateChange<CharacterState>.NextState;
            // var nextState = this.GetNextState();
            var nextState = _Character.StateMachine.NextState;
            if (nextState == this) //if the next state is the current state (return false, cannot interrupt itself)
                return CanInterruptSelf;
            else if (Priority == CharacterStatePriority.Low) //check if the current state has a low priority
                return true; // true allow transition to the next state
            else //now we check other cases, if the next state has a higher priority than the current state we can transition to the next state
                return nextState.Priority > Priority;
        }
    }

}
