
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

[UpdateInGroup(typeof(PhysicsSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
public partial struct EnemyDamageSystem : ISystem
{
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<SimulationSingleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state){
        var simulationSingleton = SystemAPI.GetSingleton<SimulationSingleton>();
        var ecb = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        state.Dependency = new DamageJob{
            EnemyLifeLookup = SystemAPI.GetComponentLookup<EnemyLife>(false),
            PlayerProjectileDataLookup = SystemAPI.GetComponentLookup<BulletData>(true),
            PlayerProjectileLookup = SystemAPI.GetComponentLookup<PlayerProjectileTag>(true),
            EnemyLookup = SystemAPI.GetComponentLookup<EnemyTag>(true),
            Ecb = ecb
        }.Schedule(simulationSingleton, state.Dependency);
    }
}

[BurstCompile]
public struct DamageJob : ITriggerEventsJob{
    public ComponentLookup<EnemyLife> EnemyLifeLookup;
    [ReadOnly] public ComponentLookup<PlayerProjectileTag> PlayerProjectileLookup;
    [ReadOnly] public ComponentLookup<BulletData> PlayerProjectileDataLookup;
    [ReadOnly] public ComponentLookup<EnemyTag> EnemyLookup;
    public EntityCommandBuffer Ecb;

    public void Execute(TriggerEvent triggerEvent){
        Entity entityA = triggerEvent.EntityA;
        Entity entityB = triggerEvent.EntityB;

        bool isBodyAProjectile = PlayerProjectileLookup.HasComponent(entityA);
        bool isBodyBProjectile = PlayerProjectileLookup.HasComponent(entityB);
        bool isBodyAEnemy = EnemyLookup.HasComponent(entityA);
        bool isBodyBEnemy = EnemyLookup.HasComponent(entityB);

        if (isBodyAProjectile && isBodyBEnemy)
        {
            ApplyDamage(entityB, entityA);
            Debug.Log("Entrei colisão 1");
        }
        else if (isBodyBProjectile && isBodyAEnemy)
        {
            ApplyDamage(entityA, entityB);
            Debug.Log("Entrei colisão 2");
        }
    }

    private void ApplyDamage(Entity enemyEntity, Entity projectileEntity){
        EnemyLife health = EnemyLifeLookup[enemyEntity];
        health.lifeValue -= PlayerProjectileDataLookup[projectileEntity].damage;
        EnemyLifeLookup[enemyEntity] = health;
        Ecb.DestroyEntity(projectileEntity);
    }
}