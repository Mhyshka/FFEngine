using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class SentMessage : BaseMessage
    {
        #region Message Network Configuration
        protected string _channel;
        internal string Channel
        {
            get
            {
                return _channel;
            }
        }


        /// <summary>
        /// Should this message be send over & over until it's succesfully delivered.
        /// </summary>
        protected bool _isMandatory;
        internal virtual bool IsMandatory
        {
            get
            {
                return _isMandatory;
            }
        }

        /// <summary>
        /// Should this message be read by the server's local fake client.
        /// </summary>
        protected bool _isHandleByMock;
        internal virtual bool IsHandleByMock
        {
            get
            {
                return _isHandleByMock;
            }
        }
        #endregion

        internal SentMessage(MessageData a_data,
                                string a_channel,
                                bool a_isMandatory = true,
                                bool a_isHandleByMock = false)
        {
            _data = a_data;
            _timestamp = DateTime.Now.Ticks;
            _channel = a_channel;
            _isMandatory = a_isMandatory;
            _isHandleByMock = a_isHandleByMock;
        }

        #region Post Write
        internal SimpleCallback onMessageSent;

        internal virtual void PostWrite()
        {
            SimpleCallback copy = onMessageSent;
            onMessageSent = null;

            if (copy != null)
                copy();

            if (IsCompleteOnSent)
                OnComplete();
        }
        #endregion

        #region Serialization
        public override void SerializeData(FFByteWriter stream)
        {
            base.SerializeData(stream);
            stream.Write(_channel.GetHashCode());
        }

        internal byte[] Serialize()
        {
            FFByteWriter stream = new FFByteWriter();

            stream.Write((short)HeaderType);
            SerializeData(stream);

            stream.Write((short)Data.Type);
            Data.SerializeData(stream);

            stream.Close();
            FFLog.Log(EDbgCat.NetworkSerialization, "Serializing Request type : " + Data.Type.ToString());
            return stream.Data;
        }
        #endregion

        #region Complete
        protected virtual bool IsCompleteOnSent
        {
            get
            {
                return true;
            }
        }
        #endregion
    }
}
