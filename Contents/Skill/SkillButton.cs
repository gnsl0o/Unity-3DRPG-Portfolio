using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public SkillSO skill;

    public SkillManager player;

    public Image imgIcon;

    public Image imgCool;

    public PlayerContext playerContext;

    private void Start()
    {
        imgIcon.sprite = skill.Icon;

        imgCool.fillAmount = 0;
    }

    public void OnClicked()
    {
        // if (playerContext.actionStateMachine.CurrentState is not AttackingState || imgCool.fillAmount > 0) return;

        player.ActivateSkill(skill);

        StartCoroutine(SC_Cool());
    }

    IEnumerator SC_Cool()
    {
        float tick = 1f / skill.cool;
        float t = 0.2f;

        imgCool.fillAmount = 1;

        while(imgCool.fillAmount > 0)
        {
            imgCool.fillAmount = Mathf.Lerp(1, 0, t);
            t += (Time.deltaTime * tick);

            yield return null;
        }
    }
}
