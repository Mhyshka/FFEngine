using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace FF.Network
{
    internal class SynchronizedClientBallPosition : MonoBehaviour
    {
        #region Inspector Properties
        public float lerpSpeed = 5f;

        protected float _timeElapsed = 0f;
        protected int count = 0;
        protected double ecartType = 0f;
        #endregion

        #region Properties
        internal LinkedList<Vector3> _positionEvents = new LinkedList<Vector3>();
        #endregion

        #region Network
        internal Receiver.GenericReceiver<Message.MessagePongBallPosition> _receiver;

        void Awake()
        {
            _receiver = new Receiver.GenericReceiver<Message.MessagePongBallPosition>(OnMessageReceived);
            Engine.Receiver.RegisterReceiver(Message.EMessageType.PongBallPosition, _receiver);
        }

        void OnDestroy()
        {
            Engine.Receiver.UnregisterReceiver(Message.EMessageType.PongBallPosition, _receiver);
        }

        void OnMessageReceived(Message.MessagePongBallPosition a_message)
        {
            _positionEvents.AddLast(a_message.position);
            count++;
        }
        #endregion

        void Update()
        {
            _timeElapsed += Time.deltaTime;
            if (_timeElapsed > 1f)
            {
                _timeElapsed %= 1f;
                Debug.LogError("Messages received : " + count);
                count = 0;
            }

            float movementDistance = Mathf.Lerp(0f,
                                                TotalDistance,
                                                Time.deltaTime * lerpSpeed);
            Vector3 newPos = PositionForDistance(movementDistance); ;
            newPos.y = transform.position.y;
            transform.position = newPos;
            /*transform.position = Vector3.Lerp(transform.position,
                                                newPos,
                                                Time.deltaTime * lerpSpeed);*/
        }

        float TotalDistance
        {
            get
            {
                float distance = 0f;

                if (_positionEvents.Count > 0)
                {
                    Vector3 current = transform.position;
                    foreach (Vector3 each in _positionEvents)
                    {
                        distance += Vector3.Distance(each, current);
                        current = each;
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
                LinkedListNode<Vector3> current = _positionEvents.First;

                while (current.Next != null)
                {
                    float distanceBetween = Vector3.Distance(previous, current.Value);
                    if (distanceBetween < a_distanceMoved)
                    {
                        a_distanceMoved -= distanceBetween;
                        previous = current.Value;
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

                position = Vector3.MoveTowards(previous, current.Value, a_distanceMoved);
            }

            return position;
        }
    }
}