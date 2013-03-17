using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace BattleSiteE.GameObjects.Managers
{
    enum Fade { NONE, IN, OUT }
    public class MusicManager
    {
        //SINGLETON
        public static MusicManager instance;
        public static MusicManager Instance
        {
            get
            {
                if (instance == null) instance = new MusicManager();
                return instance;
            }
        }

        private DateTime fadestartTime;
        private int fadelength;
        private Fade fade = Fade.NONE;
        private Song nextTrack = null;
        private bool prevTrack = false;
        private float maxvolume = 0.5f;

        private Dictionary<String, Song> loadedTracks;

        public MusicManager()
        {
            loadedTracks = new Dictionary<string, Song>();
            nextTrack = null;
            fade = Fade.NONE;
        }

        // Add a new preloaded track, 
        public void addTrack(String name, Song track)
        {
            if (loadedTracks.ContainsKey(name)) return;
            loadedTracks.Add(name, track);
        }

        // start playing a track
        public void startTrack(String trackname, int fadeMS)
        {
            if (loadedTracks.ContainsKey(trackname))
            {
                // set to next track
                nextTrack = loadedTracks[trackname];
                fadelength = fadeMS;
                fadestartTime = DateTime.Now;

                // if a previous track exists, fade out
                if (prevTrack)
                {                    
                    fade = Fade.OUT;
                }
                else
                {
                    // otherwise start fade in
                    fade = Fade.IN;
                    MediaPlayer.Play(nextTrack);
                    MediaPlayer.IsRepeating = true;
                    nextTrack = null;
                    prevTrack = true;
                }
            }
        }

        public void stop(int fadeMS)
        {
            fadelength = fadeMS;
            fadestartTime = DateTime.Now;
            fade = Fade.OUT;
        }

        // THE GREAT MUSICAL UPDATE LOOP
        public void Update()
        {
            // If fading out
            if (fade == Fade.OUT)
            {
                DateTime now = DateTime.Now;
                // calculate the volume value
                float v = maxvolume - (float)((now - fadestartTime).TotalMilliseconds / fadelength) * maxvolume;
                MediaPlayer.Volume = v;
                if (v <= 0.0f)
                {
                    // stop fade
                    if (nextTrack == null)
                    {
                        fade = Fade.NONE;
                    }
                    else
                    {
                        // start fade in
                        fade = Fade.IN;
                        MediaPlayer.Play(nextTrack);
                        MediaPlayer.IsRepeating = true;
                        nextTrack = null;
                        fadestartTime = DateTime.Now;
                    }                    
                }

            }
            // otherwise if fading in
            else if (fade == Fade.IN)
            {
                DateTime now = DateTime.Now;
                float v = (float)((now - fadestartTime).TotalMilliseconds / fadelength) * maxvolume;
                MediaPlayer.Volume = v;

                if (v >= maxvolume)
                {
                    fade = Fade.NONE;
                }
            }

            
        }

        

    }
}
