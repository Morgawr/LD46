using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{

    public AudioSource BaseMusic;
    public AudioSource CombatMusic;
    public AudioSource BossMusic;
    public AudioSource BossMusicIntro;
    public AudioSource EndMusic;
    public AudioSource MenuMusic;

    public AudioMixer BGMMixer;

    bool playMusic = true;

    bool isInTransition = false;
    bool isPlayingBase = true;
    bool isInBossBattle = false;
    bool stopTransition = false;

    [Range(0.0f, 1f)]
    public float BattleAndNormalBGMTransitionSpeed = .3f;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    bool UpdateAudioInTransition() {
        // Exit early
        if(stopTransition) {
            stopTransition = false;
            return false;
        }
        float step = BattleAndNormalBGMTransitionSpeed * Time.deltaTime;
        bool done = true;
        if(isPlayingBase) { // Lower combat + Raise base
            if(CombatMusic.volume > 0.0f) {
                CombatMusic.volume -= step;
                done = false;
            }
            if(BaseMusic.volume < 1.0f) {
                BaseMusic.volume += step;
                done = false;
            }
        } else { // Raise combat + Lower base
            if(BaseMusic.volume > 0.0f) {
                BaseMusic.volume -= step;
                done = false;
            }
            if(CombatMusic.volume < 1.0f) {
                CombatMusic.volume += step;
                done = false;
            }
        }
        CombatMusic.volume = Mathf.Clamp(CombatMusic.volume, 0, 1);
        BaseMusic.volume = Mathf.Clamp(BaseMusic.volume, 0, 1);
        if(done)
            isInTransition = false;
        return !done;
    }

    //public void ToggleAudio() {
    //    playMusic = !playMusic;

    //    if(!playMusic) {
    //        BGMMixer.SetFloat("MixerVolume", -80f); // silent
    //    } else {
    //        BGMMixer.SetFloat("MixerVolume", 0f); // normal volume
    //    }
    //}

    public void StopGameMusic() {
        BaseMusic.Stop();
        CombatMusic.Stop();
        BossMusic.Stop();
    }

    public void SetCombatMode(bool mode) {
        if((isPlayingBase && !mode) || (!isPlayingBase && mode) || isInBossBattle)
            return;
        isPlayingBase = !isPlayingBase;
        if(!isInTransition) {
            StartCoroutine(Timer.RefillableTicker(new Delegates.CheckAndMaybeContinueDel(UpdateAudioInTransition)));
        }
    }

    public void StartBossMusic() {
        if(isInBossBattle)
            return;

        BaseMusic.volume = 0;
        CombatMusic.volume = 0;
        BossMusicIntro.Play();
        BossMusic.PlayDelayed(BossMusicIntro.clip.length);
    }

    // We go back to normal playing
    public void StopBossMusic() {
        isInBossBattle = false;
        BaseMusic.volume = 1;
        CombatMusic.volume = 0;
        BossMusicIntro.Stop();
        BossMusic.Stop();
    }

    // Audio is not running at first, we only start once we hit a certain
    // checkpoint, so we start both tracks (combat is muted) when we trigger it.
    public void StartNormalTracks() {
        if(!BaseMusic.isPlaying)
            BaseMusic.Play();
        if(!CombatMusic.isPlaying) {
            CombatMusic.Play();
        }
    }

    public void StartEndgame() {
        BaseMusic.Stop();
        CombatMusic.Stop();
        if(!EndMusic.isPlaying)
            EndMusic.Play();
        BossMusic.Stop();
        BossMusicIntro.Stop();
        MenuMusic.Stop();
    }

    public void StartNormalGame(){
        MenuMusic.Stop();
    }

    public void StartMenu(){
        MenuMusic.Play();
    }
}
