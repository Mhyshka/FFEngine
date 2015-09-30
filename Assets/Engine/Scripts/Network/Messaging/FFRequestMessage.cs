using UnityEngine;
using System.Collections;

namespace FF.Networking
{
	internal abstract class FFRequestMessage : FFMessage
	{
        #region Properties
        internal int requestId = -1;
        internal SimpleCallback onTimeout = null;

        protected float _timeElapsed = 0f;
        protected virtual float TimeoutDuration
        {
            get
            {
                return 5f;
            }
        }
        #endregion

        internal bool CheckForTimeout()
        {
            _timeElapsed += Time.deltaTime;
            if(_timeElapsed > 5f)
            {
                if (onTimeout != null)
                    onTimeout();

                return true;
            }

            return false;
        }

        internal void Cancel()
        {
            FFEngine.Network.MainClient.CancelRequest(requestId);
        }

        #region Serialization
        public override void SerializeData (FFByteWriter stream)
		{
			stream.Write(requestId);
		}
		
		public override void LoadFromData (FFByteReader stream)
		{
			requestId = stream.TryReadInt();
		}
        #endregion
    }
}