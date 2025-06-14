using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Game.Components;

namespace Game.Systems
{
    [BurstCompile]
    public partial struct PlayerMoveSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (transform, input, speed) in
                SystemAPI.Query<RefRW<LocalTransform>, RefRO<Game.Components.InputData>, RefRO<MoveSpeed>>().WithAll<PlayerTag>())
            {
                float3 move = new float3(input.ValueRO.MoveDirection.x, 0, input.ValueRO.MoveDirection.y);
                transform.ValueRW.Position += move * speed.ValueRO.Value * deltaTime;
            }
        }
    }
}