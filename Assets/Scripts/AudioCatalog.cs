using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioCatalog", menuName = "AudioConfiguration/AudioSO")]
public class AudioCatalog : ScriptableObject
{
    public AudioClip backgroundMusic;
    public AudioClip diceImpact1;
    public AudioClip diceImpact2;
    public AudioClip dicexdice;
    public AudioClip dicexpad;
    public List<AudioClip> pawning;
}
