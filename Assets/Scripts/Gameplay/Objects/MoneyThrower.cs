using UnityEngine;

public class MoneyThrower : MonoBehaviour
{
    [SerializeField]
    private GameObject MoneyPrefab;
    [SerializeField]
    private GameObject MoneyVFX;

    [SerializeField]
    private int FixedSpawnAmount;
    [SerializeField]
    private int AmountModifier;

    private int spawnAmount;
    private int stepAmount;

    [SerializeField]
    private float UpForce;
    [SerializeField]
    private float SideForce;

    private Vector3 spawnForce;

    private GameObject spawnedMoney;

    private void Awake()
    {
        MoneyVFX.SetActive(false);
    }

    private void Start()
    {
        //SpawnMoney(304);
    }

    public void SpawnMoney(int amount)
    {
        MoneyVFX.SetActive(false);
        MoneyVFX.SetActive(true);

        if (amount % AmountModifier == 0)
        {
            spawnAmount = FixedSpawnAmount;
            stepAmount = amount / AmountModifier;
        }
        else
        {
            spawnAmount = FixedSpawnAmount + 1;
            stepAmount = Mathf.FloorToInt(amount / (float)AmountModifier);
        }

        spawnForce = Vector3.zero;
        for (int i = 0; i < spawnAmount; i++)
        {
            //spawnForce.x = Random.Range(-SideForce, SideForce);
            //spawnForce.y = Random.Range(UpForce / 2f, UpForce);
            //spawnForce.z = Random.Range(-SideForce, SideForce);

            //if (spawnForce.x > 0) spawnForce.x += SideForce;
            //else spawnForce.x -= SideForce;

            //if (spawnForce.z > 0) spawnForce.z += SideForce * 3f;
            //else spawnForce.z -= SideForce * 3f;

            spawnForce.x = Random.Range(-SideForce / 3f, SideForce / 3f);
            spawnForce.y = Random.Range(UpForce / 2f, UpForce);
            spawnForce.z = Random.Range(-SideForce / 2f, -SideForce);

            spawnedMoney = Instantiate(MoneyPrefab, transform.position, Random.rotation);
            spawnedMoney.GetComponent<Rigidbody>().velocity = spawnForce;
            spawnedMoney.GetComponent<ThrowMoney>().Amount = i != FixedSpawnAmount ? stepAmount : amount % AmountModifier;
        }
    }
}
