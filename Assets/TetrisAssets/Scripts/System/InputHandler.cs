using UnityEngine;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    public UnityEvent OnSwipeLeft;
    public UnityEvent OnSwipeRight;
    public UnityEvent OnSwipeDown;
    public UnityEvent OnQuickSwipeDown;
    public UnityEvent OnClick;
    public UnityEvent OnRelease;

    [SerializeField] private float clickTime;
    [SerializeField] private float holdDuration;
    [SerializeField] private float swipeDownTime;
    [SerializeField] private float swipeDownDuration;
    [SerializeField] private bool isMouseHeld;
    [SerializeField] private bool isSwipedDown;
    private Vector3 _touchPos;

    private float HoldThreshold => TetrisGameplayConfig.HoldThreshold;
    private float HorizontalSwipeThreshold => TetrisGameplayConfig.HorizontalSwipeThreshold;
    private float SwipeDownThreshold => TetrisGameplayConfig.SwipeDownThreshold;
    private float VerticalSwipeThreshold => TetrisGameplayConfig.VerticalSwipeThreshold;
    private TetrisGameplayConfig TetrisGameplayConfig => TetrisGameplayConfig.Instance;

    private void Update()
    {
        CheckReleaseMouse();
        CheckHorizontalSwipe();
        CheckMouseClick();
        CheckHoldMouse();
        CheckVerticalSwipe();
    }

    private void CheckMouseClick()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        _touchPos = GetMousePos();
        holdDuration = 0;
        clickTime = Time.time;
    }

    private void CheckHoldMouse()
    {
        if (!Input.GetMouseButton(0)) return;

        holdDuration = Time.time - clickTime;

        if (holdDuration < HoldThreshold) return;

        Debug.Log("(INPUT) Holding mouse...");
        isMouseHeld = true;
    }

    private void CheckReleaseMouse()
    {
        if (!Input.GetMouseButtonUp(0)) return;

        OnRelease?.Invoke();

        if (isMouseHeld)
        {
            isMouseHeld = false;
            CheckQuickSwipeDown();
            return;
        }

        Debug.Log("(INPUT) Clicking mouse...");
        OnClick?.Invoke();
    }

    private void CheckHorizontalSwipe()
    {
        if (!isMouseHeld) return;

        var mousePos = GetMousePos();
        var swipeDistance = Mathf.Abs(mousePos.x - _touchPos.x);

        if (swipeDistance < HorizontalSwipeThreshold) return;

        var swipeDirection = Mathf.Sign(_touchPos.x - mousePos.x);

        if (swipeDirection < 0)
            SwipeRight();
        else if (swipeDirection > 0) SwipeLeft();
    }

    private void CheckVerticalSwipe()
    {
        if (!isMouseHeld) return;

        var mousePos = GetMousePos();
        var swipeDistance = Mathf.Abs(mousePos.y - _touchPos.y);

        if (swipeDistance < VerticalSwipeThreshold) return;

        var swipeDirection = Mathf.Sign(_touchPos.y - mousePos.y);

        if (swipeDirection <= 0) return;

        SwipeDown();
    }

    private void CheckQuickSwipeDown()
    {
        if (!isSwipedDown) return;
        isSwipedDown = false;

        swipeDownDuration = Time.time - swipeDownTime;

        if (swipeDownDuration > SwipeDownThreshold) return;

        OnQuickSwipeDown?.Invoke();
        Debug.Log("(INPUT) Quick Swipe Down");
    }

    private void SwipeDown()
    {
        Debug.Log("(INPUT) Swipe Down");
        swipeDownTime = Time.time;
        isSwipedDown = true;
        OnSwipeDown?.Invoke();
    }

    private void SwipeLeft()
    {
        _touchPos = GetMousePos();
        Debug.Log("(INPUT) Swipe left");
        OnSwipeLeft?.Invoke();
    }

    private void SwipeRight()
    {
        _touchPos = GetMousePos();
        Debug.Log("(INPUT) Swipe right");
        OnSwipeRight?.Invoke();
    }

    private Vector3 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}