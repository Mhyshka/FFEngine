using UnityEngine;
using System.Collections;
using System;

using FF.Input;
using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class InputEventReceiver : BaseMessageReceiver
    {
        protected override void HandleMessage()
        {
            if (_message.HeaderType == EHeaderType.Message)
            {
                if (_message.Data.Type == EDataType.InputEvent)
                {
                    /*MessageInputEventData inputEventData = _message.Data as MessageInputEventData;
                    InputEventNetwork input = Engine.Inputs.ManagerForClient(_client.NetworkID).EventForKey(inputEventData.eventKey) as InputEventNetwork;
                    if (input != null)
                    {
                        if (inputEventData.isDown)
                            input.ForceDown();
                        else
                            input.ForceUp();
                    }
                    else
                    {
                        Debug.LogError("Input is null");
                    }*/
                }
            }
           
        }
    }
}