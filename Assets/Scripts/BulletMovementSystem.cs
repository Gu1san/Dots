using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections; // Para EntityCommandBuffer

public partial struct BulletMovementSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (bullet, transform, entity) in SystemAPI.Query<RefRW<BulletData>, RefRW<LocalTransform>>().WithEntityAccess())
        {
            float3 forward = math.forward(transform.ValueRO.Rotation);
            transform.ValueRW.Position += forward * bullet.ValueRO.Speed * deltaTime;

            bullet.ValueRW.TimeAlive += deltaTime;
            if (bullet.ValueRO.TimeAlive >= bullet.ValueRO.Lifetime)
            {
                ecb.DestroyEntity(entity);
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
