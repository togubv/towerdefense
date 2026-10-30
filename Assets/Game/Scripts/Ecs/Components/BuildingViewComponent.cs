using Leopotam.Ecs;
using UnityEngine;

namespace TD
{
    public struct BuildingViewComponent
    {
        public EcsEntity entity;
        public Transform trans;
        public BuildingView view;
    }
}