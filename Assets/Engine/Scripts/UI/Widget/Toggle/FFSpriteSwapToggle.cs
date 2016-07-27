using UnityEngine;
using System.Collections;
using FF.UI;

internal class FFSpriteSwapToggle : FFToggle
{
    #region Inspector Properties
    public UISprite selectedSprite = null;
    public UISprite unselectedSprite = null;
    #endregion

    protected override void PlaySelectTransition()
    {
        selectedSprite.gameObject.SetActive(true);
        unselectedSprite.gameObject.SetActive(false);
    }

    protected override void PlayDeselectTransition()
    {
        selectedSprite.gameObject.SetActive(false);
        unselectedSprite.gameObject.SetActive(true);
    }
}