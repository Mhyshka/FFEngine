using UnityEngine;
using System.Collections.Generic;
using FF.Scripting;

namespace FF.Dialogue
{
    internal class Transition
    {
        #region Properties
        protected List<Condition> _conditions = new List<Condition>();
        internal List<Condition> Conditions
        {
            get
            {
                return _conditions;
            }
        }

        protected ConversationStep _targetStep = null;
        internal ConversationStep TargetStep
        {
            get
            {
                return _targetStep;
            }
        }
        #endregion
    }
}
