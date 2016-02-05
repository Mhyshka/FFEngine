using UnityEngine;
using System.Collections;

using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal abstract class BaseReceiver
    {
        internal abstract void Read(ReadMessage a_message);
    }
}
