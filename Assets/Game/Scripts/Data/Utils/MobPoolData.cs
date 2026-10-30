using UnityEngine;

namespace TD
{
    [CreateAssetMenu(fileName = "MobPoolData", menuName = "Data/Mob")]
    public class MobPoolData : ScriptableObject
    {
        [SerializeField] private UnitView _prefab;
        [SerializeField] private string _title;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;
        [Header("Stats")]
        [SerializeField] private float _baseHealthValue;
        [SerializeField] private float _movespeed;
        [SerializeField] private CurrencyAmount[] _rewards;

        public string id => name;
        public UnitView prefab => _prefab;
        public string title => _title;
        public string description => _description;
        public Sprite icon => _icon;
        public float baseHealthValue => _baseHealthValue;
        public float movespeed => _movespeed;
        public CurrencyAmount[] rewards => _rewards;
    }
}
