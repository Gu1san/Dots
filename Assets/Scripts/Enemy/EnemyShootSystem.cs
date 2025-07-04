using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] 
public partial struct EnemyShootSystem : ISystem{
    [BurstCompile]
    public void OnCreate(ref SystemState state){
        state.RequireForUpdate<PlayerTag>();
        state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state){
        if (!SystemAPI.TryGetSingleton(out LocalToWorld playerTransform))return;

        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        float deltaTime = SystemAPI.Time.DeltaTime;


        foreach (var (gun, gunTransform) in SystemAPI.Query<RefRW<EnemyGun>, RefRO<LocalToWorld>>().WithAll<EnemyTag>()){
            gun.ValueRW.shotCooldown -= deltaTime;
            if (gun.ValueRO.shotCooldown > 0) continue;
            gun.ValueRW.shotCooldown = gun.ValueRO.fireRate;
            float3 fireDirection = math.normalize(playerTransform.Position - gunTransform.ValueRO.Position);
            Entity newBullet = ecb.Instantiate(gun.ValueRO.bulletPrefab);
            var bulletTransform = LocalTransform.FromPositionRotationScale(
                gunTransform.ValueRO.Position,
                quaternion.LookRotationSafe(fireDirection, math.up()),
                gun.ValueRO.bulletScale);
            ecb.SetComponent(newBullet, bulletTransform);

            // ecb.AddComponent(newBullet new projectMoveData
            // {
            //     linear = fireDirection * gun.ValueRO.shotStrength,
            //     angular = float3.zero
            // });
        }
    }
}