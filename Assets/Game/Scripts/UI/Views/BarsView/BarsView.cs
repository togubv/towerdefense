using System.Collections.Generic;
using UnityEngine;

namespace TD
{
    public class BarsView : MonoBehaviour
    {
        [SerializeField] private HealthBarContainer _healthBarPrefab;
        [SerializeField] private Transform _healthBarsParent;

        private ObjectPool<HealthBarContainer> _healthBarsPool;

        private List<HealthBarRuntimeData> _activeHealthBars;

        private RectTransform _canvasRectT;
        private Camera _cam;

        public void Init()
        {
            _activeHealthBars = new List<HealthBarRuntimeData>();
            _healthBarsPool = new ObjectPool<HealthBarContainer>(
                _healthBarPrefab,
                30,
                _healthBarsParent
                );
        }

        public void InitCanvas(Canvas canvas)
        {
            _cam = Camera.main;
            _canvasRectT = canvas.GetComponent<RectTransform>();
        }

        public void ShowHealthBar(Transform trans, float percent)
        {
            if (percent <= 0)
            {
                HideHealthBar(trans);
                return;
            }

            var data = GetHealthBarData(trans);

            if (data == null)
            {
                var container = _healthBarsPool.Get();
                data = new HealthBarRuntimeData(trans, container.rectT, container);
                _activeHealthBars.Add(data);
            }

            data.container.SetValue(percent);
            var screenPos = GetScreenPosition(trans.position);
            data.containerRectT.anchoredPosition = screenPos;
            
        }

        public void HideHealthBar(Transform trans)
        {
            var data = GetHealthBarData(trans);

            if (data == null)
            {
                return;
            }

            _activeHealthBars.Remove(data);
            _healthBarsPool.Release(data.container);
        }

        private void Update()
        {
            MoveBars();
        }

        private void MoveBars()
        {
            for (int i = _activeHealthBars.Count - 1; i >= 0; i--)
            {
                var healthBar = _activeHealthBars[i];

                if (healthBar == null || healthBar.trans == null)
                {
                    continue;
                }

                var screenPos = GetScreenPosition(healthBar.trans.position);
                healthBar.containerRectT.anchoredPosition = screenPos;
            }
        }

        private Vector2 GetScreenPosition(Vector3 worldPos)
        {
            Vector2 screenPos = _cam.WorldToScreenPoint(
                worldPos
                );

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvasRectT,
                screenPos,
                _cam,
                out Vector2 localPos
            );

            return localPos;
        }

        private HealthBarRuntimeData GetHealthBarData(Transform trans)
        {
            foreach (var h in _activeHealthBars)
            {
                if (h.trans == trans)
                {
                    return h;
                }
            }

            return null;
        }
    }
}
