using System.Collections.Generic;
using UnityEngine;

namespace RougeLike.Map
{
	public enum BoxType
	{
		None = 0,
		Spring,
		Summer,
		Autumn,
		Winter,
		During
	}
	public class RougeLikeBox
	{
		public Vector3 size => go.GetComponent<BoxCollider>().bounds.size;
		public BoxType type;
		public Vector3 position;
		public GameObject go;
	}
	class RogueLikeRoom : RougeLikeBox
	{
		public List<RogueLikeRoom> nearestRooms = new List<RogueLikeRoom>();//最近的四个房间
		public RogueLikeRoom(Vector3 position)
		{
			this.position = position;
		}
	}

	class RougeLikeWall : RougeLikeBox
	{
	}

	class RougeLikeRoad : RougeLikeBox
	{
		public RogueLikeRoom roomPre;
		public RogueLikeRoom roomPost;
		public List<Vector3> connects;
	}
}