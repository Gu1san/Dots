using UnityEngine;
using Unity.Entities;
public class PTAU : MonoBehaviour
{

    class Baker : Baker<PTAU>{
        public override void Bake(PTAU authoring){
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<playerTag>(entity);
        }
    }

}
public struct playerTag : IComponentData { }
