using System.Collections.Generic;
using RougeLike.Battle;
using RougeLike.Util;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle.ECS
{
    public class FindTarget
    {
        public enum Regex
        {
            Closest,
            Furthest,
            Random,
            All,
        }

        public EntityBehave.EntityType type;
        public Regex regex;
        public void FindTargets(List<EntityBehave> result)
        {
            switch (type)
            {
                case EntityBehave.EntityType.Bullet:
                    result.AddRange(MonoECS.instance.m_BulletList);
                    break;
                case EntityBehave.EntityType.Enemy:
                    result.AddRange(MonoECS.instance.m_MonsterList);
                    break;
                case EntityBehave.EntityType.Player:
                    result.Add(MonoECS.instance.mainEntity.entity);
                    break;
            }

            switch (regex)
            {
                case Regex.Closest:
                    result.Sort((x,y) =>
                    {
                        var position = MonoECS.instance.mainEntity.transform.position;
                        if (x.compTransform == null)
                            return -1;
                        if (y.compTransform == null)
                            return -1;
                        var xDis = Vector3.Distance(position,x.compTransform.position);
                        var yDis = Vector3.Distance(position,y.compTransform.position);
                        return (int)(xDis - yDis);
                    });
                    break;
                case Regex.Furthest:
                    result.Sort((x,y) =>
                    {
                        var position = MonoECS.instance.mainEntity.transform.position;
                        if (x.compTransform == null)
                            return -1;
                        if (y.compTransform == null)
                            return -1;
                        var xDis = (x.compTransform.position - position).sqrMagnitude;
                        var yDis = (y.compTransform.position - position).sqrMagnitude;
                        return yDis.CompareTo(xDis);
                    });
                    break;
                case Regex.Random:
                    result.RandomSort();
                    break;
            }
        }
    }
}