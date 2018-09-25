using System;
using System.Collections.Generic;
using System.Linq;

public abstract class UState<TParamStruct> where TParamStruct : struct
{
    /// <summary>
    /// The state machine that the state is connected to.
    /// </summary>
	public UStateMachine<TParamStruct> Parent;

    /// <summary>
    /// Called upon entry to the state.
    /// </summary>
    /// <param name="ps">The state machines parameters</param>
	public virtual void Enter(TParamStruct ps) { }
    /// <summary>
    /// Called every frame.
    /// </summary>
    /// <param name="ps">The state machines parameters</param>
	public virtual void Update(TParamStruct ps) { }
    /// <summary>
    /// Called every fixed framerate frame.
    /// </summary>
    /// <param name="ps">The state machines parameters</param>
	public virtual void FixedUpdate(TParamStruct ps) { }
    /// <summary>
    /// Called every GUI Event (Can be multiple time per frame)
    /// </summary>
    /// <param name="ps"></param>
	public virtual void OnGUI(TParamStruct ps) { }
    /// <summary>
    /// Draws gizmos.
    /// </summary>
    /// <param name="ps"></param>
	public virtual void OnDrawGizmos(TParamStruct ps) { }
    /// <summary>
    /// Called every frame.
    /// </summary>
    /// <param name="ps"></param>
	public virtual void CheckForTransitions(TParamStruct ps) { }
    /// <summary>
    /// Called on the exit of the state.
    /// </summary>
    /// <param name="ps"></param>
	public virtual void Exit(TParamStruct ps) { }
}