using UnityEngine;
using System.Collections;

namespace FF
{
    /// <summary>
    /// Where T is the identifier of the state.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface IFsmState<T>
    {
        T ID
        {
            get;
        }
        void Enter(T a_previousStateId);
        T DoUpdate();
        void Exit(T a_targetStateId);
    }
}