using UnityEngine;
using System.Collections;

//must be as same as the inspector show
public enum AUDIO_TYPE : int
{
	CLICK = 0,
	CLEAN,
}

public class AudioManager : MonoBehaviour {

	public AudioClip[] audioClip;
	private AudioSource audioSrc;

	// Use this for initialization
	void Start () 
	{
		audioSrc = GetComponent<AudioSource> ();
	}
	
	void PlaySound(AUDIO_TYPE type)
	{
		audioSrc.clip = audioClip [(int)type];
		audioSrc.Play ();
		//audioSrc.PlayOneShot (audioClip [(int)type]);
		//Debug.Log("PlaySound");
	}
}
