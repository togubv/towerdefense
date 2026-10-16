using UnityEngine;

namespace TD
{
    [System.Serializable]
    public class HealthBarRuntimeData
    {
        public Transform trans;
        public RectTransform containerRectT;
        public HealthBarContainer container;

        public HealthBarRuntimeData(Transform trans, RectTransform containerRectT, HealthBarContainer container)
        {
            this.trans = trans;
            this.containerRectT = containerRectT;
            this.container = container;
        }
    }
}