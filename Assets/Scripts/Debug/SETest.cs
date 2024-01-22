/* 制作日
*　製作者
*　最終更新日
*/

using UnityEngine;
using System.Collections;

public class SETest : MonoBehaviour
{

	[SerializeField]
	private AudioClip SE;
	private AudioSource AudioSource;

	private void Awake()
	{
		AudioSource = this.GetComponent<AudioSource>();
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			AudioSource.PlayOneShot(SE);
		}
	}

	private void OnDestroy()
	{
	}
}