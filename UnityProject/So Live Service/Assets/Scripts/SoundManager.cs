using NUnit.Framework;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    // References
    [SerializeField] private TMPro.TextMeshProUGUI narratorText;
    [SerializeField] private List<AudioClip> StartingClips = new List<AudioClip>();
    [SerializeField] private List<string> StartingAudioText = new List<string>();
    [SerializeField] private AudioSource audioSource;

    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(PlayStartingClips());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayStartingClips()
    {
        for (int i = 0; i < StartingClips.Count; i++)
        {
            // Play Audio with Text
            audioSource.clip = StartingClips[i];
            narratorText.text = StartingAudioText[i];
            audioSource.Play();

            yield return new WaitWhile(() => audioSource.isPlaying);

            // Clear Text
            narratorText.text = "";

            yield return new WaitForSeconds(2f);
        }
        
    }
}
