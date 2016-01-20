using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using FF;

namespace FF.Input
{	
	internal class NetworkInputManager : InputManager
	{
		#region Properties
        #endregion

        #region Manager
        internal NetworkInputManager(bool a_registerForBack = false) : base(a_registerForBack)
        {
            
        }

        internal override void TearDown()
        {
            base.TearDown();
        }
        #endregion

        protected override void SetupDefaultEvents()
        {
            RegisterInputEvent(new InputEventNetwork(EInputEventKey.Up));
            RegisterInputEvent(new InputEventNetwork(EInputEventKey.Down));
            RegisterInputEvent(new InputEventNetwork(EInputEventKey.Left));
            RegisterInputEvent(new InputEventNetwork(EInputEventKey.Right));
            RegisterInputEvent(new InputEventNetwork(EInputEventKey.Back));
            RegisterInputEvent(new InputEventNetwork(EInputEventKey.Submit));
            RegisterInputEvent(new InputEventNetwork(EInputEventKey.Action));
        }
    }
}