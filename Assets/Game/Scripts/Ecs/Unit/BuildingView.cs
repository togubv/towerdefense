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

        private Material[] _baseMaterials;
        private bool _isUnplaceable;

        public void SetPreviewState()
        {
            triggerObserver.gameObject.SetActive(false);
            coll.enabled = false;
            hitboxColl.enabled = false;
            meshR.materials = previewMaterials;
        }

        public void ToggleSelectedState(bool toggle)
        {
            if (toggle)
            {
                meshR.materials = previewMaterials;
            }
            else
            {
                meshR.materials = _baseMaterials;
            }
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

        private void Awake()
        {
            _baseMaterials = meshR.materials;
        }
    }
}