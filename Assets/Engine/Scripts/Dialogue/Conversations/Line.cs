using UnityEngine;
using System.Collections;

namespace FF.Dialogue
{
    internal class Line
    {
        #region Properties
        protected Actor _source = null;
        internal Actor Source
        {
            get
            {
                return _source;
            }
        }

        protected string _content = "";
        internal virtual string Content
        {
            get
            {
                return _content;
            }
        }

        protected bool _isVocal = true;
        internal bool IsVocal
        {
            get
            {
                return _isVocal;
            }
        }

        protected AudioClip _voiceOver = null;
        internal AudioClip VoiceOver
        {
            get
            {
                return _voiceOver;
            }
        }
        #endregion
    }
}