using UnityEngine;
using System.Collections;
using System.Net;

using FF.Network;

namespace FF.Multiplayer
{	
	internal class FFNetworkPlayer : IByteStreamSerialized
	{
        #region properties
        internal FFPlayer player = null;
		internal bool isHost = false;
		internal bool useTV = false;
        internal bool isDced = false;

		internal Slot slot = null;

        internal int busyCount = 0;
        internal bool IsBusy
        {
            get
            {
                return busyCount > 0;
            }
        }
		
		internal SlotRef SlotRef
		{
			get
			{
				SlotRef slotRef = new SlotRef();
				slotRef.slotIndex = slot.slotIndex;
				slotRef.teamIndex = slot.team.teamIndex;
				return slotRef;
			}
		}

        protected int _playerID = -1;
        internal int ID
        {
            get
            {
                return _playerID;
            }
            set
            {
                _playerID = value;
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
		#endregion
		
		#region Serialization
		public virtual void SerializeData(FFByteWriter stream)
		{
			stream.Write(player);
			stream.Write(isHost);
			stream.Write(useTV);
            stream.Write(isDced);
            stream.Write(_playerID);
		}
		
		public virtual void LoadFromData(FFByteReader stream)
		{
			player = stream.TryReadObject<FFPlayer>();
			isHost = stream.TryReadBool();
			useTV = stream.TryReadBool();
            isDced = stream.TryReadBool();
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
