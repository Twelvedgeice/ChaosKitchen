using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [System.Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }
    [SerializeField] private List<KitchenObjectSO_GameObject> KitchenObjectSO_GameObjectList;
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    private readonly List<GameObject> visuals = new List<GameObject>();

    private void OnEnable()
    {
        if (plateKitchenObject == null) plateKitchenObject = GetComponentInParent<PlateKitchenObject>();
        if (plateKitchenObject != null) plateKitchenObject.OnIngredientAdded += IngredientAdded;
        if (plateKitchenObject != null) plateKitchenObject.OnContentsChanged += ContentsChanged;
        RefreshVisuals();
    }
    private void Start() { RefreshVisuals(); }
    private void OnDisable()
    {
        if (plateKitchenObject != null) plateKitchenObject.OnIngredientAdded -= IngredientAdded;
        if (plateKitchenObject != null) plateKitchenObject.OnContentsChanged -= ContentsChanged;
    }
    private void IngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEvnetArgs e) { RefreshVisuals(); }

    private void ContentsChanged(object sender, System.EventArgs e) { RefreshVisuals(); }
    public void RefreshVisuals()
    {
        if (KitchenObjectSO_GameObjectList != null)
            foreach (var old in KitchenObjectSO_GameObjectList)
                if (old.gameObject != null) old.gameObject.SetActive(false);
        foreach (var visual in visuals)
        {
            if (visual == null) continue;
            visual.SetActive(false);
            if (Application.isPlaying) Destroy(visual); else DestroyImmediate(visual);
        }
        visuals.Clear();
        if (plateKitchenObject == null || plateKitchenObject.GetKitchenObjectSOList() == null) return;
        var foods = plateKitchenObject.GetKitchenObjectSOList();
        for (int i = 0; i < foods.Count; i++)
        {
            var food = foods[i];
            if (food == null || food.prefab == null) continue;
            var visual = new GameObject("PlateFood_" + food.name);
            visual.transform.SetParent(plateKitchenObject.transform, false);
            // Copy only visible meshes; plate decorations must not contain gameplay scripts.
            foreach (var filter in food.prefab.GetComponentsInChildren<MeshFilter>(true))
            {
                var renderer = filter.GetComponent<MeshRenderer>();
                if (filter.sharedMesh == null || renderer == null || !renderer.enabled) continue;
                bool active = true;
                for (var t = filter.transform; t != null; t = t.parent)
                {
                    if (!t.gameObject.activeSelf) active = false;
                    if (t == food.prefab) break;
                }
                if (!active) continue;
                var part = new GameObject(filter.name, typeof(MeshFilter), typeof(MeshRenderer));
                part.transform.SetParent(visual.transform, false);
                part.transform.localPosition = food.prefab.InverseTransformPoint(filter.transform.position);
                part.transform.localRotation = Quaternion.Inverse(food.prefab.rotation) * filter.transform.rotation;
                var s = food.prefab.lossyScale;
                var f = filter.transform.lossyScale;
                part.transform.localScale = new Vector3(f.x/s.x,f.y/s.y,f.z/s.z);
                part.GetComponent<MeshFilter>().sharedMesh = filter.sharedMesh;
                part.GetComponent<MeshRenderer>().sharedMaterials = renderer.sharedMaterials;
            }
            var renderers = visual.GetComponentsInChildren<MeshRenderer>();
            if (renderers.Length == 0) { DestroyImmediate(visual); continue; }
            Bounds bounds = renderers[0].bounds;
            foreach (var r in renderers) bounds.Encapsulate(r.bounds);
            float scale = .52f / Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z, .001f);
            Vector3 center = visual.transform.InverseTransformPoint(bounds.center);
            float bottom = visual.transform.InverseTransformPoint(new Vector3(bounds.center.x,bounds.min.y,bounds.center.z)).y;
            visual.transform.localScale = Vector3.one * scale;
            float x = foods.Count == 1 ? 0f : (i % 2 == 0 ? -.27f : .27f);
            float z = foods.Count <= 2 ? 0f : (i / 2 == 0 ? -.27f : .27f);
            visual.transform.localPosition = new Vector3(x-center.x*scale,.15f-bottom*scale,z-center.z*scale);
            visuals.Add(visual);
        }
    }
}
