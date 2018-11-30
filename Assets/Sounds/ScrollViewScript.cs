using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollViewScript : MonoBehaviour, IBeginDragHandler {

    public void PlayScrollSound()
    {
        SoundManager.current.PlayScrollingSound();
    }

    public void OnBeginDrag(PointerEventData data)
    {
        PlayScrollSound();
    }
}
