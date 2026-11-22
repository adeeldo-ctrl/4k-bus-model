using UnityEngine;

public class BusCameraRig : MonoBehaviour
{
    public Transform target;
    public Camera cameraComponent;
    public float distance = 10f;
    public float sensitivity = 0.18f;
    public float height = 3.2f;

    int mode;
    float yaw = 180f;
    float pitch = 12f;
    Vector2 previousTouch;
    bool dragging;

    void LateUpdate()
    {
        if (target == null) return;
        if (Input.GetKeyDown(KeyCode.C)) SwitchCamera();
        if (Input.GetMouseButtonDown(0)) { previousTouch = Input.mousePosition; dragging = true; }
        if (Input.GetMouseButtonUp(0)) dragging = false;
        if (dragging && Input.GetMouseButton(0))
        {
            Vector2 now = Input.mousePosition;
            Vector2 delta = now - previousTouch;
            yaw += delta.x * sensitivity;
            pitch = Mathf.Clamp(pitch - delta.y * sensitivity, -5, 55);
            previousTouch = now;
        }

        Vector3 focus = target.position + Vector3.up * 2f;
        if (mode == 0)
        {
            Quaternion orbit = Quaternion.Euler(pitch, yaw, 0);
            transform.position = focus + orbit * new Vector3(0, 0, -distance);
            transform.LookAt(focus);
        }
        else if (mode == 1)
        {
            transform.position = target.TransformPoint(new Vector3(0, 2.65f, 1.55f));
            transform.rotation = target.rotation * Quaternion.Euler(4, 180, 0);
        }
        else
        {
            transform.position = target.TransformPoint(new Vector3(-0.55f, 2.55f, 2.2f));
            transform.rotation = target.rotation * Quaternion.Euler(0, 180, 0);
        }
    }

    public void SwitchCamera() { mode = (mode + 1) % 3; }
}
