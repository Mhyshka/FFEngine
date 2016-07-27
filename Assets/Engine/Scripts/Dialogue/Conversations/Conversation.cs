using UnityEngine;
using System.Collections;

namespace FF.Dialogue
{
    internal class Conversation
    {
        #region Properties
        protected ConversationStep _startStep;
        internal ConversationStep StartStep
        {
            get
            {
                return _startStep;
            }
        }

        #region Editor
        protected string _editorName;
        internal string EditorName
        {
            get
            {
                return _editorName;
            }
        }

        protected string _editorDescription;
        internal string EditorDescription
        {
            get
            {
                return _editorDescription;
            }
        }
        #endregion
        #endregion
    }
}
