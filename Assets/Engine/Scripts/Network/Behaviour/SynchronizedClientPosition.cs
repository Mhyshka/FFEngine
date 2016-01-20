using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FF.Network
{
    internal class SynchronizedClientPosition : MonoBehaviour
    {
        #region Inspector Properties
        public float lerpSpeed = 5f;
        #endregion

        #region Properties
        internal LinkedList<NetworkPositionEvent> _positionEvents = new LinkedList<NetworkPositionEvent>();
        #endregion

        #region Network
        internal Receiver.GenericReceiver<Message.MessagePositionEvent> _receiver;

        void Awake()
        {
            _receiver = new Receiver.GenericReceiver<Message.MessagePositionEvent>(OnMessageReceived);
            Engine.Receiver.RegisterReceiver(Message.EMessageType.PositionEvent, _receiver);
        }

        void OnDestroy()
        {
            Engine.Receiver.UnregisterReceiver(Message.EMessageType.PositionEvent, _receiver);
        }

        void OnMessageReceived(Message.MessagePositionEvent a_message)
        {
            _positionEvents.AddLast(a_message.positionEvent);
        }
        #endregion

        void Update()
        {
            float movementDistance = Mathf.Lerp(0f, TotalDistance, Time.deltaTime * lerpSpeed);
            transform.position = PositionForDistance(movementDistance);
        }

        float TotalDistance
        {
            get
            {
                float distance = 0f;

                if (_positionEvents.Count > 0)
                {
                    Vector3 current = transform.position;
                    foreach (NetworkPositionEvent each in _positionEvents)
                    {
                        distance += Vector3.Distance(each.position, current);
                        current = each.position;
                    }
                }

                return distance;
            }
        }

        Vector3 PositionForDistance(float a_distanceMoved)
        {
            Vector3 position = transform.position;

            if (_positionEvents.Count > 0)
            {
                Vector3 previous = transform.position;
                LinkedListNode<NetworkPositionEvent> current = _positionEvents.First;

                while (current.Next != null)
                {
                    float distanceBetween = Vector3.Distance(previous, current.Value.position);
                    if (distanceBetween < a_distanceMoved)
                    {
                        a_distanceMoved -= distanceBetween;
                        previous = current.Value.position;
                        current = current.Next;
                    }
                    else
                    {
                        break;
                    }
                }

                while (current.Previous != null)
                {
                    _positionEvents.RemoveFirst();
                }

                position = Vector3.MoveTowards(previous, current.Value.position, a_distanceMoved);
            }

            return position;
        }
    }
}