namespace RougeLike.Battle
{
	public class Architype
	{
		public int[] allOfIndices { get; private set; }
		public Architype(int[] all)
		{
			allOfIndices = all;
		}

		public bool Is(EntityBehave entity)
		{
			return entity.HasComponents(allOfIndices);
		}
		public System.Action<EntityBehave> OnAdd;
		public System.Action<EntityBehave> OnRemove;
	}
}
