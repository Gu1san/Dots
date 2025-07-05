using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public partial struct EnemyMoveSystem : ISystem {
    [BurstCompile]
    public void OnUpdate(ref SystemState state){
        Entity playerEntity;
        Debug.Log("cheguasasasasi aqui?");
        if (!SystemAPI.TryGetSingletonEntity<playerTag>(out playerEntity)) return;
        Debug.Log("cheguei aqui?");
        LocalTransform playerTransform = SystemAPI.GetComponent<LocalTransform>(playerEntity);
        Debug.Log("passei aqui?");
        float deltaTime = SystemAPI.Time.DeltaTime;
        foreach (var (transform, moveSpeed) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<MoveSpeedEnemy>>().WithAll<EnemyTag>()){
            var direction = math.normalize(playerTransform.Position - transform.ValueRO.Position);
            transform.ValueRW.Position += direction * moveSpeed.ValueRO.value * deltaTime;
        }
    }
}
