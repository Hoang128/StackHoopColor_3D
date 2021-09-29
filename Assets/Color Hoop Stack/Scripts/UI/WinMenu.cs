using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinMenu : MonoBehaviour
{
    public Animator animator;
    public GameObject backgroundPanel;
    public ParticleSystem lFirework;
    public ParticleSystem rFirework;

    private void OnEnable()
    {
        StartCoroutine(PlayFireWorkAfter(lFirework, 0f));
        StartCoroutine(PlayFireWorkAfter(rFirework, 0f));
        StartCoroutine(ResetFireWorkAfter(rFirework, 2f));
        animator.Play("Win Menu Start Anim");
        backgroundPanel.gameObject.SetActive(true);
        SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.WIN], false);
        SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.CHEER], false);
    }

    private void OnDisable()
    {
        backgroundPanel.gameObject.SetActive(false);
        GameplayMgr.Instance.GoToLevel(GameplayMgr.Instance.currentLevel + 1);
    }

    public void PlayEndAnimation()
    {
        lFirework.Stop(true);
        rFirework.Stop(true);
        animator.Play("Win Menu End Anim");
        SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.BUTTON], false);
    }

    public void DisableMenu()
    {
        gameObject.SetActive(false);
    }

    public IEnumerator PlayFireWorkAfter(ParticleSystem firework, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        firework.Play(true);
    }

    public IEnumerator ResetFireWorkAfter(ParticleSystem firework, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        firework.Stop(true);
        firework.Play(true);
    }
}
