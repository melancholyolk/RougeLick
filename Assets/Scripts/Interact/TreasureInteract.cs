using Cysharp.Threading.Tasks;
using RougeLike.Battle;
using UnityEngine;
namespace RougeLike.Interact
{
    public class TreasureInteract : MonoBehaviour
    {
        public enum Type
        {
            RecoverHP = 0,
            AddForce = 1,
            SkillLevelUP = 3,
            SlowMonster = 4,
        }

        public Animator animator;

        private async void AddRandomEffect(EntityBehave entity)
        {
            animator.SetBool("Open",true);
            await UniTask.Delay(1000);
            var random = Random.Range(0,4);
            var des = "";
            switch((Type)random)
            {
                case Type.RecoverHP:
                    entity.compCharacter.RecoverHP(30);
                    des = "回复30生命值";
                    break;
                case Type.AddForce:
                    entity.compCharacter.AddForce();
                    des = "永久提升20%伤害";
                    break;
                case Type.SkillLevelUP:
                    MonoECS.instance.RandomRiseSkill();
                    des = "随机技能提升一级";
                    break;
                case Type.SlowMonster:
                    MonoECS.instance.systemTime.SlowMonster();
                    des = "减慢敌人速度20S";
                    break;
            }
            MonoECS.instance.OpenTreasure(random, des);
            Destroy(this.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            var entity = other.GetComponent<MonoEntity>();
            if(entity != null && entity.entity != null)
            {
                if (entity.entity.entityType == Battle.EntityBehave.EntityType.Player)
                    AddRandomEffect(entity.entity);
            }
        }
    }
}
