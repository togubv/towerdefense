using UnityEngine;
using UnityEngine.UI;

namespace TD
{
    public class HealthBarContainer : MonoBehaviour, IPoolable
    {
        public RectTransform rectT;
        public Image fillImage;
        
        public void SetValue(float percent)
        {
            fillImage.fillAmount = percent;
        }

        public void OnGet()
        {
            gameObject.SetActive(true);
        }

        public void OnRelease()
        {
            gameObject.SetActive(false);
        }
    }
}