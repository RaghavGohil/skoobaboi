using UnityEngine;

public class FishGenerator : MonoBehaviour
{

    [SerializeField]
    Fish[] fishes;

    public static FishGenerator instance;

    void Awake()
    {
        instance = this;
    }

    public void SpawnFishes() // optimize
    {
        foreach(Fish fish in fishes)
        {
            for(int i = 0;i<fish.numFishes;i++)
            {

                float randX = Random.Range(-90,90); // hardcoded according to the boundary plane values
                float randZ = Random.Range(-90,90);

                // random y based on the fish levels

                float randomLevel;

                if(fish.levelGenerated.Length > 1)
                    randomLevel = fish.levelGenerated[Random.Range(0,fish.levelGenerated.Length-1)];
                else
                    randomLevel = fish.levelGenerated[0];

                float randY = -1*Random.Range((Level.instance.unit*(randomLevel-1f)),(Level.instance.unit*randomLevel));
                
                Vector3 position = new Vector3(randX,randY,randZ);

                float randRotY = Random.Range(0,350);

                Instantiate(fish.fishPrefab,position,Quaternion.Euler(fish.fishPrefab.transform.eulerAngles.x,randRotY,0f));
            
            }
        }
    }

}
