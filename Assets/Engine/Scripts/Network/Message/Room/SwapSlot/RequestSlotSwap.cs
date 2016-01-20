using UnityEngine;
using System.Collections;
using System;

using FF.Multiplayer;

namespace FF.Network.Message
{
    internal class RequestSlotSwap : ARequest
    {
        internal enum EErrorCode
        {
            PlayerNotFound,
            TargetRefused,
            TargetIsBusy
        }

        #region Properties
        internal SlotRef targetSlot;

        internal override EMessageType Type
        {
            get
            {
                return EMessageType.SlotSwapRequest;
            }
        }

        internal override bool HandleByMock
        {
            get
            {
                return true;
            }
        }
        #endregion

        #region Constructors
        public RequestSlotSwap()
        {
        }

        internal RequestSlotSwap(SlotRef a_target)
        {
            targetSlot = a_target;
        }
        #endregion

        protected override float TimeoutDuration
        {
            get
            {
                return float.MaxValue;
            }
        }

        #region Serialization
        public override void LoadFromData(FFByteReader stream)
        {
            base.LoadFromData(stream);
            targetSlot = stream.TryReadObject<SlotRef>();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            base.SerializeData(stream);
            stream.Write(targetSlot);
        }
        #endregion


        internal static string MessageForCode(int a_errorCode)
        {
            string errorMessage = "";

            errorMessage = ARequest.MessageForErrorCode(a_errorCode);
            if (string.IsNullOrEmpty(errorMessage))
            {
                EErrorCode errorCode = (EErrorCode)a_errorCode;

                switch (errorCode)
                {
                    case EErrorCode.PlayerNotFound:
                        errorMessage = "Player not found.";
                        break;

                    case EErrorCode.TargetRefused:
                        errorMessage = "Target refused.";
                        break;

                    case EErrorCode.TargetIsBusy:
                        errorMessage = "Target is busy.";
                        break;
                }
            }
            return errorMessage;
        }
    }
}