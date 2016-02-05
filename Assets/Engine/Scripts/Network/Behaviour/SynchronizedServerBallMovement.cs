﻿using UnityEngine;
using System.Collections;

namespace FF.Network
{
    internal class SynchronizedServerBallMovement : MonoBehaviour
    {
        protected void NetworkUpdate(Vector3 a_position, Vector3 a_velocity)
        {
            Message.MessageBallMovementData movement = new Message.MessageBallMovementData(a_position, a_velocity);
            Engine.Network.Server.BroadcastMessage(movement);
        }

        internal void UpdateNow(Vector3 a_position, Vector3 a_velocity)
        {
            NetworkUpdate(a_position, a_velocity);
        }
    }
}