using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMaker : MonoBehaviour {
    public static SoundMaker Instance
    {
        get { return instance; }
    }
    private static SoundMaker instance;

    public List<AudioSource> audioSources = new List<AudioSource>();
    int audioSourceIndex = 0;

    [System.Serializable]
    public class SoundEntry
    {
        public string name;
        public List<AudioClip> clips = new List<AudioClip>();
    }
    public List<SoundEntry> soundEntries = new List<SoundEntry>();
    Dictionary<string, List<AudioClip>> nameToClip = new Dictionary<string, List<AudioClip>>();

    void Awake()
    {
        soundEntries.ForEach(s => nameToClip[s.name] = s.clips);

        instance = this;
    }

    public void PlaySound(string name)
    {
        GetSource().PlayOneShot(GetClipForName(name));
    }

    private AudioClip GetClipForName(string name)
    {
        var clips = nameToClip[name];
        return clips[Random.Range(0, clips.Count)];
    }

    private AudioSource GetSource()
    {
        audioSourceIndex++;
        audioSourceIndex %= audioSources.Count;

        return audioSources[audioSourceIndex];
    }
}
