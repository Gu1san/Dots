using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;
using Unity.Burst;
using Unity.Transforms;
using Unity.Physics;

[BurstCompile]
public partial struct BulletSystem : ISystem {

    private void OnUpdate(ref SystemState state){
        EntityManager entityManager = state.EntityManager;
        NativeArray<Entity> allEntities = entityManager.GetAllEntities();
        foreach(Entity entity in allEntities){
            if (entityManager.HasComponent<BulletComponent>(entity) && entityManager.HasComponent<BulletLifeTimeComponent>(entity)){
                LocalTransform bulletTransform = entityManager.GetComponentData<LocalTransform>(entity);
                BulletComponent bulletComponent = entityManager.GetComponentData<BulletComponent>(entity);
                bulletTransform.Position += bulletComponent.Speed * SystemAPI.Time.DeltaTime * bulletTransform.Right();
                entityManager.SetComponentData(entity, bulletTransform);
                BulletLifeTimeComponent bulletLifeTimeComponent = entityManager.GetComponentData<BulletLifeTimeComponent>(entity);
                bulletLifeTimeComponent.RemainingLifeTime -= SystemAPI.Time.DeltaTime;
                if (bulletLifeTimeComponent.RemainingLifeTime <= 0f){
                    entityManager.DestroyEntity(entity);
                    continue;
                }
                entityManager.SetComponentData(entity, bulletLifeTimeComponent);
                //Physics
                NativeList<ColliderCastHit> hits = new NativeList<ColliderCastHit>(Allocator.Temp);
                float3 point1 = new float3(bulletTransform.Position - bulletTransform.Right() * 0.15f);
                PhysicsWorld physicsWorld = SystemAPI.GetSingletonRW<PhysicsWorldSingleton>().ValueRW.PhysicsWorld;
                physicsWorld.SphereCastAll(point1, bulletComponent.Size/2.0f, float3.zero, 1f, ref hits, new CollisionFilter {
                    BelongsTo = (uint) CollisionLayer.Default,
                    CollidesWith = (uint) CollisionLayer.Wall
                });
                if (hits.Length > 0){
                    entityManager.DestroyEntity(entity);
                }
                hits.Dispose();
            }
        } 
    }
}

