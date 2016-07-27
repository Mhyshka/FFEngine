using UnityEngine;
using System.Collections;

namespace FF.Dialogue
{
    internal class Actor
    {
        #region Properties
        protected string _gameName = "";
        internal virtual string DisplayName
        {
            get
            {
                return _gameName;
            }
        }

        internal Texture2D portrait = null;

        #region Editor
        protected string _editorName = "char_0001";
        internal string EditorName
        {
            get
            {
                return _editorName;
            }
        }
        #endregion
        #endregion
    }
}