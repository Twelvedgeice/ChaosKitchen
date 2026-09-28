using UnityEngine;

// A camera-facing yellow arrow marking the existing delivery counter.
public class DeliveryMarker : MonoBehaviour
{
    private Mesh mesh;
    private Material material;
    private Vector3 origin;
    public static void Attach(Transform counter)
    {
        if (counter.Find("DeliveryArrow") != null) return;
        var go = new GameObject("DeliveryArrow");
        go.transform.SetParent(counter, false);
        float top = counter.position.y + 1.3f;
        foreach (var r in counter.GetComponentsInChildren<MeshRenderer>())
            if (r.enabled) top = Mathf.Max(top, r.bounds.max.y);
        go.transform.position = new Vector3(counter.position.x, top + .85f, counter.position.z);
        go.transform.localScale = Vector3.one;
        go.AddComponent<DeliveryMarker>();
    }
    private void Awake()
    {
        origin = transform.position;
        mesh = new Mesh { name = "DeliveryArrowMesh" };
        mesh.vertices = new[] {
            new Vector3(-.13f,.65f,0), new Vector3(.13f,.65f,0),
            new Vector3(.13f,.13f,0), new Vector3(.36f,.13f,0),
            new Vector3(0,-.35f,0), new Vector3(-.36f,.13f,0),
            new Vector3(-.13f,.13f,0)
        };
        mesh.triangles = new[] {0,1,2,0,2,6,5,3,4,2,1,0,6,2,0,4,3,5};
        mesh.RecalculateNormals(); mesh.RecalculateBounds();
        gameObject.AddComponent<MeshFilter>().sharedMesh = mesh;
        material = new Material(Shader.Find("Sprites/Default"));
        material.color = new Color(1f,.82f,0f,1f);
        gameObject.AddComponent<MeshRenderer>().sharedMaterial = material;
    }
    private void LateUpdate()
    {
        transform.position = origin + Vector3.up * (Mathf.Sin(Time.time * 2.5f) * .1f);
        if (Camera.main != null) transform.rotation = Camera.main.transform.rotation;
    }
    private void OnDestroy()
    {
        if (mesh != null) Destroy(mesh);
        if (material != null) Destroy(material);
    }
}
