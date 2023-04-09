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
            SkillLevelUP = 2
        }

        public Animator animator;

        private async void AddRandomEffect(EntityBehave entity)
        {
            animator.SetBool("Open",true);
            await UniTask.Delay(1000);
            var random = Random.Range(0,3);
            var des = "";
            switch((Type)random)
            {
                case Type.RecoverHP:
                    entity.compCharacter.RecoverHP(30);
                    des = "恢复30点生命值ֵ";
                    break;
                case Type.AddForce:
                    entity.compCharacter.AddForce();
                    des = "永久增加20%伤害";
                    break;
                case Type.SkillLevelUP:
                    MonoECS.instance.RandomRiseSkill();
                    des = "随机对一个技能升一级";
                    break;
                // case Type.SlowMonster:
                //     MonoECS.instance.systemTime.SlowMonster();
                //     des = "���������ٶ�20S";
                //     break;
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
