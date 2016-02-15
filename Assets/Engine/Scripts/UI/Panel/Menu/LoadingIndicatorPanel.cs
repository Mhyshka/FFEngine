using UnityEngine;
using System.Collections;

namespace FF.UI
{
    internal class LoadingIndicatorPanel : FFPanel
    {
        public UILabel descriptionLabel = null;

        internal void SetDescription(string a_text)
        {
            descriptionLabel.text = a_text;
        }
    }
}
