using UnityEngine;
using System.Collections.Generic;
using FF.Scripting;

namespace FF.Dialogue
{
    internal abstract class ConversationStep
    {
        #region Properties
        protected Line _line = null;
        internal Line Line
        {
            get
            {
                return _line;
            }
        }

        protected List<Action> _actions = new List<Action>();
        internal List<Action> Actions
        {
            get
            {
                return _actions;
            }
        }
        #endregion
    }
}
