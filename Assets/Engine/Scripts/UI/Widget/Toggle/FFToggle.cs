using UnityEngine;
using System.Collections;

namespace FF.UI
{
    internal abstract class FFToggle : MonoBehaviour
    {
        #region Inspector Properties
        //public bool startsSelected = false;
        #endregion

        #region Properties
        protected bool _isSelected = false;
        internal bool IsSelected
        {
            get
            {
                return _isSelected;
            }
        }
        #endregion

        protected virtual void Awake()
        {
            //SetSelected(startsSelected, true);
        }

        #region Interface
        internal void SetSelected(bool a_state, bool a_forceAnim = false)
        {
            if (a_state && !_isSelected)
                Select(a_forceAnim);
            else if (_isSelected)
                Deselect(a_forceAnim);
        }

        internal void Toggle()
        {
            SetSelected(!_isSelected);
        }

        internal void Select(bool a_forceAnim)
        {
            if (!_isSelected || a_forceAnim)
                PlaySelectTransition();

            _isSelected = true;
        }

        internal void Deselect(bool a_forceAnim)
        {
            if (_isSelected || a_forceAnim)
                PlayDeselectTransition();

            _isSelected = false;
        }
        #endregion

        protected abstract void PlaySelectTransition();



        protected abstract void PlayDeselectTransition();
    }
}