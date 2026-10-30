using UnityEngine;

namespace TD
{
    [CreateAssetMenu(fileName = "BuildingPoolData", menuName = "Data/Building")]
    public class BuildingPoolData : ScriptableObject
    {
        [SerializeField] private BuildingView _prefab;
        [SerializeField] private string _title;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private CurrencyAmount[] _costs;
        [SerializeField] private BuildingUpgradeData[] _upgrades;
        [Header("Stats")]
        [SerializeField] private float _attackPerSeconds = 1f;

        public string id => name;
        public BuildingView prefab => _prefab;
        public string title => _title;
        public string description => _description;
        public Sprite icon => _icon;
        public CurrencyAmount[] costs => _costs;
        public BuildingUpgradeData[] upgrades => _upgrades;
        public float attackPerSeconds => _attackPerSeconds;
    }
}