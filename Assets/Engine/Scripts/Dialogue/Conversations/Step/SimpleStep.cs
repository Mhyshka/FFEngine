using UnityEngine;
using System.Collections.Generic;
using FF.Scripting;

namespace FF.Dialogue
{
    internal class Step : ConversationStep
    {
        #region Properties
        protected List<Transition> _transitions = new List<Transition>();
        internal List<Transition> Transitions
        {
            get
            {
                return _transitions;
            }
        }
        #endregion
    }
}
