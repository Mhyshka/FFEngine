using UnityEngine;

using FF.UI;

namespace FF.Pong
{
	internal class PongScorePanel : FFPanel
	{
        #region Inspector Properties
        public UILabel playerNameLabel = null;
        public FFRatingWidget goalScored = null;

        public Color blueTeamNameColor = Color.blue;
        public Color purpleTeamNameColor = Color.green;

        public UILabel commentLabel = null;
        #endregion

        #region Properties
        internal void SetScore(string a_playerName, ESide a_playerSide, int a_goalCount)
        {
            playerNameLabel.text = a_playerName;
            goalScored.curValue = a_goalCount;

            if (a_playerSide == ESide.Left)
            {
                playerNameLabel.color = blueTeamNameColor;
            }
            else if (a_playerSide == ESide.Right)
            {
                playerNameLabel.color = purpleTeamNameColor;
            }
        }

        internal void DisplayDefaultComment()
        {
            commentLabel.text = "scored a goal!";
        }
        #endregion

    }
}