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

        public void addTrack(String name, Song track)
        {
            Debug.WriteLine("Adding " + name);
            if (loadedTracks.ContainsKey(name)) return;
            loadedTracks.Add(name, track);
        }

        public void startTrack(String trackname, int fadeMS)
        {
            Debug.WriteLine("Trying " + trackname);
            if (loadedTracks.ContainsKey(trackname))
            {
                Debug.WriteLine("Track "+ trackname + " found. playing");
                nextTrack = loadedTracks[trackname];
                fadelength = fadeMS;
                fadestartTime = DateTime.Now;

                if (prevTrack)
                {                    
                    fade = Fade.OUT;
                }
                else
                {
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

        public void Update()
        {
            if (fade == Fade.OUT)
            {
                DateTime now = DateTime.Now;
                float v = maxvolume - (float)((now - fadestartTime).TotalMilliseconds / fadelength) * maxvolume;
                MediaPlayer.Volume = v;
                if (v <= 0.0f)
                {
                    if (nextTrack == null)
                    {
                        fade = Fade.NONE;
                    }
                    else
                    {
                        fade = Fade.IN;
                        MediaPlayer.Play(nextTrack);
                        MediaPlayer.IsRepeating = true;
                        nextTrack = null;
                        fadestartTime = DateTime.Now;
                    }
                    
                }
                Debug.WriteLine(v);

            }

            else if (fade == Fade.IN)
            {
                DateTime now = DateTime.Now;
                float v = (float)((now - fadestartTime).TotalMilliseconds / fadelength) * maxvolume;
                MediaPlayer.Volume = v;

                if (v >= maxvolume)
                {
                    fade = Fade.NONE;
                }
                Debug.WriteLine(v);
            }

            
        }

        

    }
}
