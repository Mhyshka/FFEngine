using UnityEngine;
using System.Collections;

using FF.UI;

namespace FF.Network.Message
{
	internal class StringMessageData : MessageData
	{
        #region Properties
        public string _stringData = null;
        internal string StringData
        {
            get
            {
                return _stringData;
            }
        }
		
	 	internal override EDataType Type
	 	{
			get
			{
				return EDataType.String;
			}
		}
        #endregion

        public StringMessageData()
		{
		}
		
		internal StringMessageData(string a_string)
		{
            _stringData = a_string;
		}

        /*
        internal override void PostWrite()
        {
            base.PostWrite();
            //TODO - FIX THAT SHIT
            _client.EndConnection("The server closed this room.");
        }*/

        #region Serialization
        public override void SerializeData (FFByteWriter stream)
		{
			stream.Write(_stringData);
		}
		
		public override void LoadFromData (FFByteReader stream)
		{
			_stringData = stream.TryReadString();
		}
		#endregion
	}
}