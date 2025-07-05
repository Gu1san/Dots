using Unity.Entities;
using UnityEngine;

public class BulletPrefabReference : MonoBehaviour
{
    public GameObject bulletPrefab;

    public class Baker : Baker<BulletPrefabReference>
    {
        public override void Bake(BulletPrefabReference authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new BulletPrefab
            {
                Value = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic)
            });
        }
    }
}

public struct BulletPrefab : IComponentData
{
    public Entity Value;
}
