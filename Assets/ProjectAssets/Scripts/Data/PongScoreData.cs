using UnityEngine;
using System.Collections;


namespace FF.Pong
{
    internal enum ESide
    {
        None = -1,
        Left = 0,
        Right = 1,
    }

    internal delegate void SideCallback(ESide a_side);
    internal class PongMatchData
    {
        #region Properties
        protected int _requiredPointsToWin;
        internal int RequiredPointsToWin
        {
            get
            {
                return _requiredPointsToWin;
            }
        }

        protected PongRoundData[] _rounds;
        internal PongRoundData[] Rounds
        {
            get
            {
                return _rounds;
            }
        }

        internal int LeftScore
        {
            get
            {
                int count = 0;
                for (int i = 0; i < MaxRoundCount; i++)
                {
                    if (_rounds[i].goalSide == ESide.Right)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        internal int RightScore
        {
            get
            {
                int count = 0;
                for (int i = 0; i < MaxRoundCount; i++)
                {
                    if (_rounds[i].goalSide == ESide.Left)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        internal int RoundPlayedCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < MaxRoundCount; i++)
                {
                    if (_rounds[i].IsComplete)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
                return count;
            }
        }

        internal int HighestScore
        {
            get
            {
                return Mathf.Max(LeftScore, RightScore);
            }
        }

        internal ESide WinningSide
        {
            get
            {
                if (LeftScore > RightScore)
                    return ESide.Left;
                else if (RightScore > LeftScore)
                    return ESide.Right;
                else
                    return ESide.None;
            }
        }
        #endregion

        internal int MaxRoundCount
        {
            get
            {
                return _requiredPointsToWin * 2 - 1;
            }
        }

        internal PongMatchData(int a_requiredPointsToWin)
        {
            _requiredPointsToWin = a_requiredPointsToWin;
            _rounds = new PongRoundData[MaxRoundCount];
            for (int i = 0; i < MaxRoundCount; i++)
            {
                _rounds[i] = new PongRoundData();
            }
        }
    }

    internal class PongRoundData
    {
        #region Properties
        internal int strikerId = -1;
        internal int rallyCount = 0;

        internal int smashCount = 0;
        internal bool isSmash = false;

        internal ESide goalSide = ESide.None;

        internal bool IsComplete
        {
            get
            {
                return goalSide != ESide.None;
            }
        }
        #endregion
    }
}