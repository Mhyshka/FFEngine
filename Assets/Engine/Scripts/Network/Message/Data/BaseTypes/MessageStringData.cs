using UnityEngine;
using System.Collections;

using FF.UI;

namespace FF.Network.Message
{
	internal class MessageStringData : MessageData
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

        public MessageStringData()
		{
		}
		
		internal MessageStringData(string a_string)
		{
            _stringData = a_string;
		}

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