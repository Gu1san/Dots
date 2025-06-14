using Unity.Entities;
using Unity.Mathematics;

namespace Game.Components
{
    public struct InputData : IComponentData
    {
        public float2 MoveDirection;
    }
}