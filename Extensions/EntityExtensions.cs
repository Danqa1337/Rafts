using System;
using System.Collections.Generic;
using Unity.Entities;

public static class EntityExtensions
{

    public static bool HasComponent<T>(this Entity entity)
    {
        return World.DefaultGameObjectInjectionWorld.EntityManager.HasComponent<T>(entity);
    }
    public static bool HasBuffer<T>(this Entity entity) where T : struct, IBufferElementData
    {
        return World.DefaultGameObjectInjectionWorld.EntityManager.HasComponent<T>(entity);
    }
    public static bool Exists(this Entity entity)
    {
        return World.DefaultGameObjectInjectionWorld.EntityManager.Exists(entity);
    }
    public static bool Contans<T>(this DynamicBuffer<T> buffer, T item) where T : struct
    {
        for (int i = 0; i < buffer.Length; i++)
        {
            if (buffer[i].Equals(item))
            {
                return true;
            }
        }
        return false;
    }
    public static DynamicBuffer<T> AddBuffer<T>(this Entity entity) where T : struct, IBufferElementData
    {
        return World.DefaultGameObjectInjectionWorld.EntityManager.AddBuffer<T>(entity);
    }
    public static DynamicBuffer<T> GetBuffer<T>(this Entity entity) where T : struct, IBufferElementData
    {
        return World.DefaultGameObjectInjectionWorld.EntityManager.GetBuffer<T>(entity);
    }
    public static T GetComponentObject<T>(this Entity entity)
    {

        return World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentObject<T>(entity);
    }
    public static T GetComponentData<T>(this Entity entity) where T : struct, IComponentData
    {
        if (entity.HasComponent<T>())
        {
            return World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<T>(entity);

        }
        throw new NullReferenceException("no such component on " + entity);
    }

    public static ArchetypeChunk GetChunk(this Entity entity)
    {
        return World.DefaultGameObjectInjectionWorld.EntityManager.GetChunk(entity);
    }

    public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> list)
    {
        foreach (var item in list)
        {
            set.Add(item);
        }
    }
    public static void SetComponentData<T>(this Entity entity, T data) where T : struct, IComponentData
    {
        if (entity == Entity.Null) throw new System.NullReferenceException();
        if (entity.HasComponent<T>())
        {
            World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(entity, data);
        }
        else
        {
            World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData(entity, data);
        }
    }
    public static void SetZeroSizedTagComponentData<T>(this Entity entity, T data) where T : struct, IComponentData
    {
        if (entity == Entity.Null) throw new System.NullReferenceException();

        World.DefaultGameObjectInjectionWorld.EntityManager.RemoveComponent<T>(entity);
        World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData(entity, data);

    }
    public static void SetSharedComponentData<T>(this Entity entity, T data) where T : struct, ISharedComponentData
    {
        if (entity == Entity.Null) throw new System.NullReferenceException();

        World.DefaultGameObjectInjectionWorld.EntityManager.AddSharedComponentData(entity, data);

    }
    public static void SetChunkComponentData<T>(this Entity entity, T data) where T : struct, IComponentData
    {
        if (entity == Entity.Null) throw new System.NullReferenceException();

        World.DefaultGameObjectInjectionWorld.EntityManager.SetChunkComponentData(GetChunk(entity), data);

    }

    public static void AddComponentData<T>(this Entity entity, T data) where T : struct, IComponentData
    {
        if (entity == Entity.Null) throw new NullReferenceException();
        World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData(entity, data);
    }
    public static void AddComponentData(this Entity entity, IComponentData d)
    {
        World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData(entity, d);
    }
    public static void AddBufferElement<T>(this Entity entity, T d) where T : struct, IBufferElementData
    {
        if (!entity.HasBuffer<T>())
        {
            entity.AddBuffer<T>();
        }
        World.DefaultGameObjectInjectionWorld.EntityManager.GetBuffer<T>(entity).Add(d);
    }

    public static void RemoveBuffer<T>(this Entity entity) where T : struct, IBufferElementData
    {
        World.DefaultGameObjectInjectionWorld.EntityManager.RemoveComponent<T>(entity);
    }

    public static void AddComponentObject(this Entity entity, object obj)
    {
        if (entity == Entity.Null) throw new NullReferenceException();

        World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentObject(entity, obj);
    }
    public static void RemoveComponent<T>(this Entity entity) where T : struct, IComponentData
    {
        if (entity.HasComponent<T>())
        {
            World.DefaultGameObjectInjectionWorld.EntityManager.RemoveComponent<T>(entity);
        }
    }
    public static void RemoveZeroSizedComponent<T>(this Entity entity) where T : struct, IComponentData
    {

        World.DefaultGameObjectInjectionWorld.EntityManager.RemoveComponent<T>(entity);

    }

    public static void Remove<T>(this DynamicBuffer<T> list, T element) where T : struct
    {
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].Equals(element))
            {
                list.RemoveAtSwapBack(i);
            }
        }
    }
    public static void SetName(this Entity entity, string name)
    {
#if(UNITY_EDITOR)
        World.DefaultGameObjectInjectionWorld.EntityManager.SetName(entity, name);
#endif
    }
    public static void Destroy(this Entity entity)
    {
        World.DefaultGameObjectInjectionWorld.EntityManager.DestroyEntity(entity);
    }

}
