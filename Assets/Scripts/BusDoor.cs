using UnityEngine;

public class BusDoor : MonoBehaviour
{
    public float openAngle = 78f;
    public float animationSpeed = 3f;
    bool isOpen;
    Quaternion closedRotation;

    void Awake() { closedRotation = transform.localRotation; }

    public void Toggle() { isOpen = !isOpen; }

    void Update()
    {
        Quaternion target = closedRotation * Quaternion.Euler(0, isOpen ? openAngle : 0, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * animationSpeed);
    }
}
