using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ACT.Battle
{
    public enum ParticleConfig
    {
        Common,
        Anomaly,
        Buff,
        Count
    }

    public enum TextMessage
    {
        None,
        Parry //格挡
    }

    public struct SymbolsData
    {
        //配置
        [Tooltip("相对于实体后方偏移量")] public float offsetX;
        [Tooltip("相对于实体垂直偏移量")] public float offsetY;
        [Tooltip("相对于实体偏移量")] public float offsetZ;
        [Tooltip("暴击情况下缩放倍率")] public float criticalSize;
        [Tooltip("暴击情况下粒子配置")] public ParticleSystem criticalParticleConfig;
        [Tooltip("位数对应缩放倍率")] public List<float> digitsSize;

        //一组字符串按顺序排列，从左上角开始
        public char[] chars;

        //每个符号的坐标Dictionary 
        private Dictionary<char, Vector2> charsDict;

        [Range(1, 10)] public int row, column;

        //缩放粒子长度和宽度 用于对纹理缩放采样
        public Vector2 symbolScale;

        public void Initialize()
        {
            charsDict = new Dictionary<char, Vector2>();
            for (int i = 0; i < chars.Length; i++)
            {
                //符号坐标计算，更改符号编号
                //在数字行和列中，知道行的长度等于10
                var uv = new Vector2(i % column, (row - 1) - i / column);
                charsDict.Add(chars[i], uv);
            }
        }

        public Vector2 GetTextureCoordinates(char c)
        {
            if (charsDict.TryGetValue(c, out Vector2 texCoord))
                return texCoord;
            return Vector2.zero;
        }
    }

    public class ConfigColor
    {
        public Color color;
    }

    public class ConfigParticle
    {
        public ParticleSystem particleSystem;
        [Tooltip("是否自定义data，否者为默认data样式")] public bool isCustom;
        [ShowIf("isCustom")] public SymbolsData customData;
    }

    public class CmdDamageNumber
    {
        public int damage;
        public Vector3 position;
        public bool isCriticle;
    }

    public class TextParticleSystem : SerializedMonoBehaviour
    {
        // [BoxGroup("文本消息配置")] public Dictionary<TextMessage, int> textMessageIndexData; //该文本跳字对应的chars索引从左上角开始
        // [BoxGroup("文本消息配置")] public Dictionary<TextMessage, Color> textMessageColor;
        // [BoxGroup("文本消息配置")] public ConfigParticle configTextMessage;
        //
        // [BoxGroup("数字消息配置")] public Dictionary<ParticleConfig, ConfigParticle> configDamageMessage;
        //
        // [BoxGroup("数字消息配置")]
        //public Battle.Configs.ConfigColor configColor;
        public Dictionary<ParticleConfig, ConfigColor> configColorDic;

        [Tooltip("默认data样式")] public SymbolsData defaultData;

        public bool disable;

        //缓存的数据
        char[] m_show = new char[11];
        List<Vector4> m_CustomData = new List<Vector4>(20);
        Vector2[] m_TexCords = new Vector2[12];
        ParticleSystem m_Par;
        SymbolsData m_Data;

        private void Awake()
        {
            defaultData.Initialize();

            // for (int i = 0; i < (int)ParticleConfig.Count; i++)
            // {
            //     if (configDamageMessage[(ParticleConfig)i].isCustom)
            //         configDamageMessage[(ParticleConfig)i].customData.Initialize();
            // }
            //
            // if (configTextMessage.isCustom)
            //     configTextMessage.customData.Initialize();

            m_Data = defaultData;
            m_Par = defaultData.criticalParticleConfig;
        }

        [Button("Fire")]
        public void Test()
        {
            m_Data = defaultData;
            m_Data.Initialize();
            m_Par = defaultData.criticalParticleConfig;
            CmdDamageNumber cmd = new CmdDamageNumber();
            cmd.position = Vector3.up;
            cmd.damage = 14115;
            cmd.isCriticle = false;
            SpawnDamageTextParticle(cmd);
        }

        //生成文本字符跳字 每种字符写死 对应字符的textMessageIndexData固定
        // public void SpawnTextMessage(Vector3 position, TextMessage testMessage, bool faceLeft)
        // {
        //     if (configTextMessage.isCustom)
        //         m_Data = configTextMessage.customData;
        //     else
        //         m_Data = defaultData;
        //
        //     m_Par = configTextMessage.particleSystem;
        //
        //     switch (testMessage)
        //     {
        //         case TextMessage.Parry:
        //             SpawnParticle(
        //                 new Vector3(position.x + m_Data.offsetX * (faceLeft ? -1 : 1), position.y, m_Data.offsetZ),
        //                 textMessageIndexData[TextMessage.Parry],
        //                 textMessageColor[TextMessage.Parry], m_Data.digitsSize[0]);
        //             break;
        //     }
        // }

        //生成伤害数字
        public void SpawnDamageTextParticle(CmdDamageNumber cmd)
        {
            // if (cmd.damage == 0)
            // 	return;
            // if (cmd.entityType == EntityType.StateObject)
            // if (cmd.entityType == EntityType.StateObject)
            // 	return;

            //更新要使用的粒子系统和配置数据
            // if (configDamageMessage[cmd.particleConfig].isCustom)
            // 	m_Data = configDamageMessage[cmd.particleConfig].customData;
            // else
            // 	m_Data = defaultData;

            // if (cmd.isCriticle)
            //     m_Par = m_Data.criticalParticleConfig;
            // else
            // 	m_Par = configDamageMessage[cmd.particleConfig].particleSystem;

            //更新
            var color = cmd.isCriticle ? Color.red : Color.white;
            // ConfigColor configColor;
            // if (configColorDic.ContainsKey(cmd.particleConfig))
            // 	configColor = configColorDic[cmd.particleConfig];
            // else
            // 	configColor = configColorDic[ParticleConfig.Common];

            // switch (cmd.element)
            // {
            // 	case Core.Element.Normal:
            // 		color = configColor.normal;
            // 		break;
            // 	case Core.Element.Fire:
            // 		color = configColor.fire;
            // 		break;
            // 	case Core.Element.Ice:
            // 		color = configColor.ice;
            // 		break;
            // 	case Core.Element.Dark:
            // 		color = configColor.dark;
            // 		break;
            // 	case Core.Element.Electric:
            // 		color = configColor.electric;
            // 		break;
            // 	case Core.Element.Energy:
            // 		color = configColor.energy;
            // 		break;
            //
            // }

            int temp = cmd.damage;
            int digits = 0;

            while ((temp /= 10) >= 1)
                digits++;

            if (digits > m_Data.digitsSize.Count - 1)
                digits = m_Data.digitsSize.Count - 1;

            float size = m_Data.digitsSize[digits];
            if (cmd.isCriticle)
                size *= m_Data.criticalSize;


            SpawnParticle(new Vector3(cmd.position.x + m_Data.offsetX,m_Data.offsetY , cmd.position.z), cmd.damage,
                color, size);
        }

        // public void SpawnReCoverParticle(Vector3 position, int message, Color color, float startSize)
        // {
        //     m_Data = defaultData;
        //     m_Par = configDamageMessage[ParticleConfig.Common].particleSystem;
        //
        //     int temp = message;
        //     int digits = 0;
        //
        //     while ((temp /= 10) >= 1)
        //         digits++;
        //
        //     temp = message;
        //     for (int i = digits + 1; i > 0; i--)
        //     {
        //         m_show[i] = (char)(temp % 10 + 48);
        //         temp = temp / 10;
        //     }
        //
        //     m_show[0] = '+';
        //     FIreParticle(position, color, startSize, m_show, digits + 2);
        // }
        //
        // public void SpawnReduceParticle(Vector3 position, int message, Color color, float startSize)
        // {
        //     m_Data = defaultData;
        //     m_Par = configDamageMessage[ParticleConfig.Common].particleSystem;
        //
        //     int temp = message;
        //     int digits = 0;
        //
        //     while ((temp /= 10) >= 1)
        //         digits++;
        //
        //     temp = message;
        //     for (int i = digits + 1; i > 0; i--)
        //     {
        //         m_show[i] = (char)(temp % 10 + 48);
        //         temp = temp / 10;
        //     }
        //
        //     FIreParticle(position, color, startSize, m_show, digits + 2);
        // }

        void SpawnParticle(Vector3 position, int message, Color color, float startSize)
        {
            int temp = message;
            int digits = 0;

            while ((temp /= 10) >= 1)
                digits++;

            temp = message;
            for (int i = digits; i >= 0; i--)
            {
                m_show[i] = (char)(temp % 10 + 48);
                temp = temp / 10;
            }

            FIreParticle(position, color, startSize, m_show, digits + 1);
        }


        #region TextParticle

        /// <summary>
        /// 发射前要准备好m_Par 和 m_Data 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="startSize"></param>
        /// <param name="message"></param>
        /// <param name="len"></param>
        void FIreParticle(Vector3 position, Color color, float startSize, char[] message, int len)
        {
            if (disable)
                return;

            //初始化emmishen参数
            //颜色位置从方法参数中得到
            //系统将startSize3D引入x，以防止符号拉伸或收缩
            //更改消息长度

            var emitParams = new ParticleSystem.EmitParams
            {
                startColor = color,
                position = position,
                applyShapeToPosition = true,
                startSize3D = new Vector3(len * m_Data.symbolScale.x, 1 * m_Data.symbolScale.y, 1)
            };
            emitParams.startSize3D *= startSize * m_Par.main.startSizeMultiplier;

            m_Par.Emit(emitParams, 1);

            SetCustomParticle(message, len);
        }

        void SetCustomParticle(char[] message, int len)
        {
            // 12个Vector2 Pack到一个Vector4中 . 每3个Vector2，也就是6个float，pack到一个float中。
            for (int i = 0; i < len; i++)
            {
                //获得符号的位置
                m_TexCords[i] = m_Data.GetTextureCoordinates(message[i]);
            }

            for (int i = len; i < m_TexCords.Length - 1; i++)
            {
                m_TexCords[i] = Vector2.zero;
            }

            m_TexCords[m_TexCords.Length - 1] = new Vector2(0, len);

            Vector4 custom1Data = CreateCustomData(m_TexCords);

            //收到ParticleSystemCustomData流 ParticleSystem的Custom1
            int dataCount = m_Par.GetCustomParticleData(m_CustomData, ParticleSystemCustomData.Custom1);
            //改变最后一个元素的数据，即我们刚刚创造的粒子。
            m_CustomData[dataCount - 1] = custom1Data;
            //将数据返回到particlestem。
            m_Par.SetCustomParticleData(m_CustomData, ParticleSystemCustomData.Custom1);
        }

        //Vector2组包函数，在float中有符号坐标。
        float PackFloat(Vector2[] vecs)
        {
            if (vecs == null || vecs.Length == 0)
                return 0;
            var result = vecs[0].y * 10000 + vecs[0].x * 100000;
            if (vecs.Length > 1)
                result += vecs[1].y * 100 + vecs[1].x * 1000;
            if (vecs.Length > 2)
                result += vecs[2].y + vecs[2].x * 10;
            return result;
        }

        Vector2[] m_temp = new Vector2[3];

        /// <summary>
        /// 把12个UV pack 到1个Float4中
        /// </summary>
        /// <param name="texCoords"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private Vector4 CreateCustomData(Vector2[] texCoords)
        {
            var data = Vector4.zero;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var ind = i * 3 + j;
                    m_temp[j] = texCoords[ind];
                }

                data[i] = PackFloat(m_temp);
            }

            return data;
        }

        #endregion
    }
}