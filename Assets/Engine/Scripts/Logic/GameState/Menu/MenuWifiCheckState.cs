using UnityEngine;
using System.Collections;

namespace FF
{
	internal class MenuWifiCheckState : ANavigationMenuState
	{
		internal override int ID
        {
            get
            {
                return (int)EMenuStateID.WifiCheck;
            }
        }

        internal override void Enter()
        {
            base.Enter();
            FFLog.Log(EDbgCat.Logic, "Menu wifi Check state enter.");

            if (Engine.Inputs.ShouldUseNavigation)
            {
                _navigationPanel.FocusBackButton();
            }
        }

        #region Events
        protected override void RegisterForEvent ()
		{
			base.RegisterForEvent ();
			Engine.NetworkStatus.onLanStatusChanged += OnNetworkStateChanged;
        }
		
		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
			Engine.NetworkStatus.onLanStatusChanged -= OnNetworkStateChanged;
        }
		
		internal void OnNetworkStateChanged(bool a_status)
		{
			if(a_status)
				RequestState(outState.ID);
		}
        #endregion
    }
}