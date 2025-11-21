using System.Collections.Generic;
using UnityEngine;

namespace Script.Core.Entity
{
    /// <summary>
    /// 实体管理器
    /// </summary>
    public class EntityManager : Singleton<EntityManager>
    {
        /// <summary>
        /// 所有的实体
        /// </summary>
        private Dictionary<enEntityType, List<BaseEntity>> _entities;

        /// <summary>
        /// 玩家操作的实体（主英雄）
        /// </summary>
        private BaseEntity _mainHero;

        public override void F_Init()
        {
            base.F_Init();
            _entities = new Dictionary<enEntityType, List<BaseEntity>>();
            for (int i = 0; i < (int)enEntityType.All; i++)
            {
                _entities.Add((enEntityType)i, new List<BaseEntity>());
            }
        }

        public void F_AddEntity(BaseEntity baseEntity)
        {
            enEntityType entityType = baseEntity.F_GetEntityType();
            if (!_entities[entityType].Contains(baseEntity))
            {
                _entities[entityType].Add(baseEntity);
                //这个英是不是玩家操作的英雄
                if (baseEntity is MainHero)
                {
                    _mainHero = baseEntity;
                }
            }
            else
            {
                Debug.LogError($"已经添加过相应的实例了,为什么现在还要添加进来.baseEntity:{baseEntity}");
            }
        }

        public void F_RemoveEntity(BaseEntity baseEntity)
        {
            enEntityType entityType = baseEntity.F_GetEntityType();
            if (_entities[entityType].Contains(baseEntity))
            {
                _entities[entityType].Remove(baseEntity);
            }
            else
            {
                Debug.LogError($"要移除的实现已经移除过了，为什么现在还要移除一次.baseEntity:{baseEntity}");
            }
        }

        /// <summary>
        /// 获取主控英雄
        /// </summary>
        /// <returns></returns>
        public BaseEntity F_GetMainHero()
        {
            return _mainHero;
        }

        /// <summary>
        /// 获取指定类型的实体列表
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public List<BaseEntity> F_GetEntityByType(enEntityType entityType)
        {
            return _entities[entityType];
        }

        public void F_Clear()
        {
            _mainHero = null;
            _entities.Clear();
        }
    }
}