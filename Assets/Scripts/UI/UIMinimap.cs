using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMinimap : MonoBehaviour
{
    [SerializeField]
    private Image map;
    [SerializeField, Range(0.01f, 10.0f)]
    private float zoom = 1.0f;

    void Start()
    {
        
    }

    void Update()
    {
        map.rectTransform.anchoredPosition = new Vector2(
            (-DungeonManager.Instance.plPos.x) * map.rectTransform.rect.width * zoom, 
            (-DungeonManager.Instance.plPos.y) * map.rectTransform.rect.width * zoom);

        map.rectTransform.localScale = new Vector2(zoom, zoom);
    }
}
