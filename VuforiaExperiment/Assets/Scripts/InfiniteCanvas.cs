using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class InfiniteCanvas : MonoBehaviour
{
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    private void Update()
    {
        if (!_canvas.enabled)
            _canvas.enabled = true;
    }
}
