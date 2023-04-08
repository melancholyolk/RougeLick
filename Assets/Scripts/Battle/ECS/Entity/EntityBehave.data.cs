using Fundamental;

namespace RougeLike.Battle
{
	/// <summary>
	/// 专门用来存放静态方法和数据结构的地方
	/// </summary>
	public partial class EntityBehave
	{
		public enum EntityType
		{
			Null,
			Enemy,
			Player,
			Bullet
		}
		public EntityType entityType = EntityType.Null;
	}
}
