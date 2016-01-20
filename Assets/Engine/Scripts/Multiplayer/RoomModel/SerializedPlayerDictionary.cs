using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;

using FF.Network;
using System;

namespace FF.Multiplayer
{
    internal class SerializedPlayerDictionary<T> : Dictionary<int, T> where T : IByteStreamSerialized, new()
    {
    }
}