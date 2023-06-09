﻿using System;
using Cysharp.Threading.Tasks;
using RougeLike.Battle.Configs;
using System.Collections.Generic;
using CustomSerialize;
using RougeLike.Battle.Action;
using RougeLike.Util;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using RougeLike.Battle.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using RougeLike.Map.Config;
using RougeLike.Tool;

namespace RougeLike.Battle
{
	public class MonoECS : MonoBehaviour
	{
		public List<EntityBehave> AllBullets = new List<EntityBehave>();
		
		public static MonoECS instance;

		public BF_SetInteractiveShaderEffects bF;

		public System.Action frameAction;

		public Camera mainCamera;
		#region System

		public ConfigScene configScene;
		public ConfigProcess configProcess;
		EntityLocal m_EntityLocal;
		public EntityLocal entityLocal
		{
			get { return m_EntityLocal; }
		}

		List<ECSSystem> m_SystemList;
		List<EntityBehave> m_ToAdd = new List<EntityBehave>(), m_ToRemove = new List<EntityBehave>();
		public SystemInput systemInput;
		public SystemWeapon systemWeapon;
		public SystemPhysics systemPhysics;
		public SystemBullet systemBullet;
		public SystemProcess systemProcess;
		public SystemMonster systemMonster;
		public SystemTime systemTime;

		public float TimeScale => systemTime.timeScale;
		private void Start()
		{
			instance = this;
			mainCamera = Camera.main;
			m_EntityLocal = new EntityLocal();

			m_SystemList = new List<ECSSystem>();

			systemInput = new SystemInput();
			systemInput.Init(m_EntityLocal);

			systemWeapon = new SystemWeapon();
			systemPhysics = new SystemPhysics();
			systemBullet = new SystemBullet();
			systemProcess = new SystemProcess();
			systemMonster = new SystemMonster();
			systemTime = new SystemTime();
			systemProcess.Init(configProcess.processs);

			m_SystemList.Add(systemWeapon);
			m_SystemList.Add(systemPhysics);
			m_SystemList.Add(systemBullet);
			m_SystemList.Add(systemProcess);
			m_SystemList.Add(systemMonster);
			m_SystemList.Add(systemTime);
			InitECS();
			GameStart();
			m_Running = true;
		}

		private void FixedUpdate()
		{
			systemInput.FixedUpdate();
			if (!m_Running)
				return;

			for (int i = 0; i < m_SystemList.Count; i++)
			{
				m_SystemList[i].FixedUpdate();
			}
		}

		private void Update()
		{
			systemInput.Update();
			if (!m_Running)
				return;
			for (int i = 0; i < m_SystemList.Count; i++)
			{
				m_SystemList[i].Update();
			}
			foreach (var eb in m_ToAdd)
			{
				RegistEntity(eb);
			}
			m_ToAdd.Clear();

			foreach (var eb in m_ToRemove)
			{
				RemoveEntity(eb);
			}
			m_ToRemove.Clear();
			frameAction?.Invoke();
		}
		#endregion

		#region ecs

		List<EntityBehave> m_AllEntities;
		Dictionary<uint, EntityBehave> m_EntityDic;
		List<List<EntityBehave>> m_ArchitypeLists;
		List<Architype> m_Architypes;

		public Architype Behave { get; private set; }

		private List<EntityBehave> m_BehaveList;
		public List<EntityBehave> m_BulletList;
		public List<EntityBehave> m_TimeList;
		public List<EntityBehave> m_abilityList;
		public List<EntityBehave> m_AiList;
		// public List<Test> test;
		public List<EntityBehave> m_MonsterList;

		

		// public class Test
		// {
		// 	public Decimal a;
		// 	public Decimal b;
		// 	public Decimal c;
		// 	public Decimal d;
		// 	public Decimal e;
		// 	public Decimal f;
		// 	public Decimal g;
		// }
		public List<EntityBehave> GetAllEntities()
		{
			return m_AllEntities;
		}

		void InitECS()
		{
			m_AllEntities = new List<EntityBehave>();
			m_ArchitypeLists = new List<List<EntityBehave>>();
			m_Architypes = new List<Architype>();
			m_EntityDic = new Dictionary<uint, EntityBehave>();

			// (m_BehaveList, Behave) = RegistArchitype2(EntityBehave.ID, EntityBehave.Transform, EntityBehave.Time, EntityBehave.Input, EntityBehave.Motion, EntityBehave.Physics, EntityBehave.Effect);
			var bulletTuple = RegistArchitype2(EntityBehave.Transform, EntityBehave.Bullet, EntityBehave.Physics);
			m_BulletList = bulletTuple.list;
			bulletTuple.architype.OnAdd += OnArchitypeOnAdd;
			bulletTuple.architype.OnRemove += OnArchitypeOnRemove;
			// test = new List<Test>();
			// for (int i = 1; i <= 1000; i++)
			// {
			// 	test.Add(new Test());
			// }
			m_MonsterList = RegistArchitype(EntityBehave.Transform, EntityBehave.Monster, EntityBehave.Physics, EntityBehave.Time);
			//  = monsterTuple.list;
			// monsterTuple.architype.OnRemove += (e) => e.Release();
		}

		private async void OnArchitypeOnRemove(EntityBehave e)
		{
			var comp = e.compTransform.transform.GetComponent<MonoFireEffect>();
			comp.Stop();
			if (e.compBullet.config is ConfigCircleBullet)
				e.compBullet.bulletGroup.children.Remove(e);
			if (e.compBullet.config.useObjectPool)
			{
				var go = e.compTransform.transform.gameObject;
				var bulletName = e.compBullet.config.bulletName;
				await UniTask.Delay(5000);
				EntityPool.Instance.ReleaseBullet(go,bulletName);
			}
			else
			{
				Destroy(e.compTransform.transform.gameObject,5);
			}
			e.Release();
		}
		private void OnArchitypeOnAdd(EntityBehave e)
		{
			if (e.compBullet.config is ConfigCircleBullet)
			{
				e.compBullet.bulletGroup.children.Add(e);
			}
		}

		public List<EntityBehave> RegistArchitype(params int[] comps)
		{
			var res = new Architype(comps);
			return RegistArchitype(res);
		}

		public (List<EntityBehave> list, Architype architype) RegistArchitype2(params int[] comps)
		{
			var architype = new Architype(comps);
			var list = RegistArchitype(architype);
			return (list, architype);
		}
		List<EntityBehave> RegistArchitype(Architype a)
		{
			m_Architypes.Add(a);
			var newList = new List<EntityBehave>();
			m_ArchitypeLists.Add(newList);

			foreach (var entity in m_AllEntities)
			{
				if (a.Is(entity))
					newList.Add(entity);
			}

			return newList;
		}

		public List<EntityBehave> GetAllMonster()
		{
			return m_MonsterList;
		}

		#endregion

		#region Pause暂停逻辑
		public bool CouldPause = true;
		public bool running
		{
			get
			{
				return m_Running;
			}
			protected set
			{
				if (!CouldPause)
				{
					m_Running = true;
					return;
				}
				m_Running = value;
			}
		}

		private bool m_Running;
		private int m_PauseRef = 0;
		public void PushPause()
		{
			m_PauseRef++;
			if (m_PauseRef > 0)
				running = false;
		}

		public void PopPause()
		{
			m_PauseRef--;
			if (m_PauseRef == 0)
				running = true;
		}
		#endregion

		#region InputManager

		void HandleButton(InputValue value, ref ButtonState state)
		{
			float f = value.Get<float>();
			state.currentFramePressed = f > 0.5f;
		}

		public void OnMove(InputValue value)
		{
			if (m_EntityLocal == null)
			{
				Debug.Log("为空");
				return;
			}
			var dir = value.Get<Vector2>();
			if (dir.magnitude > 0)
			{
				m_EntityLocal.compLocalInput.move.pressed = true;
				m_EntityLocal.compLocalInput.move.dir = dir;
			}
			else
			{
				m_EntityLocal.compLocalInput.move.pressed = false;
			}
		}

		public void OnDodge(InputValue value)
		{
			HandleButton(value, ref m_EntityLocal.compLocalInput.dodgeState);
		}

		public void OnWeapon(InputValue value)
		{
			if (!m_EntityLocal.compLocalInput.disableWeapon)
				HandleButton(value, ref m_EntityLocal.compLocalInput.weaponState);
		}
		public void OnSwitchWeapon1(InputValue value)
		{
			if (!m_EntityLocal.compLocalInput.disableSwitchWeapon1)
				HandleButton(value, ref m_EntityLocal.compLocalInput.switchWeapon1State);
		}

		public void OnSwitchWeapon2(InputValue value)
		{
			if (!m_EntityLocal.compLocalInput.disableSwitchWeapon2)
				HandleButton(value, ref m_EntityLocal.compLocalInput.switchWeapon2State);
		}

		public void OnSwitchWeapon3(InputValue value)
		{
			if (!m_EntityLocal.compLocalInput.disableSwitchWeapon3)
				HandleButton(value, ref m_EntityLocal.compLocalInput.switchWeapon3State);
		}

		public void OnSwitchWeapon4(InputValue value)
		{
			if (!m_EntityLocal.compLocalInput.disableSwitchWeapon4)
				HandleButton(value, ref m_EntityLocal.compLocalInput.switchWeapon4State);
		}

		public void OnEsc(InputValue value)
		{
			HandleButton(value, ref m_EntityLocal.compLocalInput.esc);
		}
		#endregion

		#region GameStart

		async void GameStart()
		{
			await LoadGame();
		}

		async UniTask LoadGame()
		{
			//生成场景
			CreateScene();
			//生成角色
			CreateCharacter();
		}

		public void CreateScene()
		{
			foreach (var config in configScene.configs)
			{
				switch (config.method)
				{
					case ConfigGameObject.GenMethod.RandomSort:
						List<bool> positions = new List<bool>();
						for (int i = 0; i < 500 * 500; i++)
						{
							positions.Add(i<config.num);
						}
						positions.RandomSort();
						for (int i = 0; i < 500 * 500; i++)
						{
							if (positions[i])
							{
								var go = GameObject.Instantiate(config.gameObject);
								go.transform.position = new Vector3(i % 500, 0, i / 500);
							}
						}
						break;
					case ConfigGameObject.GenMethod.RandomWalk:
						var mapRandomWalk = GameUtil.RandomWalk(new Vector2Int(0, 0), new Vector2Int(500,500),config.num, 500);
						foreach (var pos in mapRandomWalk)
						{
							var go = GameObject.Instantiate(config.gameObject);
							go.transform.position = new Vector3(pos.x, 0, pos.y);
						}
						break;
					case ConfigGameObject.GenMethod.GeometricSegmentation:
						List<Vector2Int> mapGeometricSegmentation = new List<Vector2Int>();
						Vector3Int originStart = new Vector3Int(125,125,0);
						Vector2Int originBorder = new Vector2Int(500, 500);
						GameUtil.GeometricSegmentation(Vector3.zero, originStart, originBorder, config.num,
							mapGeometricSegmentation, 0,0);
						foreach (var pos in mapGeometricSegmentation)
						{
							var go = GameObject.Instantiate(config.gameObject);
							go.transform.position = new Vector3(pos.x, 0, pos.y);
						}
						break;
				}
			}
		}
		public void CreateCharacter()
		{
			mainEntity = SpawnEntity.Instance.SpawnCharacterEntity(character);
			bF.transformToFollow = mainEntity.entity.compTransform.transform;
			battlemain.Init(mainEntity.entity);
			systemInput.InputState = systemInput.battleInput;
		}
		

		public void EnqueueBehave(EntityBehave eb)
		{
			m_EntityDic[eb.oid] = eb;
			m_ToAdd.Add(eb);
		}

		public void RegistEntity(EntityBehave entity)
		{
			m_AllEntities.Add(entity);

			for (int i = 0; i < m_ArchitypeLists.Count; i++)
			{
				if (m_Architypes[i].Is(entity))
				{
					m_ArchitypeLists[i].Add(entity);
					m_Architypes[i].OnAdd?.Invoke(entity);
				}
			}
		}

		public void EnqueueRemove(EntityBehave eb)
		{
			m_EntityDic.Remove(eb.oid);
			// RemoveEntity(eb);
			m_ToRemove.Add(eb);
		}

		public void RemoveEntity(EntityBehave entity)
		{
			m_AllEntities.Remove(entity);

			for (int i = 0; i < m_ArchitypeLists.Count; i++)
			{
				if (m_Architypes[i].Is(entity))
				{
					m_ArchitypeLists[i].Remove(entity);
					m_Architypes[i].OnRemove?.Invoke(entity);
				}
			}
		}

		#endregion

		#region Entity
		public ConfigCharacter character;
		public MonoEntity mainEntity;
		#endregion

		#region DramgaCalculate
		/// <summary>
		/// 伤害公式
		/// </summary>
		/// <param name="owner"></param>
		/// <param name="target"></param>
		public void DamageCalculate(EntityBehave owner, EntityBehave target)
		{
			if (target.compMonster.m_dead)
				return;
			#region 暴击率

				var compBulletOwner = owner.compBullet.owner;
			int criticalBonus = (int)(100 * compBulletOwner.compCharacter.criticalBonus);
			var criticalMul = 1f;
			var isCritical = false;
			if (GameUtil.GetRandWithPossibility(new [] { criticalBonus, 100 - criticalBonus }, 100) == 0)
			{
				//暴击了
				criticalMul += (1 + compBulletOwner.compCharacter.criticalDamageBonus);
				isCritical = true;
			}
			#endregion
			//最终伤害
			var bulletDamage = owner.compBullet.damage * criticalMul * (1 + mainEntity.entity.compCharacter.damageBonus);

			#region 播放伤害跳字
			var memory = new Memory() { caster = owner,target = target,damage = (int)bulletDamage,isCritical = isCritical };
			ActionPlayParticle playParticle = new ActionPlayParticle();
			playParticle.Do(memory);

			var dir = target.compTransform.transform.position - mainEntity.entity.compTransform.transform.position;
			target.compPhysic.rigidbody.AddForce(dir.normalized * 1000);
			#endregion

			//目标扣血
			target.compMonster.Hurt(bulletDamage);
		}

		#endregion

		#region OpenUI

		public List<ConfigSkillPool> skillPool = new List<ConfigSkillPool>();
		private List<ConfigSkill> configSkills = new List<ConfigSkill>();
		public MonoSkillPanel skillPanel;
		public void OpenSkillChoose()
		{
			configSkills.Clear();
			var skillConfig = skillPool[0];
			var skills = skillConfig.configSkills;
			var skillNum = skills.Count;
			var canUpSkill = new List<ConfigSkill>();
			for (int i = 0; i < skillNum; i++)
			{
				var skill = skills[i];
				if (mainEntity.entity.compSkill.GetSkillLevel(skill.uid) < skill.GetLevelInfo() - 1)
					canUpSkill.Add(skill);	
			}
			GameUtil.RandomSort(canUpSkill);
			int num = canUpSkill.Count >= 3 ? 3 : canUpSkill.Count;

			for(int i = 0; i < num; i++)
            {
				configSkills.Add(canUpSkill[i]);
			}
			if(configSkills.Count != 0)
            {
				PushPause();
				skillPanel.gameObject.SetActive(true);
				battlemain.gameObject.SetActive(false);
				skillPanel.Init(configSkills);
			}
		}

		public void RandomRiseSkill()
        {
			var skillConfig = skillPool[0];
			var skills = skillConfig.configSkills;
			var skillNum = skills.Count;
			var canUpSkill = new List<ConfigSkill>();
			for (int i = 0; i < skillNum; i++)
			{
				var skill = skills[i];
				if (mainEntity.entity.compSkill.GetSkillLevel(skill.uid) < skill.GetLevelInfo() - 1)
					canUpSkill.Add(skill);
			}
			if (canUpSkill.Count > 0)
			{
				var index = Random.Range(0, canUpSkill.Count);
				mainEntity.entity.compSkill.AddSkill(canUpSkill[index]);
			}
			
		}

		public AttributeShow attributeView;

		public void OpenAttribute()
        {
			var state = attributeView.gameObject.activeInHierarchy;
			if(state)
            {
				PopPause();
			}
			else
            {
				PushPause();
			}
			battlemain.gameObject.SetActive(state);
			attributeView.gameObject.SetActive(!state);
			attributeView.OpenView(mainEntity.entity);
		}

		public MonoBattleMain battlemain;
		public int killAllNum = 0;
		public void AddKillNum() => killAllNum++;

		public MonoTreasureInfo treasureInfo;
		public async void OpenTreasure(int type ,string str)
        {
			treasureInfo.gameObject.SetActive(true);
			PushPause();
			treasureInfo.SetInfo(type,str);
			await UniTask.Delay(2000);
			PopPause();
			treasureInfo.gameObject.SetActive(false);
		}

		public MonoGameEnd gameEnd;

		public void GameEnd(bool isVectory)
        {
			PushPause();
			gameEnd.gameObject.SetActive(true);
			if (isVectory)
            {
				gameEnd.SetInfo("游戏胜利",battlemain.GameTime,killAllNum);
            }
            else
            {
				gameEnd.SetInfo("游戏结束", battlemain.GameTime, killAllNum);
			}
        }
		#endregion

		public void EscPressed()
		{
			if (gameEnd.gameObject.activeInHierarchy)
            {
				GameSave.AddXML(killAllNum,battlemain.GameTime);
				Destroy(GameObject.Find("MonoLoginView"));
				SceneManager.LoadSceneAsync(0);
            }
		}
		public CinemachineImpulseSource shake;
		public void CameraShake()
        {
			shake.GenerateImpulseAt(mainEntity.entity.compTransform.position,Vector3.one);
		}
	}
}

