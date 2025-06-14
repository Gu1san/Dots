using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Game.Components;

namespace Game.Systems
{
    [BurstCompile]
    public partial struct PlayerInputSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float2 move = float2.zero;
            move.x = Input.GetAxisRaw("Horizontal");
            move.y = Input.GetAxisRaw("Vertical");

            foreach (var input in SystemAPI.Query<RefRW<Game.Components.InputData>>().WithAll<PlayerTag>())
            {
                input.ValueRW.MoveDirection = move;
            }
        }
    }
}