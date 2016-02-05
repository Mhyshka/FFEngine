using UnityEngine;
using System.Collections;

namespace FF.Network.Message
{
	internal class ResponseSuccess : MessageData
	{
        #region Properties
        internal override EDataType Type
        {
            get
            {
                return EDataType.R_Success;
            }
        }
        #endregion
	}
}