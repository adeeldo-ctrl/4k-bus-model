using UnityEngine;

// Lightweight procedural Karachi street blockout. Uses shared materials and simple meshes
// so the starter scene remains suitable for mobile iteration.
public class KarachiStreetGenerator : MonoBehaviour
{
    public int buildingPairs = 7;
    public float streetLength = 150f;

    void Start()
    {
        BuildStreet();
    }

    void BuildStreet()
    {
        Material asphalt = MakeMat("Road asphalt", new Color(0.12f, 0.14f, 0.16f));
        Material line = MakeMat("Road marking", new Color(0.88f, 0.86f, 0.76f));
        Material pavement = MakeMat("Concrete pavement", new Color(0.43f, 0.43f, 0.40f));
        Material curb = MakeMat("Curb paint", new Color(0.83f, 0.78f, 0.64f));
        Material glass = MakeMat("Shop glass", new Color(0.08f, 0.18f, 0.22f));
        Material[] walls = {
            MakeMat("Building sand", new Color(0.69f, 0.56f, 0.40f)),
            MakeMat("Building pale blue", new Color(0.42f, 0.56f, 0.59f)),
            MakeMat("Building warm white", new Color(0.78f, 0.73f, 0.62f)),
            MakeMat("Building faded peach", new Color(0.68f, 0.43f, 0.34f))
        };
        Material[] signMats = {
            MakeMat("Shop sign teal", new Color(0.02f, 0.34f, 0.34f)),
            MakeMat("Shop sign red", new Color(0.58f, 0.08f, 0.06f)),
            MakeMat("Shop sign blue", new Color(0.05f, 0.18f, 0.48f))
        };

        Cube("Karachi carriageway", new Vector3(0, -0.04f, 0),
            new Vector3(10f, 0.18f, streetLength), asphalt);
        Cube("Left pavement", new Vector3(-6.1f, 0.02f, 0),
            new Vector3(2.2f, 0.24f, streetLength), pavement);
        Cube("Right pavement", new Vector3(6.1f, 0.02f, 0),
            new Vector3(2.2f, 0.24f, streetLength), pavement);
        Cube("Left curb", new Vector3(-5.05f, 0.12f, 0),
            new Vector3(0.18f, 0.28f, streetLength), curb);
        Cube("Right curb", new Vector3(5.05f, 0.12f, 0),
            new Vector3(0.18f, 0.28f, streetLength), curb);

        for (float z = -streetLength * 0.48f; z < streetLength * 0.48f; z += 8f)
        {
            Cube("Broken lane marking", new Vector3(0, 0.065f, z),
                new Vector3(0.12f, 0.025f, 3.5f), line);
        }

        for (int i = 0; i < buildingPairs; i++)
        {
            float z = -streetLength * 0.43f + i * (streetLength * 0.86f / Mathf.Max(1, buildingPairs - 1));
            for (int side = -1; side <= 1; side += 2)
            {
                float width = 6.4f + (i % 3) * 0.7f;
                float height = 6.5f + (i % 4) * 1.15f;
                float x = side * 10.2f;
                Material wall = walls[(i + (side > 0 ? 1 : 0)) % walls.Length];
                Cube("Karachi low-rise building", new Vector3(x, height * 0.5f, z),
                    new Vector3(width, height, 12.5f), wall);

                // Street-facing shopfront glazing and signboard.
                float frontX = x - side * (width * 0.5f + 0.045f);
                Cube("Shopfront glass", new Vector3(frontX, 1.45f, z),
                    new Vector3(0.08f, 1.85f, 4.1f), glass);
                Cube("Colourful shop sign", new Vector3(frontX - side * 0.06f, 2.75f, z),
                    new Vector3(0.16f, 0.62f, 4.8f), signMats[i % signMats.Length]);

                // Upper-floor windows facing the road.
                for (int floor = 0; floor < 2; floor++)
                for (int window = 0; window < 2; window++)
                {
                    float wz = z - 2.0f + window * 4.0f;
                    Cube("Upper building window", new Vector3(frontX, 4.0f + floor * 2.0f, wz),
                        new Vector3(0.09f, 1.05f, 1.35f), glass);
                }
            }
        }

        Material poleMat = MakeMat("Streetlight metal", new Color(0.22f, 0.24f, 0.25f));
        Material foliage = MakeMat("Street tree foliage", new Color(0.12f, 0.31f, 0.18f));
        Material trunk = MakeMat("Tree trunk", new Color(0.30f, 0.19f, 0.11f));
        for (int i = 0; i < buildingPairs; i++)
        {
            float z = -streetLength * 0.4f + i * (streetLength * 0.8f / Mathf.Max(1, buildingPairs - 1));
            for (int side = -1; side <= 1; side += 2)
            {
                float x = side * 6.7f;
                Cube("Streetlight pole", new Vector3(x, 3.0f, z + 4f),
                    new Vector3(0.12f, 6f, 0.12f), poleMat);
                Cube("Streetlight arm", new Vector3(x - side * 0.45f, 5.8f, z + 4f),
                    new Vector3(0.9f, 0.1f, 0.12f), poleMat);
                Cube("Streetlight lamp", new Vector3(x - side * 0.85f, 5.72f, z + 4f),
                    new Vector3(0.42f, 0.16f, 0.3f), line);

                if (i % 2 == 0)
                {
                    Cube("Tree trunk", new Vector3(side * 7.0f, 1.1f, z - 3f),
                        new Vector3(0.35f, 2.2f, 0.35f), trunk);
                    GameObject crown = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    crown.name = "Street tree canopy";
                    crown.transform.SetParent(transform);
                    crown.transform.position = new Vector3(side * 7.0f, 3.0f, z - 3f);
                    crown.transform.localScale = new Vector3(2.0f, 2.2f, 2.0f);
                    crown.GetComponent<Renderer>().sharedMaterial = foliage;
                }
            }
        }
    }

    void Cube(string objectName, Vector3 position, Vector3 scale, Material material)
    {
        GameObject part = GameObject.CreatePrimitive(PrimitiveType.Cube);
        part.name = objectName;
        part.transform.SetParent(transform, false);
        part.transform.localPosition = position;
        part.transform.localScale = scale;
        part.GetComponent<Renderer>().sharedMaterial = material;
    }

    Material MakeMat(string materialName, Color color)
    {
        Shader shader = Shader.Find("Standard");
        Material material = new Material(shader);
        material.name = materialName;
        material.color = color;
        return material;
    }
}
