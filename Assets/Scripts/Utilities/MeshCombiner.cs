using UnityEngine;

public class MeshCombiner : MonoBehaviour {
    void Start() {
        var meshFilters = GetComponentsInChildren<MeshFilter>();
        var combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < combine.Length; i++) {
            var mf = meshFilters[i];
            combine[i].mesh = mf.sharedMesh;
            combine[i].transform = mf.transform.localToWorldMatrix;
        }

        var meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
        meshFilter.mesh.CombineMeshes(combine);

        foreach (var mf in meshFilters) {
            Destroy(mf);
            Destroy(mf.GetComponent<MeshRenderer>());
        }

        foreach (Transform child in transform) { child.gameObject.hideFlags = HideFlags.HideInHierarchy; }
    }
}
