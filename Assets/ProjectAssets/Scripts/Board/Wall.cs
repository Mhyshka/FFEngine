using UnityEngine;
using System.Collections;

namespace FF.Pong
{
    internal class Wall : MonoBehaviour, IBallContact
    {
        protected BallCollider _ballCollider = null;
        protected int _lastRacketHitId = -1;

        public Vector3 BounceOff(Vector3 a_position, Vector3 a_velocity)
        {
            Vector3 result = Vector3.Reflect(a_velocity, transform.forward);

            return result;
        }

        public void OnCollision(ServerBall a_ball)
        {
        }

        void OnTriggerEnter(Collider a_collider)
        {
            if (_ballCollider == null)
            {
                _ballCollider = a_collider.GetComponent<BallCollider>();
            }

            if (_ballCollider != null)
            {
                if (a_collider == _ballCollider.modelCollider)
                {
                    _lastRacketHitId = _ballCollider.ball.LastLocalHitInfo.playerId;
                }
            }
        }

        void OnTriggerStay(Collider a_collider)
        {
            if (_ballCollider != null && a_collider == _ballCollider)
            {
                if (_lastRacketHitId != _ballCollider.ball.LastLocalHitInfo.playerId)
                {
                    _lastRacketHitId = _ballCollider.ball.LastLocalHitInfo.playerId;
                    _ballCollider.ball.OnTriggerEnter(GetComponent<Collider>());

                    if (_ballCollider.ball.LastLocalHitInfo.playerId == Engine.Network.NetPlayer.ID)
                    {
                        _ballCollider.ball.ForceNetworkMovementSync();
                    }
                }
            }
        }
    }
}