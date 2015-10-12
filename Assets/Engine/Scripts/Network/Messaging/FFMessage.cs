using UnityEngine;
using System.Collections;
using System.Net.Sockets;

using System;

namespace FF.Networking
{
	
	
	internal abstract class FFMessage : IByteStreamSerialized
	{
		#region properties
        internal virtual bool HandleByMock
        {
            get
            {
                return false;
            }
        }

        protected FFTcpClient _client;
        internal FFTcpClient Client
        {
            set
            {
                _client = value;
            }
        }
        #endregion

        #region Methods
        internal abstract void Read();

        internal SimpleCallback onMessageSent;
        //Return true if writer thread should stop after writing.
        internal virtual bool PostWrite()
        {
            if (onMessageSent != null)
                onMessageSent();

            onMessageSent = null;
            return false;
        }
		
		/// <summary>
		/// Should this message be send over & over until it's succesfully delivered.
		/// </summary>
		internal virtual bool IsMandatory
		{
			get
			{
				return true;
			}
		}
		#endregion
		
		internal abstract EMessageType Type
		{
			get;
		}
		
		public abstract void SerializeData(FFByteWriter stream);
		
		internal byte[] Serialize()
		{
			FFByteWriter stream = new FFByteWriter();
			stream.Write((short)Type);
			SerializeData(stream);
			stream.Close();
			FFLog.Log(EDbgCat.Networking, "Serializing Request type : " + Type.ToString());
			return stream.Data;
		}
		
		public abstract void LoadFromData(FFByteReader stream);
		
		internal static FFMessage Deserialize(byte[] a_data)
		{
			FFByteReader stream = new FFByteReader(a_data);
			short value = stream.TryReadShort();
			EMessageType type = (EMessageType)value;
			FFLog.Log(EDbgCat.Networking, "Deserializing Request type : " + type.ToString());
			
			FFMessage message = FFMessageFactory.CreateMessage(type);
			if(message != null)
				message.LoadFromData(stream);
			else
				FFLog.LogError(EDbgCat.Networking, "Unkown message type : " + type);
				
			stream.Close();
			return message;
		}
	}
}