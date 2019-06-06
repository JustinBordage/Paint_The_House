using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] protected AudioClip[] soundList;
    protected AudioSource[] channelList;

    //Volume Settings
    protected static float vol_Master = 0.5f;
    protected static float vol_SFX = 0.5f;

    void Start()
    {
        initAudio(0f);
    }

    protected void initAudio(float spatialSetting)
    {
        //Compiles the list audio source channels
        channelList = GetComponents<AudioSource>();

        //Applies some default sound settings
        //to all the Audio Source Channels
        foreach (AudioSource channel in channelList)
        {
            channel.spatialBlend = spatialSetting;
            channel.dopplerLevel = 0f;
        }
    }


    protected AudioSource getChannel()
    {
        //Searches for the first channel that isn't being used
        foreach (AudioSource channel in channelList)
        {
            if (!channel.isPlaying)
            {
                channel.clip = null;
                return channel;
            }
        }

        //If everything is working properly this should never be triggered
        Debug.LogWarning("\'" + name + "\': Unused AudioSource not found, Number Of Channels: " + channelList.Length);
        return null;
    }

    AudioClip getAudioClip(string callback)
    {
        //Retrieves the AudioClip based on the callback
        foreach (AudioClip clip in soundList)
        {
            if (clip.name == callback)
            {
                return clip;
            }
        }

        return null;
    }

    protected void playAudio(ref AudioSource channel, ref AudioClip clip)
    {
        if (channel != null && clip != null)
        {
            //Attaches the audio clip
            channel.clip = clip;

            //Sets the volume that the clip will play at
            channel.volume = vol_Master * vol_SFX;

            //Plays the audio clip
            channel.Play();
        }
    }

    public void playSFX(string callback)
    {
        //Retrieves the channel required to play the sound
        AudioSource channel = getChannel();

        //Retrieves the AudioClip to play based on the callback
        AudioClip clip = getAudioClip(callback);

        //Plays the audio
        playAudio(ref channel, ref clip);
    }
}
