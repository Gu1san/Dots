using Unity.Entities;
using UnityEngine;

public class BulletAuthoring : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;
    public float bulletDamage;

    class Baker : Baker<BulletAuthoring>
    {
        public override void Bake(BulletAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<PlayerProjectileTag>(entity);
            AddComponent(entity, new BulletData
            {
                Speed = authoring.speed,
                Lifetime = authoring.lifetime,
                damage = authoring.bulletDamage,
                TimeAlive = 0f
            });
        }
    }
}
public struct PlayerProjectileTag : IComponentData { }
