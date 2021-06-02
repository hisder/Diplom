using UnityEngine;

public class RoomDisplay : MonoBehaviour
{
    [SerializeField] private DustRoomView _roomView;

    [SerializeField] private RectTransform _displayRect;

    void Update()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(_displayRect, Input.mousePosition))
        {
            _roomView.DoUpdate(DisplayPosition());
        }
    }

    private Vector2 DisplayPosition()
    {
        Vector2 position = Vector2.zero;
        Vector2 normolizedPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_displayRect, Input.mousePosition, null, out position);

        normolizedPosition = position / _displayRect.rect.size;

        return position;
    }
}
