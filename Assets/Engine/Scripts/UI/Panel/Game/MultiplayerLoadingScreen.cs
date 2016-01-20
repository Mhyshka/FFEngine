using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using FF.Network;
using FF.Multiplayer;
using FF.Logic;

namespace FF.UI
{
    internal class MultiplayerLoadingScreen : LoadingScreen
    {
        #region Inspector Properties
        public Text tipLabel = null;
        public GameObject loadingSlotPrefab = null;
        public GameObject widgetVerticalLayout = null;
        #endregion

        #region Properties
        protected Dictionary<int, FFLoadingSlotWidget> _slotWidgetsById = null;
        protected List<int> _loadingCompleteForPlayer = null;
        #endregion

        protected override void Awake()
        {
            base.Awake();
            _slotWidgetsById = new Dictionary<int, FFLoadingSlotWidget>();
            _loadingCompleteForPlayer = new List<int>();
        }

        internal override void Show(bool a_isForward = true)
        {
            ClearLoading();
            base.Show(a_isForward);
        }

        #region Row Management
        internal void PrepareView()
        {
            foreach (FFNetworkPlayer each in Engine.Game.CurrentRoom.players.Values)
            {
                GameObject newSlotGo = GameObject.Instantiate(loadingSlotPrefab);

                FFLoadingSlotWidget slotWidget = newSlotGo.GetComponent<FFLoadingSlotWidget>();
                if (slotWidget != null)
                {
                    if (each.isDced)
                        slotWidget.SetDCed(each);
                    else
                        slotWidget.SetLoading(each);

                    FFEventParameter args = new FFEventParameter();
                    args.data = each.ID;
                    slotWidget.kickButton.Data = args;

                    newSlotGo.transform.SetParent(widgetVerticalLayout.transform);
                    newSlotGo.transform.localPosition = Vector3.zero;
                    newSlotGo.transform.localScale = Vector3.one;
                    _slotWidgetsById.Add(each.ID, slotWidget);
                }
            }
        }

        internal void ClearLoading()
        {
            foreach (FFLoadingSlotWidget each in _slotWidgetsById.Values)
            {
                Destroy(each.gameObject);
            }
            _slotWidgetsById.Clear();
        }

        internal void RemovePlayer(int a_id)
        {
            FFLoadingSlotWidget target = SlotForId(a_id);
            _slotWidgetsById.Remove(target.Player.ID);
            if (target != null)
                Destroy(target.gameObject);
        }
        #endregion

        internal FFLoadingSlotWidget SlotForId(int a_networkId)
        {
            FFLoadingSlotWidget slot = null;
            _slotWidgetsById.TryGetValue(a_networkId, out slot);
            return slot;
        }

        internal void UpdateWithRoom(Room a_room, PlayerDictionary<PlayerLoadingWrapper> a_playersLoadingStates)
        {
            foreach (KeyValuePair<int, FFNetworkPlayer> keyVal in a_room.players)
            {
                FFLoadingSlotWidget slot = SlotForId(keyVal.Key);
                if (slot != null)
                {
                    PlayerLoadingWrapper wrapper = null;
                    if (a_playersLoadingStates.TryGetValue(keyVal.Key, out wrapper))
                    {
                        if (keyVal.Value.isDced)
                        {
                            slot.SetDCed(keyVal.Value);
                        }
                        else if (wrapper.state == ELoadingState.Ready)
                        {
                            slot.SetReady(keyVal.Value, wrapper.rank);
                        }
                        else if (wrapper.state == ELoadingState.NotReady)
                        {
                            slot.SetComplete(keyVal.Value, wrapper.rank);
                        }
                    }
                }
            }
        }

        internal void SetTip(string a_tip)
        {
            tipLabel.text = a_tip;
        }

        /// <summary>
        /// Call this when you'd like to preview the game in the background
        /// </summary>
        internal void SetLoadingComplete()
        {
            animator.SetTrigger("Complete");
        }
	}
}