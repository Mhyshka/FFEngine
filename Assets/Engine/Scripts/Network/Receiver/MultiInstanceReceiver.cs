using UnityEngine;
using System.Collections;

using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class MultiInstanceReceiver<T> : BaseReceiver where T : BaseReceiver, new()
    {
        #region Properties
        #endregion

        internal sealed override void Read(ReadMessage a_message)
        {
            T instance = new T();
            instance.Read(a_message);
        }
    }
}
