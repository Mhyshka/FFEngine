using UnityEngine;
using System.Collections;

namespace FF.Network.Message
{
    internal class ReadMessage : BaseMessage
    {
        protected int _channel;
        internal int Channel
        {
            get
            {
                return _channel;
            }
        }


        internal ReadMessage() : base()
        {
        }

        internal ReadMessage(MessageData a_data,
                                long a_timestamp,
                                int a_channel)
        {
            _data = a_data;
            _timestamp = a_timestamp;
            _channel = a_channel;
        }

        protected void SetData(MessageData a_data)
        {
            _data = a_data;
        }

        public override void LoadFromData(FFByteReader stream)
        {
            base.LoadFromData(stream);
            _channel = stream.TryReadInt();
        }

        #region Deserialization
        internal static ReadMessage Deserialize(byte[] a_data)
        {
            FFByteReader stream = new FFByteReader(a_data);

            //Header
            short value = stream.TryReadShort();
            EHeaderType headerType = (EHeaderType)value;
            FFLog.Log(EDbgCat.NetworkSerialization, "Deserializing header type : " + headerType.ToString());
            ReadMessage message = MessageFactory.CreateMessage(headerType);
            message.LoadFromData(stream);

            //Data
            value = stream.TryReadShort();
            EDataType dataType = (EDataType)value;
            FFLog.Log(EDbgCat.NetworkSerialization, "Deserializing data type : " + dataType.ToString());
            MessageData data = MessageFactory.CreateData(dataType);

            if (data != null)
                data.LoadFromData(stream);
            else
                FFLog.LogError(EDbgCat.NetworkSerialization, "Unkown message type : " + dataType.ToString());

            message.SetData(data);

            stream.Close();

            return message;
        }
        #endregion
    }
}