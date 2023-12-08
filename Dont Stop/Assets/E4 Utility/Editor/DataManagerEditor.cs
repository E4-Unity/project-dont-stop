using UnityEditor;

namespace E4.Utility
{
	public class DataManagerEditor : Editor
	{
		[MenuItem("DataManager/DeleteAll")]
		public static void DeleteAll()
		{
			DataManager.DeleteAllData();
		}
	}
}
