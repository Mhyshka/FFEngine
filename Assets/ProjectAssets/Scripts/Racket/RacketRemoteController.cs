using UnityEngine;
using System.Collections;
using System;

namespace FF.Pong
{
    internal class RacketRemoteController : ARacketComponent
    {
        #region Properties
        public float speed = 2f;
        #endregion

        #region Init & Destroy
        internal override void Activate()
        {
            Engine.Inputs.EventForKey(EInputEventKey.Action).onDown += OnActionPressed;
        }

        internal override void TearDown()
        {
            Engine.Inputs.EventForKey(EInputEventKey.Action).onDown -= OnActionPressed;
        }
        #endregion

        #region Callbacks
        protected void OnActionPressed()
        {
            motor.TrySmash();
        }
        #endregion

        void Update()
        {
            if (Engine.Inputs.EventForKey(EInputEventKey.Down).IsPressed)
            {
                motor.TargetRatio = Mathf.Clamp01(motor.TargetRatio + speed * Time.deltaTime);
            }
            if (Engine.Inputs.EventForKey(EInputEventKey.Up).IsPressed)
            {
                motor.TargetRatio = Mathf.Clamp01(motor.TargetRatio - speed * Time.deltaTime);
            }
        }
    }
}