using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour {

	private Vector3 m_rand;
	private float m_prevT;
	// Use this for initialization
	void Start () {
		m_rand = Random.onUnitSphere;
	}
	
	// Update is called once per frame
	void Update () {

		if(Time.time - m_prevT > 10.0f)
		{
			m_rand = Random.onUnitSphere;
			m_prevT = Time.time;
		}

		transform.rotation *= Quaternion.AngleAxis( Mathf.PI * Time.deltaTime, m_rand );

	}
}
