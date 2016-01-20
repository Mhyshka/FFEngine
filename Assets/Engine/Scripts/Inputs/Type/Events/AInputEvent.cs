using UnityEngine;
using System.Collections;

using FF.Network.Message;

namespace FF.Input
{
	internal abstract class AInputEvent
	{
		#region Properties
		internal string eventKeyName;
		internal SimpleCallback onDown = null;
		internal SimpleCallback onUp = null;
        protected bool _isForwarded = false;
        protected bool _isPressed = false;
        internal bool IsPressed
        {
            get
            {
                return _isPressed;
            }
        }
		#endregion
		
		internal virtual void DoUpdate(){ }
		
		internal AInputEvent(EInputEventKey a_eventKey) : this(a_eventKey.ToString())
		{
		}
		
		internal AInputEvent(string a_eventKeyName)
		{
			eventKeyName = a_eventKeyName;
            _isForwarded = false;
		}

        #region Callbacks
        protected virtual void OnDown()
        {
            if(onDown != null)
                onDown();

            _isPressed = true;

            if (_isForwarded && Engine.Network.MainClient != null)
                Engine.Network.MainClient.QueueMessage(new MessageInputEvent(eventKeyName, true));
        }

        protected virtual void OnUp()
        {
            if (onUp != null)
                onUp();

            _isPressed = false;

            if (_isForwarded && Engine.Network.MainClient != null)
                Engine.Network.MainClient.QueueMessage(new MessageInputEvent(eventKeyName, false));
        }

        internal void ForceDown()
        {
            OnDown();
        }

        internal void ForceUp()
        {
            OnUp();
        }
        #endregion

        #region Forward
        internal void EnableNetworkForward()
        {
            _isForwarded = true;
        }

        internal void DisableNetworkForward()
        {
            _isForwarded = false;
        }
        #endregion
    }
}