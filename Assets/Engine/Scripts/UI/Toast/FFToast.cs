using UnityEngine;
using System.Collections;

namespace FF.UI
{
    internal class FFToastData
    {
        internal string toastName = null;

        internal FFToastData()
        {
        }
    }

    internal abstract class FFToast : FFPanel
    {
        internal FFToastData currentData;

        protected abstract float Duration
        {
            get;
        }
        protected float _timeElapsed;

        internal virtual void SetContent(FFToastData a_data)
        {
            currentData = a_data;
            _timeElapsed = 0f;
        }

        protected virtual void Update()
        {
            if (gameObject.activeSelf && currentData != null)
            {
                if (_state == EState.Showing || _state == EState.Shown)
                {
                    _timeElapsed += Time.deltaTime;
                    if (_timeElapsed > Duration)
                    {
                        Hide(false);
                    }
                }
            }
        }
  
    }
}