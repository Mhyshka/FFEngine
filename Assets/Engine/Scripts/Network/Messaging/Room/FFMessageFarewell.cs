using UnityEngine;
using System.Collections;

using FF.UI;

namespace FF.Networking
{
	internal class FFMessageFarewell : FFMessage
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

        public FFMessageFarewell()
		{
		}
		
		internal FFMessageFarewell(string a_reason)
		{
            reason = a_reason;
		}
		
		internal override void Read ()
		{
            _client.EndConnection(reason);
        }

        internal override bool PostWrite()
        {
            base.PostWrite();
            _client.EndConnection("The server closed this room.");
            return true;
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