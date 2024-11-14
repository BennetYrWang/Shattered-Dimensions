using System;
using Unity.VisualScripting;
using UnityEngine;

namespace BennetWang
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [Header("BGM")] [SerializeField] private AudioClip duelBgm;
        [SerializeField] private AudioClip duelChangedBgm ,finalBattalBgm;
        [Header("SFX")] [SerializeField] private AudioClip landing;
        [SerializeField] private float landingVolume;
        [SerializeField] private AudioClip step;
        [SerializeField] private float stepVolume;
        
        public bool SfxActive = true;
        public static SoundManager Instance { get; private set; }

        private void Start()
        {
            Instance = this;
            audioSource.clip = duelBgm;
            audioSource.Play();
            
            
        }


        public void PlayBackgroundMusic(BackgroundMusic music)
        {
            AudioClip clip = null;
            
            switch (music)
            {
                case BackgroundMusic.Duel:
                    clip = duelChangedBgm;
                    break;
                case BackgroundMusic.FinalBattle:
                    clip = finalBattalBgm;
                    break;
            }

            if (audioSource.clip == clip)
                return;
            
            audioSource.clip = clip;
            audioSource.Play();
        }

        public void PlaySoundEffect(Clip clip)
        {
            if (!SfxActive)
                return;
            AudioClip audioClip = null;
            float volume = 1f;
            
            switch (clip)
            {
                case Clip.FootStep :
                    break;
                case Clip.JumpLanding:
                    audioClip = landing;
                    volume = landingVolume;
                    break;
            }
            
            audioSource.PlayOneShot(audioClip, volume);
        }
        public enum Clip
        {
            FootStep,
            HitGround,
            JumpLanding
        }

        public enum BackgroundMusic
        {
            Duel,
            FinalBattle,
        }
    }
}