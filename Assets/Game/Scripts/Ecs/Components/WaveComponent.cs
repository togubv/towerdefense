using UnityEngine;

namespace TD
{
    public struct WaveComponent
    {
        public string mobId;
        public int amount;
        public Vector3 spawnPos;
        public float spawnPeriod;

        public float spawnPeriodTimer;
        public int spawnedValue;
    }
}