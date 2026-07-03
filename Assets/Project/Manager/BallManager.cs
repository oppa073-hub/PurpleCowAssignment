using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    [SerializeField] private BallData normalBallData;
    [SerializeField] private List<BallData> equippedBalls = new List<BallData>();
    [SerializeField] private int normalBallCount = 5;
    [SerializeField] private Transform firePoint;
    [SerializeField] private PlayerShooter2D playerShooter;
    [SerializeField] private float fireInterval = 0.08f;
    [SerializeField] private PlayerSkillInventory inventory;

    private void Start()
    {
        StartCoroutine(FireInitialBalls());
    }
    private void FireOneBallImmediate(BallData ballData) //현재 장착된 볼 데이터를 기반으로 볼 생성 및 발사
    {
        if (GameManager.Instance.CurrentState != GameState.Playing) return;

        GameObject ballObj = ObjectPoolManager.Instance.GetObject(ballData.ballPrefab.gameObject, firePoint.position, Quaternion.identity);
        BallController2D ball = ballObj.GetComponent<BallController2D>();

        SkillLevelData levelData = GetSkillLevelData(ballData);
        float wallBonusRate = GetMagicMirrorBonusRate();
        float critChance = ballData.criticalChance;
        float rubyBonus = GetPassiveBonusRate(PassiveType.RubyDagger);
        float emeraldBonus = GetPassiveBonusRate(PassiveType.EmeraldDagger);
        ball.Initialize(ballData, levelData, wallBonusRate, critChance, ballData.criticalDamageRate, rubyBonus, emeraldBonus);
        ball.OnRecovered += HandleBallRecovered;

        ball.Launch(playerShooter.AimDirection);
    }

    private IEnumerator FireInitialBalls()  //시작 시 여러 개의 볼을 순차적으로 발사
    {
        // 기본공 5개
        for (int i = 0; i < normalBallCount; i++)
        {
            FireOneBallImmediate(normalBallData);
            yield return new WaitForSeconds(fireInterval);
        }

        // 스킬공
        for (int i = 0; i < equippedBalls.Count; i++)
        {
            FireOneBallImmediate(equippedBalls[i]);
            yield return new WaitForSeconds(fireInterval);
        }
    }

    private IEnumerator FireOneBall(BallData ballData) //회수된 볼을 일정 시간 후 다시 발사
    {
        yield return new WaitForSeconds(fireInterval);

        if (GameManager.Instance.CurrentState != GameState.Playing) yield break;
        FireOneBallImmediate(ballData);
    }

    private void HandleBallRecovered(BallController2D ball) //회수된 볼 제거 후 새로운 볼 발사
    {
        ball.OnRecovered -= HandleBallRecovered;
        BallData recoveredBall = ball.BallData;
        ObjectPoolManager.Instance.ReturnObject(ball.gameObject);
        StartCoroutine(FireOneBall(recoveredBall));
    }

    //private int CalculateFinalDamage(BallData ballData)
    //{
    //    int damage = GetBallDamage(ballData);
    //    damage = ApplyPassiveDamageBonus(damage);
    //
    //    return damage;
    //}
    private SkillLevelData GetSkillLevelData(BallData ballData)
    {
        for (int i = 0; i < inventory.OwnedSkills.Count; i++)
        {
            ActiveSkillData active = inventory.OwnedSkills[i].skillData as ActiveSkillData;

            if (active == null)  continue;

            if (active.linkedBallData != ballData) continue;

            int level = inventory.OwnedSkills[i].currentLevel;

            return active.levels[level - 1];
        }

        return new SkillLevelData()
        {
            damage = ballData.damage
        };
    }

   // private int ApplyPassiveDamageBonus(int damage)  //패시브 적용
   // {
   //     for (int i = 0; i < inventory.OwnedSkills.Count; i++)
   //     {
   //         SkillData skill = inventory.OwnedSkills[i].skillData;
   //
   //         PassiveSkillData passiveSkill = skill as PassiveSkillData;
   //
   //         if (passiveSkill == null) continue;
   //
   //         if (passiveSkill.passiveType == PassiveType.WarmHeart)
   //         {
   //             int level = inventory.OwnedSkills[i].currentLevel;
   //             float bonusRate = passiveSkill.levels[level - 1].value;
   //
   //             damage = Mathf.RoundToInt(damage * (1f + bonusRate));
   //         }
   //     }
   //     return damage;
   // }

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

    private float GetPassiveBonusRate(PassiveType passiveType)
    {
        for (int i = 0; i < inventory.OwnedSkills.Count; i++)
        {
            PassiveSkillData passiveSkill = inventory.OwnedSkills[i].skillData as PassiveSkillData;

            if (passiveSkill == null) continue;
            if (passiveSkill.passiveType != passiveType) continue;

            int level = inventory.OwnedSkills[i].currentLevel;
            return passiveSkill.levels[level - 1].value;
        }

        return 0f;
    }

    public void AddBall(BallData ballData)
    {
        if (ballData == null) return;

        for (int i = 0; i < equippedBalls.Count; i++)  //중복방지
        {
            if (equippedBalls[i] == ballData) return;
        }

        equippedBalls.Add(ballData);
        StartCoroutine(FireOneBall(ballData)); //새로운 공 한개만 발사
    }
}
