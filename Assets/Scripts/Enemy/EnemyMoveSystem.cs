using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;

public partial struct EnemyMoveSystem : ISystem {
    [BurstCompile]
    public void OnUpdate(ref SystemState state){
        Entity playerEntity;
        LocalTransform playerTransform;
        if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out playerEntity)) return;
        if (!SystemAPI.TryGetSingleton(out playerTransform)) return;
        float deltaTime = SystemAPI.Time.DeltaTime;
        foreach (var (transform, moveSpeed) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<MoveSpeedEnemy>>().WithAll<EnemyTag>()){
            var direction = math.normalize(playerTransform.Position - transform.ValueRO.Position);
            transform.ValueRW.Position += direction * moveSpeed.ValueRO.value * deltaTime;
        }
    }
}
