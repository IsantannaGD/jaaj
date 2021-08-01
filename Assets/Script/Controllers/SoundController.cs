﻿using UnityEngine;

public class SoundController : MonoBehaviour
{
    private FMOD.Studio.EventInstance soundInstance;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    // TitleButton Sounds Method //
    public void ConfigButtonSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/sfx/ui/sfx_ui_menu_config");
    }
    public void CredButtonSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/sfx/ui/sfx_ui_menu_creditos");
    }
    public void ExitButtonSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/sfx/ui/sfx_ui_menu_exit");
    }
    public void ContinueButtonSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/sfx/ui/sfx_ui_menu_continue");
    }
    public void PlayButtonSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/sfx/ui/sfx_ui_menu_play");
    }
    public void BackButton()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/sfx/ui/sfx_ui_menu_volume_menos");
    }

    // Fase01 Sounds Method //

    public void BackgroundMusic ()
    {
        
    }


    // Players Sounds Method //
    public void PlayerJump()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/sfx/player/sfx_player_jump");
    }
    public void PlayerDeath()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/sfx/player/sfx_player_death");
    }
    public void SkillPlayer()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/sfx/player/sfx_player_projetil");
    }
    public void DamgePlayer()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/sfx/player/sfx_player_dano");
    }
    public void SkillNyuPlayer()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/sfx/player/sfx_player_absorvendo");
    }
}
