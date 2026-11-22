using UnityEngine;
using UnityEngine.EventSystems;

public class BusCameraRig : MonoBehaviour
{
    public Transform target;
    public Camera cameraComponent;
    public float distance = 10f;
    public float sensitivity = 0.18f;
    public float height = 3.2f;

    int mode;
    float yaw = 0f;
    float pitch = 12f;
    Vector2 previousTouch;
    bool dragging;

    void LateUpdate()
    {
        if (target == null) return;
        if (Input.GetKeyDown(KeyCode.C)) SwitchCamera();
        ReadLookInput();
        UpdateCameraPose();
    }

    void ReadLookInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    previousTouch = touch.position;
                    dragging = true;
                }
            }
            else if (touch.phase == TouchPhase.Moved && dragging)
            {
                ApplyLookDelta(touch.position - previousTouch);
                previousTouch = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                dragging = false;
            }
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            bool overUi = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
            if (!overUi)
            {
                previousTouch = Input.mousePosition;
                dragging = true;
            }
        }
        if (Input.GetMouseButtonUp(0)) dragging = false;
        if (dragging && Input.GetMouseButton(0))
        {
            Vector2 now = Input.mousePosition;
            ApplyLookDelta(now - previousTouch);
            previousTouch = now;
        }
    }

    void ApplyLookDelta(Vector2 delta)
    {
        yaw += delta.x * sensitivity;
        pitch = Mathf.Clamp(pitch - delta.y * sensitivity, -12f, 58f);
    }

    void UpdateCameraPose()
    {
        if (mode == 0)
        {
            Vector3 focus = target.position + Vector3.up * 2f;
            Quaternion orbit = Quaternion.Euler(pitch, yaw, 0);
            transform.position = focus + orbit * new Vector3(0, 0, -distance);
            transform.LookAt(focus);
        }
        else if (mode == 1)
        {
            Vector3 localFocus = new Vector3(0f, 2.25f, -0.25f);
            Vector3 focus = target.TransformPoint(localFocus);
            Quaternion orbit = target.rotation * Quaternion.Euler(pitch, yaw, 0);
            transform.position = focus + orbit * new Vector3(0, 0, -2.6f);
            transform.LookAt(focus);
        }
        else
        {
            transform.position = target.TransformPoint(new Vector3(-0.55f, 2.55f, 2.2f));
            transform.rotation = target.rotation * Quaternion.Euler(pitch * 0.45f, yaw - 180f, 0);
        }
    }

    public void SwitchCamera()
    {
        mode = (mode + 1) % 3;
        if (mode == 0) { yaw = 0f; pitch = 12f; }
        else if (mode == 1) { yaw = 0f; pitch = 8f; }
        else { yaw = 180f; pitch = 0f; }
        dragging = false;
    }
}
