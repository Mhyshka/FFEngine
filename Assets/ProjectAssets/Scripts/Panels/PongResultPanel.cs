using UnityEngine;

using System.Collections.Generic;

using FF.Multiplayer;
using FF.UI;


namespace FF.Pong
{
	internal class PongResultPanel : FFPanel
	{
        #region Inspector Properties
        public List<PlayerSlotWidget> leftSidePlayers = null;
        public List<PlayerSlotWidget> rightSidePlayers = null;
        public UILabel headerLabel = null;
        public PongSuccessWidget successWidget = null;
        #endregion

        #region Properties
        protected Queue<SuccessContent> _successQueue = new Queue<SuccessContent>();
        #endregion

        protected override void Awake()
        {
            base.Awake();
            successWidget.onSuccessDisplayComplete += OnSuccessDisplayComplete;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            successWidget.onSuccessDisplayComplete -= OnSuccessDisplayComplete;
        }

        internal void SetResult(ESide a_winningSide, Room a_room)
        {
            if (a_winningSide == ESide.Left)
            {
                headerLabel.text = "Blue team wins!";
            }
            else if (a_winningSide == ESide.Right)
            {
                headerLabel.text = "Purple team wins!";
            }


            //Blue team
            foreach (PlayerSlotWidget each in leftSidePlayers)
            {
                each.gameObject.SetActive(false);
            }
            for (int i = 0; i < a_room.Teams[GameConstants.BLUE_TEAM_INDEX].Players.Count; i++)
            {

                leftSidePlayers[i].gameObject.SetActive(true);
                leftSidePlayers[i].SetPlayer(a_room.Teams[GameConstants.BLUE_TEAM_INDEX].Players[i]);
            }


            //Purple team
            foreach (PlayerSlotWidget each in rightSidePlayers)
            {
                each.gameObject.SetActive(false);
            }
            for (int i = 0; i < a_room.Teams[GameConstants.PURPLE_TEAM_INDEX].Players.Count; i++)
            {

                rightSidePlayers[i].gameObject.SetActive(true);
                rightSidePlayers[i].SetPlayer(a_room.Teams[GameConstants.PURPLE_TEAM_INDEX].Players[i]);
            }

            successWidget.Prepare();
        }

        internal void SetSuccessQueue(Queue<SuccessContent> a_queue)
        {
            _successQueue = a_queue;
            DisplayNextSuccess();
        }

        protected void OnSuccessDisplayComplete()
        {
            DisplayNextSuccess();
        }

        protected void DisplayNextSuccess()
        {
            if (_successQueue.Count > 0)
            {
                successWidget.DisplaySuccess(_successQueue.Dequeue());
            }
        }
    }
}