using System.Collections.Generic;

namespace RougeLike.Battle
{
    public abstract class ECSSystem
    {
        public virtual void FixedUpdate() {}
        public virtual void PostFixedUpdate() {}
        public virtual void Update() {}
    }

    public abstract class EntitySystem : ECSSystem
    {
        protected List<EntityBehave> m_Entities;
    }
}