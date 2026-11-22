using UnityEngine;

public class BusLightingController : MonoBehaviour
{
    public BusDrivingController driving;
    public float blinkInterval = 0.42f;

    Light[] headlamps;
    Renderer[] brakeLamps;
    Renderer[] leftIndicators;
    Renderer[] rightIndicators;
    Material brakeOn, brakeOff, amberOn, amberOff;
    int indicatorMode; // 0 off, 1 left, 2 right, 3 hazards
    bool headlightsOn;
    float blinkClock;
    bool blinkOn;

    void Awake()
    {
        if (driving == null) driving = GetComponent<BusDrivingController>();
        headlamps = GetComponentsInChildren<Light>(true);
        brakeOn = Emissive("Brake lamp lit", new Color(1f, 0.015f, 0.005f), 3.5f);
        brakeOff = Emissive("Brake lamp unlit", new Color(0.12f, 0.008f, 0.004f), 0.15f);
        amberOn = Emissive("Indicator lit", new Color(1f, 0.32f, 0.015f), 3f);
        amberOff = Emissive("Indicator unlit", new Color(0.16f, 0.045f, 0.005f), 0.12f);

        brakeLamps = new Renderer[] {
            Lamp("Left brake lamp", new Vector3(-0.78f, 1.45f, -3.56f), brakeOff, new Vector3(0.28f, 0.2f, 0.08f)),
            Lamp("Right brake lamp", new Vector3(0.78f, 1.45f, -3.56f), brakeOff, new Vector3(0.28f, 0.2f, 0.08f))
        };
        leftIndicators = new Renderer[] {
            Lamp("Front left indicator", new Vector3(-0.94f, 1.48f, 3.5f), amberOff, new Vector3(0.18f, 0.13f, 0.08f)),
            Lamp("Rear left indicator", new Vector3(-0.98f, 1.72f, -3.56f), amberOff, new Vector3(0.18f, 0.13f, 0.08f))
        };
        rightIndicators = new Renderer[] {
            Lamp("Front right indicator", new Vector3(0.94f, 1.48f, 3.5f), amberOff, new Vector3(0.18f, 0.13f, 0.08f)),
            Lamp("Rear right indicator", new Vector3(0.98f, 1.72f, -3.56f), amberOff, new Vector3(0.18f, 0.13f, 0.08f))
        };
    }

    void Update()
    {
        bool braking = driving != null && driving.brakeInput > 0.05f;
        foreach (Renderer lamp in brakeLamps) lamp.sharedMaterial = braking ? brakeOn : brakeOff;

        if (indicatorMode == 0)
        {
            Set(leftIndicators, amberOff);
            Set(rightIndicators, amberOff);
            return;
        }

        blinkClock += Time.deltaTime;
        if (blinkClock >= blinkInterval)
        {
            blinkClock = 0f;
            blinkOn = !blinkOn;
        }
        Material active = blinkOn ? amberOn : amberOff;
        Set(leftIndicators, indicatorMode == 1 || indicatorMode == 3 ? active : amberOff);
        Set(rightIndicators, indicatorMode == 2 || indicatorMode == 3 ? active : amberOff);
    }

    public void ToggleHeadlights()
    {
        headlightsOn = !headlightsOn;
        foreach (Light lamp in headlamps)
        {
            lamp.enabled = headlightsOn;
            lamp.intensity = headlightsOn ? 2.1f : 0f;
        }
    }

    public void SetIndicator(int mode)
    {
        indicatorMode = mode < 0 || mode > 3 ? 0 : mode;
        blinkClock = 0f;
        blinkOn = true;
    }

    public void CycleIndicator() => SetIndicator((indicatorMode + 1) % 4);

    Renderer Lamp(string label, Vector3 localPosition, Material material, Vector3 scale)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = label;
        obj.transform.SetParent(transform, false);
        obj.transform.localPosition = localPosition;
        obj.transform.localScale = scale;
        Collider collider = obj.GetComponent<Collider>();
        if (collider != null) Destroy(collider);
        Renderer renderer = obj.GetComponent<Renderer>();
        renderer.sharedMaterial = material;
        return renderer;
    }

    static void Set(Renderer[] lamps, Material material)
    {
        foreach (Renderer lamp in lamps) lamp.sharedMaterial = material;
    }

    static Material Emissive(string label, Color color, float intensity)
    {
        Shader shader = Shader.Find("Standard");
        Material material = new Material(shader) { name = label, color = color };
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", color * intensity);
        return material;
    }
}
