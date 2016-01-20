using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FocusedInputField : InputField
{
    public override void OnSelect(BaseEventData eventData)
    {
        //base.OnSelect(eventData);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnSelect(eventData);
    }
}
