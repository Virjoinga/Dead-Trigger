using UnityEngine;

[AddComponentMenu("Weapons/SpawnGobEffectBlood")]
public class SpawnGobEffectBlood : MonoBehaviour
{
	private GobEffectBlood blood;

	private float nextBloodTime;

	private void Start()
	{
		blood = GetComponent<GobEffectBlood>();
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void Update()
	{
		if ((bool)blood && !(nextBloodTime > Time.timeSinceLevelLoad))
		{
			nextBloodTime = Time.timeSinceLevelLoad + 1.5f;
			blood.SpawnBlood(new Vector2(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f)));
		}
	}
}
