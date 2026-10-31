using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace TD
{
    public class VoxelsGenerator : MonoBehaviour
    {
        public GameObject voxelPlanePrefab1;
        public GameObject voxelPlanePrefab2;
        public Transform voxelsParent;
        public BoxCollider boxCollider;
        public Vector2Int matrixSize;

        [SerializeField]
        [HideInInspector]
        private List<GameObject> _voxels = new List<GameObject>();

        [Button("Generate")]
        public void GeneratePlane()
        {
            if (voxelPlanePrefab1 == null || voxelPlanePrefab2 == null)
            {
                return;
            }

            Clear();

            boxCollider.size = new Vector3(matrixSize.x, 1, matrixSize.y);
            boxCollider.center = new Vector3(matrixSize.x / 2 - 0.5f, 0, matrixSize.y / 2 - 0.5f);

            for (int i = 0; i < matrixSize.x; i++)
            {
                for (int k = 0; k < matrixSize.y; k++)
                {
                    GameObject prefab = voxelPlanePrefab1;

                    if ((i + k) % 2 == 0)
                    {
                        prefab = voxelPlanePrefab2;
                    }

#if UNITY_EDITOR
                    var spawned = (GameObject)PrefabUtility.InstantiatePrefab(prefab, voxelsParent);
                    spawned.transform.localPosition = new Vector3(i, 0, k);
                    _voxels.Add(spawned);
#endif
                }
            }

            MarkDirty();
        }

        [Button("Clear")]
        public void Clear()
        {
            if (_voxels == null)
            {
                return;
            }

            if (_voxels.Count < 1)
            {
                return;
            }

            foreach (var v in _voxels)
            {
                DestroyImmediate(v);
            }

            _voxels.Clear();
            MarkDirty();
        }

        private void MarkDirty()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorUtility.SetDirty(this);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
#endif
        }
    }
}
