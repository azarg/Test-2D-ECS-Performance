
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class EntityAuthoring : MonoBehaviour
{
    class Baker : Baker<EntityAuthoring>
    {
        public override void Bake(EntityAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EntityData { });
        }
    }
}


public struct EntityData : IComponentData
{
    public float2 direction;
}
