using UnityEngine;
using System.Collections;
using System;

using FF.Input;

namespace FF.Network.Receiver
{
    internal class MessageInputEvent : AReceiver<Message.MessageInputEvent>
    {
        protected override void HandleMessage()
        {
            InputEventNetwork input = Engine.Inputs.ManagerForClient(_message.Client.NetworkID).EventForKey(_message.eventKey) as InputEventNetwork;
            if (input != null)
            {
                if (_message.isDown)
                    input.ForceDown();
                else
                    input.ForceUp();
            }
            else
            {
                Debug.LogError("Input is null");
            }
        }
    }
}