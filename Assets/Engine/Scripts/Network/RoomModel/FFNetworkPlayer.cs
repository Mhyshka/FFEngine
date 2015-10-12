using UnityEngine;
using System.Collections;
using System.Net;


namespace FF.Networking
{	
	internal class FFNetworkPlayer : IByteStreamSerialized
	{
        #region properties
        internal FFPlayer player = null;
		internal bool isHost = false;
		internal bool useTV = false;
        internal bool isDCed = false;

		internal FFSlot slot = null;

        internal int busyCount = 0;
        internal bool IsBusy
        {
            get
            {
                return busyCount > 0;
            }
        }
		
		internal FFSlotRef SlotRef
		{
			get
			{
				FFSlotRef slotRef = new FFSlotRef();
				slotRef.slotIndex = slot.slotIndex;
				slotRef.teamIndex = slot.team.teamIndex;
				return slotRef;
			}
		}

        protected IPEndPoint _ipEndPoint;
        internal IPEndPoint IpEndPoint
        {
            get
            {
                if (FFEngine.Network.IsServer && ID == FFEngine.Network.NetworkID)
                    return FFTcpServer.s_MockEP;
                else
                    return _ipEndPoint;
            }
            set
            {
                _ipEndPoint = value;
            }
        }

        internal int _playerID = -1;
        internal int ID
        {
            get
            {
                return _playerID;
            }
        }
        #endregion

        #region public methods
        public FFNetworkPlayer() : base()
		{
		
		}
		
		internal FFNetworkPlayer (int a_playerId, FFPlayer a_player)
		{
            _playerID = a_playerId;
			player = a_player;
		}

        internal void SetEP(IPEndPoint a_ep)
        {
            _ipEndPoint = a_ep;
        }
		#endregion
		
		#region Serialization
		public virtual void SerializeData(FFByteWriter stream)
		{
			stream.Write(player);
			stream.Write(isHost);
			stream.Write(useTV);
            stream.Write(isDCed);
            stream.Write(_playerID);
		}
		
		public virtual void LoadFromData(FFByteReader stream)
		{
			player = stream.TryReadObject<FFPlayer>();
			isHost = stream.TryReadBool();
			useTV = stream.TryReadBool();
            isDCed = stream.TryReadBool();
            _playerID = stream.TryReadInt();
        }
        #endregion

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is FFNetworkPlayer)
            {
                FFNetworkPlayer other = obj as FFNetworkPlayer;
                return other.ID.Equals(ID);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}
