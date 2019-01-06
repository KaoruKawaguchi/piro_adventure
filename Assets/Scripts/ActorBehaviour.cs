using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorBehaviour : MonoBehaviour {

    AccelerateMove move;
    Jump jump;
    AttackFacade attack;

    private void Awake()
    {
        move = new AccelerateMove();
        jump = new Jump();
        attack = new AttackFacade(this);

        InputManager.onJump += () =>
        {
            if (jump.CanJump) jump.Excute();
        };
    }

    private void Update()
    {
        float dir = InputManager.Horizontal;
        if (System.Math.Abs(dir) > 0) {
            move.Excute(dir);
        }
        move.Update(transform);
        jump.Update(transform);
        attack.Update();
    }
}

/// <summary>
/// 加速度付き移動
/// </summary>
class AccelerateMove
{
    const float ACCERELATE = 0.02f;    // 加速度
    const float DECERELATE_COEF = 3;   // 減速率係数
    const float MAX_VELOCITY = 0.5f;

    float velocity;             // 現在の移動速度
    bool input = false;         // 入力したフレームはtrue
    float dir;                  // 入力方向

    public void Excute(float dir)
    {
        this.dir = dir;
        velocity += ACCERELATE;
        input = true;
    }

    public void Update(Transform transform)
    {
        // 移動かける
        transform.localPosition += Vector3.right * velocity * dir;

        if (!input)
        {
            // 徐々に減速
            velocity -= velocity * Time.deltaTime * DECERELATE_COEF;
            velocity = Mathf.Clamp(velocity, 0, MAX_VELOCITY);
            if(velocity < 0.02f) {
                velocity = 0;
            }
        }
        input = false;
    }
}

/// <summary>
/// ジャンプ
/// </summary>class Jump
{
    public bool CanJump { get; private set; }

    const float ATTENUATION = 9.8f;    // 減衰度
    const float POW = 2;              // ジャンプ力

    float velocity = 0;

    public void Excute() {
        CanJump = false;
        velocity = POW;
    }

    public void Update(Transform transform) {

        if (CanJump) return;

        velocity -= Time.deltaTime * ATTENUATION;
        transform.position += Vector3.up * velocity;

        if(transform.position.y < 0) {
            // 地面接地したら再度ジャンプ可能
            Vector3 pos = transform.position;
            pos.y = 0;
            transform.position = pos;
            velocity = 0;
            CanJump = true;
        }
    }
}
