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
        [SerializeField] private int _costGold;
        [Header("Stats")]
        [SerializeField] private float _attackPerSeconds = 1f;

        public string id => name;
        public BuildingView prefab => _prefab;
        public string title => _title;
        public string description => _description;
        public Sprite icon => _icon;
        public int costGold => _costGold;
        public float attackPerSeconds => _attackPerSeconds;
    }
}