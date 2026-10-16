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
        [Header("Rewards")]
        [SerializeField] private int _gold;
        [SerializeField] private int _water;
        [SerializeField] private int _fire;
        [SerializeField] private int _earth;
        [SerializeField] private int _air;

        public string id => name;
        public UnitView prefab => _prefab;
        public string title => _title;
        public string description => _description;
        public Sprite icon => _icon;
        public float baseHealthValue => _baseHealthValue;
        public float movespeed => _movespeed;
        public int gold => _gold;
        public int water => _water;
        public int fire => _fire;
        public int earth => _earth;
        public int air => _air;
    }
}
