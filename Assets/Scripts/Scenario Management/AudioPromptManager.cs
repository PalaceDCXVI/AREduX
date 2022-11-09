using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPromptManager : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip StartingClip;

    public float startingClipDelay = 10.0f;
    private bool startingClipHasPlayed = false;
    private bool tasksUnderway = false;

    public AudioClip UtencilPlacementClip;

    public AudioClip SandwichmakingPrompt;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ScenarioManager.Instance.ScenarioHasStarted)
        {
            startingClipDelay -= Time.deltaTime;
            if (startingClipDelay <= 0.0f && !startingClipHasPlayed)
            {
                audioSource.Stop();

                audioSource.clip = StartingClip;

                audioSource.Play();

                startingClipHasPlayed = true;
            }

            if (startingClipDelay <= -StartingClip.length - 1.0f && !tasksUnderway)
            {
                ScenarioManager.Instance.currentTask.StartTask();
                tasksUnderway = true;
            }
        }
    }

    public void PlayAudioClip(AudioClip audioClip)
    {
        audioSource.Stop();

        audioSource.clip = audioClip;

        audioSource.Play();
    }

}
