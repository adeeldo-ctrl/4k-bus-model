using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Karachi4KBusBootstrap : MonoBehaviour
{
    [Header("Scene")]
    public bool createGround = true;
    public bool createTouchHud = true;
    public Color groundColor = new Color(0.20f, 0.25f, 0.22f);

    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        BuildWorld();
        GameObject bus = BuildBus();
        var driver = bus.AddComponent<BusDrivingController>();
        driver.Configure();
        BuildCamera(bus.transform);
        if (createTouchHud) BuildTouchHud(driver);
    }

    void BuildWorld()
    {
        if (createGround)
        {
            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Karachi Test Ground";
            ground.transform.localScale = new Vector3(12, 1, 12);
            ground.GetComponent<Renderer>().material = Mat("Asphalt", groundColor, 0.9f);
        }
        RenderSettings.ambientLight = new Color(0.65f, 0.66f, 0.68f);
        var sun = new GameObject("Sun").AddComponent<Light>();
        sun.type = LightType.Directional;
        sun.intensity = 1.15f;
        sun.transform.rotation = Quaternion.Euler(48, -32, 0);
    }

    GameObject BuildBus()
    {
        var root = new GameObject("Karachi 4K Bedford Local Bus");
        root.transform.position = new Vector3(0, 0.15f, 0);
        var body = Mat("Deep turquoise bus paint", new Color(0.02f, 0.36f, 0.38f), 0.42f);
        var cream = Mat("Warm cream", new Color(0.95f, 0.79f, 0.48f), 0.5f);
        var red = Mat("Truck art red", new Color(0.72f, 0.07f, 0.08f), 0.45f);
        var gold = Mat("Truck art gold", new Color(1f, 0.62f, 0.08f), 0.4f);
        var blue = Mat("Truck art blue", new Color(0.04f, 0.22f, 0.72f), 0.4f);
        var glass = Mat("Tinted glass", new Color(0.07f, 0.16f, 0.20f), 0.18f);
        var rubber = Mat("Tyre rubber", new Color(0.025f, 0.028f, 0.03f), 0.95f);
        var chrome = Mat("Chrome", new Color(0.72f, 0.75f, 0.76f), 0.2f);

        Part(root.transform, "Lower chassis", new Vector3(0, 1.05f, 0), new Vector3(2.35f, 0.55f, 7.0f), body);
        Part(root.transform, "Classic Bedford rounded bonnet", new Vector3(0, 1.48f, 2.62f), new Vector3(2.18f, 0.82f, 1.65f), cream);
        Part(root.transform, "Passenger cabin", new Vector3(0, 2.55f, -0.45f), new Vector3(2.3f, 2.15f, 5.1f), body);
        Part(root.transform, "Roof", new Vector3(0, 3.68f, -0.45f), new Vector3(2.42f, 0.18f, 5.25f), cream);

        // Front windscreen and rear glass
        Part(root.transform, "Front windscreen", new Vector3(0, 2.72f, 2.25f), new Vector3(1.9f, 0.92f, 0.055f), glass);
        Part(root.transform, "Rear window", new Vector3(0, 2.72f, -3.03f), new Vector3(1.85f, 0.85f, 0.05f), glass);

        // Side window panels and pillars
        for (int side = -1; side <= 1; side += 2)
        {
            float x = side * 1.17f;
            for (int i = 0; i < 5; i++)
            {
                float z = 1.45f - i * 0.91f;
                Part(root.transform, "Side window " + side + " " + i, new Vector3(x, 2.83f, z),
                    new Vector3(0.045f, 0.82f, 0.76f), glass);
                Part(root.transform, "Window pillar " + side + " " + i,
                    new Vector3(side * 1.19f, 2.83f, z - 0.43f), new Vector3(0.08f, 1.02f, 0.08f), cream);
            }
            // Truck-art bands
            Part(root.transform, "Lower red art stripe", new Vector3(side * 1.19f, 1.56f, -0.4f), new Vector3(0.07f, 0.16f, 5.0f), red);
            Part(root.transform, "Gold art stripe", new Vector3(side * 1.20f, 1.75f, -0.4f), new Vector3(0.08f, 0.09f, 5.0f), gold);
            Part(root.transform, "Blue art stripe", new Vector3(side * 1.20f, 1.91f, -0.4f), new Vector3(0.08f, 0.08f, 5.0f), blue);
        }

        // Front grille, bumper, lights and mirrors
        Part(root.transform, "Front grille", new Vector3(0, 1.32f, 3.46f), new Vector3(0.85f, 0.32f, 0.08f), chrome);
        Part(root.transform, "Front bumper", new Vector3(0, 0.98f, 3.52f), new Vector3(2.35f, 0.16f, 0.18f), chrome);
        for (int side = -1; side <= 1; side += 2)
        {
            var lamp = Part(root.transform, "Headlight " + side, new Vector3(side * 0.82f, 1.52f, 3.48f),
                new Vector3(0.32f, 0.25f, 0.1f), cream);
            var light = lamp.AddComponent<Light>();
            light.type = LightType.Spot;
            light.range = 24;
            light.spotAngle = 48;
            light.intensity = 0;
            light.enabled = false;
            Part(root.transform, "Mirror arm " + side, new Vector3(side * 1.34f, 2.55f, 2.05f),
                new Vector3(0.08f, 0.08f, 0.72f), chrome);
            Part(root.transform, "Mirror " + side, new Vector3(side * 1.36f, 2.62f, 1.68f),
                new Vector3(0.12f, 0.35f, 0.28f), chrome);
        }

        // Wheels
        foreach (float z in new float[] { 2.0f, -2.15f })
        foreach (int side in new int[] { -1, 1 })
        {
            var wheel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            wheel.name = "Road wheel";
            wheel.transform.SetParent(root.transform);
            wheel.transform.localPosition = new Vector3(side * 1.18f, 0.73f, z);
            wheel.transform.localRotation = Quaternion.Euler(0, 0, 90);
            wheel.transform.localScale = new Vector3(0.82f, 0.22f, 0.82f);
            wheel.GetComponent<Renderer>().material = rubber;
        }

        // Cabin floor and passenger seats
        Part(root.transform, "Interior floor", new Vector3(0, 1.55f, -0.45f), new Vector3(1.95f, 0.12f, 4.95f), cream);
        for (int row = 0; row < 5; row++)
        {
            float z = 1.25f - row * 0.88f;
            foreach (int side in new int[] { -1, 1 })
            {
                Part(root.transform, "Passenger seat cushion", new Vector3(side * 0.53f, 1.95f, z),
                    new Vector3(0.72f, 0.16f, 0.62f), red);
                Part(root.transform, "Passenger seat back", new Vector3(side * 0.53f, 2.28f, z - 0.24f),
                    new Vector3(0.72f, 0.55f, 0.14f), blue);
            }
        }

        // Driver seat, steering wheel and dashboard
        Part(root.transform, "Driver seat", new Vector3(-0.55f, 2.0f, 2.05f), new Vector3(0.62f, 0.18f, 0.65f), red);
        Part(root.transform, "Driver seat back", new Vector3(-0.55f, 2.36f, 1.78f), new Vector3(0.62f, 0.58f, 0.16f), red);
        Part(root.transform, "Dashboard", new Vector3(0, 2.05f, 2.75f), new Vector3(1.85f, 0.3f, 0.5f), cream);
        var steering = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        steering.name = "Steering wheel";
        steering.transform.SetParent(root.transform);
        steering.transform.localPosition = new Vector3(-0.55f, 2.28f, 2.45f);
        steering.transform.localRotation = Quaternion.Euler(90, 0, 0);
        steering.transform.localScale = new Vector3(0.42f, 0.045f, 0.42f);
        steering.GetComponent<Renderer>().material = chrome;

        // Functional passenger door hinged at the right side.
        var doorPivot = new GameObject("Passenger door hinge");
        doorPivot.transform.SetParent(root.transform);
        doorPivot.transform.localPosition = new Vector3(1.12f, 1.6f, 0.25f);
        var door = Part(doorPivot.transform, "Passenger door", new Vector3(0, 0.72f, 0),
            new Vector3(0.09f, 1.45f, 0.92f), cream);
        door.AddComponent<BusDoor>();
        Part(root.transform, "Roof sign board", new Vector3(0, 3.85f, 0.6f), new Vector3(1.75f, 0.24f, 0.25f), red);

        return root;
    }

    void BuildCamera(Transform bus)
    {
        var camObj = new GameObject("Bus Camera");
        var cam = camObj.AddComponent<Camera>();
        cam.tag = "MainCamera";
        camObj.AddComponent<AudioListener>();
        var rig = camObj.AddComponent<BusCameraRig>();
        rig.target = bus;
        rig.cameraComponent = cam;
    }

    void BuildTouchHud(BusDrivingController controller)
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            var es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
        }
        var canvasObj = new GameObject("Mobile HUD");
        var canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasObj.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
        canvasObj.AddComponent<GraphicRaycaster>();
        var hud = canvasObj.AddComponent<BusTouchHud>();
        hud.controller = controller;
    }

    static GameObject Part(Transform parent, string name, Vector3 pos, Vector3 scale, Material material)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(parent);
        go.transform.localPosition = pos;
        go.transform.localScale = scale;
        go.GetComponent<Renderer>().material = material;
        return go;
    }

    static Material Mat(string name, Color color, float smoothness)
    {
        var mat = new Material(Shader.Find("Standard"));
        mat.name = name;
        mat.color = color;
        mat.SetFloat("_Glossiness", smoothness);
        return mat;
    }
}
