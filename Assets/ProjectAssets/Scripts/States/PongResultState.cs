using UnityEngine;
using System.Collections.Generic;
using System;

namespace FF.Pong
{
    internal class PongResultState : APongServerState
    {
        #region Properties
        protected PongResultPanel _resultpanel;

        internal override int ID
        {
            get
            {
                return (int)EPongGameState.Result;
            }
        }
        #endregion

        #region State Methods
        internal override void Enter()
        {
            base.Enter();

            _resultpanel = Engine.UI.GetPanel("ResultPanel") as PongResultPanel;

            DisplayResult();
        }
        #endregion

        #region EventManagement
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            Engine.Events.RegisterForEvent(FFEventType.BackToMenu, OnBackToMenuPressed);
            Engine.Events.RegisterForEvent(FFEventType.PlayAgain, OnPlayAgainPressed);
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            Engine.Events.UnregisterForEvent(FFEventType.BackToMenu, OnBackToMenuPressed);
            Engine.Events.UnregisterForEvent(FFEventType.PlayAgain, OnPlayAgainPressed);
        }

        protected void OnBackToMenuPressed(FFEventParameter a_args)
        {
            RequestState((int)EStateID.Exit);
        }

        protected void OnPlayAgainPressed(FFEventParameter a_args)
        {
            _pongServerGm.NewMatch();
            _pongServerGm.ResetRackets();
            _pongServerGm.EnableGameplay();
            RequestState((int)EPongGameState.Service);
        }
        #endregion

        protected void DisplayResult()
        {
            ESide side = _pongServerGm.Score.WinningSide;

            _resultpanel.SetResult(_pongServerGm.Score.WinningSide,
                                    Engine.Game.CurrentRoom);

            Queue<SuccessContent> successQueue = new Queue<SuccessContent>();

            SuccessContent content = new SuccessContent("Winner", "Mhyshka", "1", side);
            successQueue.Enqueue(content);

            content = new SuccessContent("Winner", "Raphi", "2", side);
            successQueue.Enqueue(content);

            content = new SuccessContent("Winner", "Gaet", "3", side);
            successQueue.Enqueue(content);

            _resultpanel.SetSuccessQueue(successQueue);
        }
    }
}