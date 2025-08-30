using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoulInformation : MonoBehaviour, ISelectHandler
{
    [SerializeField] private Image MainImage;
    [SerializeField] private Button SoulButton;

    [SerializeField] private ScrollRect scrollRect;
    [HideInInspector] public SoulItem soulItem;

    public void SetSoulItem(SoulItem _soulItem, Action OnSoulClick = null)
    {
        soulItem = _soulItem;
        MainImage.sprite = soulItem.Avatar;
        if (OnSoulClick != null)
        {
            SoulButton.onClick.AddListener(() => OnSoulClick());
        }
    }


    public void OnSelect(BaseEventData eventData)
    {
        RectTransform selected = transform as RectTransform;
        RectTransform viewport = scrollRect.viewport;
        
        Vector3[] itemCorners = new Vector3[4];
        Vector3[] viewportCorners = new Vector3[4];
        selected.GetWorldCorners(itemCorners);
        viewport.GetWorldCorners(viewportCorners);

        float itemTop = itemCorners[1].y;
        float itemBottom = itemCorners[0].y;
        float viewportTop = viewportCorners[1].y;
        float viewportBottom = viewportCorners[0].y;
        
        Vector2 newPos = scrollRect.content.anchoredPosition;
        
        if (itemTop > viewportTop)
        {
            float delta = itemTop - viewportTop;
            newPos.y -= delta;
        }
        else if (itemBottom < viewportBottom)
        {
            float delta = viewportBottom - itemBottom;
            newPos.y += delta;
        }

        scrollRect.content.anchoredPosition = newPos;
    }
}