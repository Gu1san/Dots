using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public partial struct EnemyMoveSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        Entity playerEntity;
        if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out playerEntity)) return;
            LocalTransform playerTransform = SystemAPI.GetComponent<LocalTransform>(playerEntity);
            float deltaTime = SystemAPI.Time.DeltaTime;
            foreach (var (transform, moveSpeed) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<MoveSpeedEnemy>>().WithAll<EnemyTag>()){

            float3 enemyPosition = transform.ValueRO.Position;
            float3 playerPosition = playerTransform.Position;

            float3 direction = playerPosition - enemyPosition; 

            direction.y = 0; 
            direction = math.normalize(direction); 

            float distance = math.distance(playerPosition, enemyPosition);

            transform.ValueRW.Rotation = quaternion.LookRotationSafe(direction, math.up());

            if (distance > moveSpeed.ValueRO.stopValue){
                transform.ValueRW.Position += direction * moveSpeed.ValueRO.value * deltaTime;
            }
        }
    }
}


