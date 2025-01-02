using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class BlackHoleSphere : MonoBehaviour
{
    public float blackHoleMax;

    private Material mat;
	private Transform m_tansform;
    
    void Start()
	{
		mat = GetComponent<MeshRenderer>().sharedMaterial;
		m_tansform = transform;

        StartCoroutine(BlackHoleGrowth());
    }

    private IEnumerator BlackHoleGrowth()
    {
        while (true)
        {
            if (transform.localScale.x < blackHoleMax)
            {
                transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                yield return new WaitForSeconds(2.0f);
            }

            else
            {
                transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
                yield return new WaitForSeconds(15.0f);
            }
        }
    }

    void Update()
	{
		mat.SetVector("_Center", new Vector4(m_tansform.position.x, m_tansform.position.y, m_tansform.position.z, 1f));
	}
}
