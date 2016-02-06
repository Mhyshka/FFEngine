using UnityEngine;
using System.Collections;

namespace FF.Network.Message
{
    internal class ReadMessage : BaseMessage
    {
        internal ReadMessage() : base()
        {
        }

        internal ReadMessage(MessageData a_data,
                                long a_timestamp,
                                string a_channel)
        {
            _data = a_data;
            _timestamp = a_timestamp;
            _channel = a_channel;
        }

        protected void SetData(MessageData a_data)
        {
            _data = a_data;
        }

        #region Deserialization
        internal static ReadMessage Deserialize(byte[] a_data)
        {
            FFByteReader stream = new FFByteReader(a_data);

            //Header
            short value = stream.TryReadShort();
            EHeaderType headerType = (EHeaderType)value;
            FFLog.Log(EDbgCat.Serialization, "Deserializing header type : " + headerType.ToString());
            ReadMessage message = MessageFactory.CreateMessage(headerType);
            message.LoadFromData(stream);

            //Data
            value = stream.TryReadShort();
            EDataType dataType = (EDataType)value;
            FFLog.Log(EDbgCat.Serialization, "Deserializing data type : " + dataType.ToString());
            MessageData data = MessageFactory.CreateData(dataType);

            if (data != null)
                data.LoadFromData(stream);
            else
                FFLog.LogError(EDbgCat.Serialization, "Unkown message type : " + dataType.ToString());

            message.SetData(data);

            stream.Close();

            return message;
        }
        #endregion
    }
}