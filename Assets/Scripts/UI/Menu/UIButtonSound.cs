using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public string hoverSoundName = "Hover";
    public string clickSoundName = "Click";

    public void OnPointerEnter(PointerEventData eventData)
    {
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.PlaySound2D(hoverSoundName);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.PlaySound2D(clickSoundName);
		}
	}
}
