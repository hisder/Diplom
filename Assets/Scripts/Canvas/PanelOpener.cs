using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    [SerializeField] private RectTransform _panelZone;

    public void Display()
    {
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (IsOutOfBounds() && Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
        }
    }

    private bool IsOutOfBounds()
    {
        Vector2 mousePosition;
        Vector2 rectMin;
        Vector2 rectMax;

        mousePosition = Input.mousePosition;

        rectMin = (Vector2)_panelZone.transform.position + _panelZone.rect.min;
        rectMax = (Vector2)_panelZone.transform.position + _panelZone.rect.max;

        return (mousePosition.x > rectMax.x ||
                mousePosition.x < rectMin.x ||
                mousePosition.y > rectMax.y ||
                mousePosition.y < rectMin.y);
    }
}
