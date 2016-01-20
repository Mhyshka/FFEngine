using UnityEngine;
using System.Collections;

using FF.Multiplayer;

namespace FF.Network.Message
{
	internal class RequestMoveToSlot : ARequest
	{
        internal enum EErrorCode
        {
            PlayerNotfound,
            PlayerDisconnected,
            SlotIsUsed
        }
		#region Properties
		internal SlotRef slotRef;
		
		internal override EMessageType Type
		{
			get
			{
				return EMessageType.MoveToSlotRequest;
			}
		}
        #endregion

        #region Constructors
        public RequestMoveToSlot()
		{
		
		}
		
		internal RequestMoveToSlot(SlotRef a_slotRef)
		{
			slotRef = a_slotRef;
		}
        #endregion

        #region Serialization
        public override void SerializeData (FFByteWriter stream)
		{
			base.SerializeData (stream);
			stream.Write(slotRef);
		}
		
		public override void LoadFromData (FFByteReader stream)
		{
			base.LoadFromData (stream);
			slotRef = stream.TryReadObject<SlotRef>();
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
                    case EErrorCode.SlotIsUsed:
                        errorMessage = "Slot is used.";
                        break;

                    case EErrorCode.PlayerNotfound:
                        errorMessage = "Player not found.";
                        break;
                }
            }
            return errorMessage;
        }
	}
}