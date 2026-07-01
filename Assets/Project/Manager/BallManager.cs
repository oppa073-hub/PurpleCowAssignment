using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    [SerializeField] private List<BallData> equippedBalls = new List<BallData>() ;
    [SerializeField] private Transform firePoint;
    [SerializeField] private PlayerShooter2D playerShooter;
    [SerializeField] private int ballCount;
    [SerializeField] private float fireInterval = 0.08f;
    [SerializeField] private PlayerSkillInventory inventory;
    private int nextBallIndex;

    private void Start()
    {
        StartCoroutine(FireInitialBalls());
    }
    private void FireOneBallImmediate() //현재 장착된 볼 데이터를 기반으로 볼 생성 및 발사
    {
        if (equippedBalls.Count <= 0) return;
        if (GameManager.Instance.CurrentState != GameState.Playing) return;

        BallData ballData = equippedBalls[nextBallIndex % equippedBalls.Count];

        nextBallIndex++;

        GameObject ballObj = ObjectPoolManager.Instance.GetObject(ballData.ballPrefab.gameObject, firePoint.position, Quaternion.identity);
        BallController2D ball = ballObj.GetComponent<BallController2D>();

        int damage = CalculateFinalDamage(ballData);
        float wallBonusRate = GetMagicMirrorBonusRate();
        float critChance = ballData.criticalChance + GetPassiveCriticalChanceBonus();
        ball.Initialize(ballData, damage, wallBonusRate, critChance, ballData.criticalDamageRate);
        ball.OnRecovered += HandleBallRecovered;

        ball.Launch(playerShooter.AimDirection);
    }

    private IEnumerator FireInitialBalls()  //시작 시 여러 개의 볼을 순차적으로 발사
    {
        for (int i = 0; i < ballCount; i++)
        {
            FireOneBallImmediate();
            yield return new WaitForSeconds(fireInterval);
        }
    }

    private IEnumerator FireOneBall() //회수된 볼을 일정 시간 후 다시 발사
    {
        yield return new WaitForSeconds(fireInterval);

        if (GameManager.Instance.CurrentState != GameState.Playing) yield break;
        FireOneBallImmediate();
    }

    private void HandleBallRecovered(BallController2D ball) //회수된 볼 제거 후 새로운 볼 발사
    {
        ball.OnRecovered -= HandleBallRecovered;
        ObjectPoolManager.Instance.ReturnObject(ball.gameObject);
        StartCoroutine(FireOneBall());
    }

    private int CalculateFinalDamage(BallData ballData)
    {
        int damage = GetBallDamage(ballData);
        damage = ApplyPassiveDamageBonus(damage);

        return damage;
    }

    private int ApplyPassiveDamageBonus(int damage)  //패시브 적용
    {
        for (int i = 0; i < inventory.OwnedSkills.Count; i++)
        {
            SkillData skill = inventory.OwnedSkills[i].skillData;

            PassiveSkillData passiveSkill = skill as PassiveSkillData;

            if (passiveSkill == null) continue;

            if (passiveSkill.passiveType == PassiveType.WarmHeart)
            {
                int level = inventory.OwnedSkills[i].currentLevel;
                float bonusRate = passiveSkill.levels[level - 1].value;

                damage = Mathf.RoundToInt(damage * (1f + bonusRate));
            }
        }
        return damage;
    }

    private int GetBallDamage(BallData ballData)  //데미지 계산
    {
        for (int i = 0; i < inventory.OwnedSkills.Count; i++)
        {
            SkillData skill = inventory.OwnedSkills[i].skillData;

            ActiveSkillData activeSkill = skill as ActiveSkillData;

            if (activeSkill == null) continue;

            if (activeSkill.linkedBallData == ballData)
            {
                int level = inventory.OwnedSkills[i].currentLevel;
                return activeSkill.levels[level - 1].damage;
            }
        }

        return ballData.damage;
    }
    private float GetMagicMirrorBonusRate()  //패시브용
    {
        for (int i = 0; i < inventory.OwnedSkills.Count; i++)
        {
            PassiveSkillData passiveSkill = inventory.OwnedSkills[i].skillData as PassiveSkillData;

            if (passiveSkill == null) continue;
            if (passiveSkill.passiveType != PassiveType.MagicMirror) continue;

            int level = inventory.OwnedSkills[i].currentLevel;
            return passiveSkill.levels[level - 1].value;
        }

        return 0f;
    }

    private float GetPassiveCriticalChanceBonus()  //치명타 확률 적용
    {
        float bonus = 0f;

        for (int i = 0; i < inventory.OwnedSkills.Count; i++)
        {
            PassiveSkillData passiveSkill = inventory.OwnedSkills[i].skillData as PassiveSkillData;

            if (passiveSkill == null) continue;

            if (passiveSkill.passiveType == PassiveType.RubyDagger || passiveSkill.passiveType == PassiveType.EmeraldDagger)
            {
                int level = inventory.OwnedSkills[i].currentLevel;
                bonus += passiveSkill.levels[level - 1].value;
            }
        }

        return bonus;
    }

    public void AddBall(BallData ballData)
    {
        if (ballData == null) return;

        for (int i = 0; i < equippedBalls.Count; i++)  //중복방지
        {
            if (equippedBalls[i] == ballData) return;
        }

        equippedBalls.Add(ballData);
    }
}
