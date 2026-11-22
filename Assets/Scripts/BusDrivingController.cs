using UnityEngine;

public class BusDrivingController : MonoBehaviour
{
    [Header("Driving")]
    public float acceleration = 7f;
    public float maxForwardSpeed = 18f;
    public float maxReverseSpeed = 6f;
    public float braking = 14f;
    public float steeringRate = 48f;
    public float wheelRadius = 0.42f;

    [HideInInspector] public float throttleInput;
    [HideInInspector] public float brakeInput;
    [HideInInspector] public float steeringInput;
    [HideInInspector] public bool reverseSelected;
    [HideInInspector] public bool handbrake;

    float speed;
    BusLightingController lighting;
    Transform steeringWheel;
    Transform[] wheels;
    Light[] headlights;
    BusDoor door;

    public float SpeedKph => speed * 3.6f;

    public void Configure()
    {
        steeringWheel = transform.Find("Steering wheel");
        wheels = GetComponentsInChildren<Transform>();
        headlights = GetComponentsInChildren<Light>();
        door = GetComponentInChildren<BusDoor>();
        lighting = GetComponent<BusLightingController>();
    }

    void Update()
    {
        ReadKeyboard();
        float direction = reverseSelected ? -1f : 1f;
        if (throttleInput > 0.01f)
            speed += acceleration * throttleInput * Time.deltaTime * direction;
        else
            speed = Mathf.MoveTowards(speed, 0, (brakeInput * braking + (handbrake ? braking * 2 : 1.4f)) * Time.deltaTime);

        speed = Mathf.Clamp(speed, -maxReverseSpeed, maxForwardSpeed);
        if (brakeInput > 0.01f) speed = Mathf.MoveTowards(speed, 0, braking * brakeInput * Time.deltaTime);

        float steer = steeringInput * steeringRate * Mathf.Clamp01(Mathf.Abs(speed) / 4f);
        transform.Rotate(0, steer * Time.deltaTime * Mathf.Sign(Mathf.Abs(speed) < 0.1f ? 1 : speed), 0);
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

        if (steeringWheel != null)
            steeringWheel.localRotation = Quaternion.Euler(90, 0, -steeringInput * 420f);

        foreach (var t in wheels)
        {
            if (t.name == "Road wheel")
                t.Rotate(Vector3.up, speed / Mathf.Max(0.1f, wheelRadius) * Mathf.Rad2Deg * Time.deltaTime, Space.Self);
        }
    }

    void ReadKeyboard()
    {
        if (Application.isMobilePlatform) return;
        throttleInput = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) ? 1 : 0;
        brakeInput = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) ? 1 : 0;
        steeringInput = (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) ? 1 : 0)
                      - (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
        reverseSelected = Input.GetKey(KeyCode.R);
        handbrake = Input.GetKey(KeyCode.Space);
        if (Input.GetKeyDown(KeyCode.E)) ToggleDoor();
        if (Input.GetKeyDown(KeyCode.L)) ToggleLights();
    }

    public void SetThrottle(float value) => throttleInput = Mathf.Clamp01(value);
    public void SetBrake(float value) => brakeInput = Mathf.Clamp01(value);
    public void SetSteering(float value) => steeringInput = Mathf.Clamp(value, -1, 1);
    public void SetReverse(bool value) => reverseSelected = value;
    public void SetHandbrake(bool value) => handbrake = value;

    public void ToggleDoor()
    {
        if (door != null) door.Toggle();
    }

    public void ToggleLights()
    {
        if (lighting != null) lighting.ToggleHeadlights();
        else
        {
            foreach (var light in headlights) light.enabled = !light.enabled;
        }
    }

    public void SetIndicator(int mode)
    {
        if (lighting != null) lighting.SetIndicator(mode);
    }
}
