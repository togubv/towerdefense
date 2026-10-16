using UnityEngine;

namespace TD
{
    public class BuildingView : MonoBehaviour
    {
        public TriggerObserver triggerObserver;
        public Animator animator;
        public Collider coll;
        public MeshRenderer meshR;
        public Material[] previewMaterials;

        public void SetPreviewState()
        {
            coll.enabled = false;
            meshR.materials = previewMaterials;
        }
    }
}