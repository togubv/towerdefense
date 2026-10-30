using UnityEngine;

namespace TD
{
    public class BuildingView : MonoBehaviour
    {
        public TriggerObserver triggerObserver;
        public Animator animator;
        public Collider coll;
        public Collider hitboxColl;
        public MeshRenderer meshR;
        public Material[] previewMaterials;
        public Material[] previewMaterialsUnplaeable;

        private bool _isUnplaceable;

        public void SetPreviewState()
        {
            triggerObserver.gameObject.SetActive(false);
            coll.enabled = false;
            hitboxColl.enabled = false;
            meshR.materials = previewMaterials;
        }

        public void SetUnplaceableState(bool toggle)
        {
            if (_isUnplaceable == toggle)
            {
                return;
            }

            if (toggle)
            {
                _isUnplaceable = true;
                meshR.materials = previewMaterialsUnplaeable;
            }
            else
            {
                _isUnplaceable = false;
                meshR.materials = previewMaterials;
            }
        }
    }
}