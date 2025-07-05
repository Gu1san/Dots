using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

public partial struct EnemyProjectileMoveSystem : ISystem{

    [BurstCompile]
    public void OnUpdate(ref SystemState state){
       float deltatime = SystemAPI.Time.DeltaTime;
        foreach (var (Transform, moveData) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<ProjectileMoveData>>().WithAll<ProjectileTag>()){
            Transform.ValueRW.Position += moveData.ValueRO.Direction * moveData.ValueRO.Speed * deltatime;
        }
    }
}
