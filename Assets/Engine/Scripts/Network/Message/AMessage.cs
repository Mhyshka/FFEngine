using UnityEngine;
using System.Collections;
using System.Net.Sockets;

using System;

namespace FF.Network.Message
{
	
	
	internal abstract class AMessage : IByteStreamSerialized
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
            get
            {
                return _client;
            }
            set
            {
                _client = value;
            }
        }
        #endregion

        #region Methods
        internal SimpleCallback onMessageSent;
        //Return true if writer thread should stop after writing.
        internal virtual void PostWrite()
        {
            if (onMessageSent != null)
                onMessageSent();

            onMessageSent = null;
        }

        internal virtual bool ShouldStopAfterWrite
        {
            get
            {
                return false;
            }
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

        #region Serialization
        public abstract void SerializeData(FFByteWriter stream);
		
		internal byte[] Serialize()
		{
			FFByteWriter stream = new FFByteWriter();
			stream.Write((short)Type);
			SerializeData(stream);
			stream.Close();
			FFLog.Log(EDbgCat.Serialization, "Serializing Request type : " + Type.ToString());
			return stream.Data;
		}
		
		public abstract void LoadFromData(FFByteReader stream);
		
		internal static AMessage Deserialize(byte[] a_data)
		{
			FFByteReader stream = new FFByteReader(a_data);
			short value = stream.TryReadShort();
			EMessageType type = (EMessageType)value;
			FFLog.Log(EDbgCat.Serialization, "Deserializing Request type : " + type.ToString());
			
			AMessage message = MessageFactory.CreateMessage(type);
			if(message != null)
				message.LoadFromData(stream);
			else
				FFLog.LogError(EDbgCat.Serialization, "Unkown message type : " + type);
				
			stream.Close();
			return message;
        }
        #endregion


    }
}