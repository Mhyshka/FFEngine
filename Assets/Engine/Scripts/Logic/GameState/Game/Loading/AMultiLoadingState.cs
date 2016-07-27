using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.UI;
using FF.Multiplayer;
using FF.Network.Message;

namespace FF.Logic
{
    internal abstract class AMultiLoadingState : LoadingState
    {
        #region Properties
        protected MultiplayerLoadingScreen _loadingScreen;

        protected ManualLoadingStep _unlockStep;
        #endregion

        #region State Methods
        internal override void Enter()
        {
            base.Enter();

            _loadingScreen = Engine.UI.LoadingScreen as MultiplayerLoadingScreen;
            _loadingScreen.PrepareView();

            OnRoomUpdate(Engine.Network.CurrentRoom);
        }
        #endregion

        #region Events
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            Engine.Network.CurrentRoom.onRoomUpdated += OnRoomUpdate;
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            if(Engine.Network.CurrentRoom != null)
                Engine.Network.CurrentRoom.onRoomUpdated -= OnRoomUpdate;
        }

        protected void OnEveryoneReady(ReadMessage a_message)
        {
            RequestState(outState.ID);
        }

        protected void OnRoomUpdate(Room a_room)
        {
            _loadingScreen.UpdateWithRoom(a_room, Engine.Game.Loading.PlayersLoadingState);
        }
        #endregion


        #region LoadingStep
        protected override void SetupLoadingSteps()
        {
            base.SetupLoadingSteps();
            _unlockStep = new ManualLoadingStep("Swipe to unlock", 0f);
            _loadingSteps.Add(_unlockStep);
        }

        protected override void OnLoadingComplete()
        {
            base.OnLoadingComplete();
            _loadingScreen.SetLoadingComplete();
        }
        #endregion
    }
}