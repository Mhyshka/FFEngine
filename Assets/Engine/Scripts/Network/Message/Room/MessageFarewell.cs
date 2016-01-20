using UnityEngine;
using System.Collections;

using FF.UI;

namespace FF.Network.Message
{
	internal class MessageFarewell : AMessage
	{
        #region Properties
        public string reason = null;
		
	 	internal override EMessageType Type
	 	{
			get
			{
				return EMessageType.Farewell;
			}
		}

        internal override bool HandleByMock
        {
            get
            {
                return true;
            }
        }
        #endregion

        public MessageFarewell()
		{
		}
		
		internal MessageFarewell(string a_reason)
		{
            reason = a_reason;
		}

        internal override void PostWrite()
        {
            base.PostWrite();
            _client.EndConnection("The server closed this room.");
        }

        internal override bool ShouldStopAfterWrite
        {
            get
            {
                return true;
            }
        }

        internal override bool IsMandatory
        {
            get
            {
                return false;
            }
        }

        #region Serialization
        public override void SerializeData (FFByteWriter stream)
		{
			stream.Write(reason);
		}
		
		public override void LoadFromData (FFByteReader stream)
		{
			reason = stream.TryReadString();
		}
		#endregion
	}
}