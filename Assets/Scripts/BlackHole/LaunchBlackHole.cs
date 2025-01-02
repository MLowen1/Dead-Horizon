using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchBlackHole : MonoBehaviour
{
    public GameObject ToBeLaunched;
    public float LaunchVelocity = 100f;
    public int InstanceCount = 0;

    // Update is called once per frame

    private void Start()
    {
        StartCoroutine(LaunchingBlackhole());
    }

    public IEnumerator LaunchingBlackhole()
    {
        while (true)
        {
            yield return new WaitForSeconds(60f);
            for (int i = 0; i < InstanceCount; i++) 
            {
                
                float angle = i * (360 / InstanceCount);
                float Posx = transform.position.x + Mathf.Sin(angle * Mathf.Deg2Rad);
                float PosZ = transform.position.z + Mathf.Cos(angle * Mathf.Deg2Rad);

                Vector3 ProjVec = new Vector3(Posx,0,PosZ);
                Vector3 moveDir = (ProjVec - transform.position).normalized * LaunchVelocity;

                var proj = Instantiate(ToBeLaunched, transform.position, Quaternion.identity);
                Debug.Log("shooting towards" + moveDir);
                proj.GetComponent<Rigidbody>().velocity = moveDir;
            }
        }
    }
}
