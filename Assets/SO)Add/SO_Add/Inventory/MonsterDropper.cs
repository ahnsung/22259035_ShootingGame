using UnityEngine;

public class MonsterDropper : MonoBehaviour
{
    [System.Serializable]
    public class DropTable
    {
        public ItemData itemData;
        [Range(0f, 1f)] public float dropRate = 0.5f;
        public int minCount = 1;
        public int maxCount = 1;
    }

    public GameObject dropPrefab;
    public DropTable[] dropTables;

    public void Drop()
    {
        if (dropPrefab == null || dropTables == null)
        {
            return;
        }
        foreach (DropTable table in dropTables)
        {
            if (table.itemData == null)
            {
                continue;
            }
            if (Random.value > table.dropRate)
            {
                continue;
            }
            GameObject dropObject = Instantiate(
    dropPrefab, transform.position, Quaternion.identity);

            DropItem dropitem = dropObject.GetComponent<DropItem>();

            if ((dropitem != null))
            {
                {
                    dropitem.itemData = table.itemData;
                    dropitem.count = Random.Range(table.minCount, table.maxCount + 1);
                }
            }
        }
    }
}
