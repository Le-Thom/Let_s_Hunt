using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderVideo : Slider
{
    public SetVideo video;
    protected override void Start()
    {
        video = FindObjectOfType<SetVideo>();
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        video.onMouseOver = true;
    }
    public override void OnPointerExit(PointerEventData eventData)
    {  
        base.OnPointerExit(eventData);

        video.onMouseOver = false;
    }
}
