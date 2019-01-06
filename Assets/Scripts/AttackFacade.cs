using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コマンドに応じて適切な攻撃を実行するクラス
/// </summary>
public class AttackFacade
{
    enum Type {
        Single,
        Double,
    }

    AttackBase currentAttack;
    ActorBehaviour user;

    public AttackFacade(ActorBehaviour user) {
        this.user = user;
        currentAttack = null;
    }

    public void Update()
    {
        if (currentAttack == null)
        {
            if (InputManager.Fire1)
            {
                CreateAttack(Type.Single);
            }
            else if (InputManager.Fire3)
            {
                CreateAttack(Type.Double);
            }
        }
        else
        {
            currentAttack.Update();
        }
    }

    /// <summary>
    /// 攻撃動作終了時にコールバックを受け取る
    /// </summary>
    /// <param name="atk_base">Atk base.</param>
    void OnAttackFinished(AttackBase atk_base){
        currentAttack = null;
    }

    /// <summary>
    /// 攻撃制御クラスを生成する
    /// </summary>
    /// <param name="type">生成したい攻撃の種類</param>
    void CreateAttack(Type type) {
        switch(type) {
            case Type.Single:
                currentAttack = new AttackSingle(user);
                break;
            case Type.Double:
                currentAttack = new AttackDouble(user);
                break;
        }
        currentAttack.Excute(user.transform, null);
        currentAttack.onFinished = this.OnAttackFinished;
    }
}

/// <summary>
/// １種類分の攻撃基底クラス
/// </summary>
abstract class AttackBase
{
    public System.Action<AttackBase> onFinished;
    public bool IsFinished { get; private set; }

    protected ActorBehaviour user;

    public AttackBase(ActorBehaviour user) { this.user = user; }
    ~AttackBase() {
        onFinished = null;
        user = null;
        IsFinished = false;
    }

    /// <summary>
    /// 攻撃を実行する
    /// </summary>
    /// <param name="muzzle">攻撃発生場所</param>
    /// <param name="target">攻撃対象(null可能)</param>
    public void Excute(Transform muzzle, Transform target)
    {
        _Begin(muzzle, target);
    }

    public void Update(){
        if (_IsFinished())
        {
            IsFinished = true;
            // 派生クラス側で攻撃終了時に終了を通知
            if (onFinished != null)
            {
                onFinished(this);
                onFinished = null;
            }
        } else {
            _Update();
        }
    }

    protected abstract void _Begin(Transform muzzle, Transform target);
    protected abstract bool _IsFinished();
    protected virtual void _Update() { }
}


/// <summary>
/// 単発飛び道具攻撃
/// </summary>
class AttackSingle : AttackBase
{
    Timer timer;
    public AttackSingle(ActorBehaviour user) : base(user) { }
    protected override void _Begin(Transform muzzle, Transform target)
    {
        Projectile projectile;
        projectile = Projectile.Create();
        projectile.transform.position = muzzle.position + Vector3.up * 2;
        projectile.Fire(muzzle.transform.right);
        timer = new Timer();
        timer.StartCountDown(0.2f, null);
    }

    protected override bool _IsFinished()
    {
        return timer.Complete;
    }
}

/// <summary>
/// 前後飛び道具攻撃
/// </summary>
class AttackDouble : AttackBase
{
    Timer timer;
    public AttackDouble(ActorBehaviour user) : base(user) { }
    protected override void _Begin(Transform muzzle, Transform target)
    {
        // 前方向と後ろ方向に同時に１発ずつ弾を飛ばす
        Projectile projectile;
        projectile = Projectile.Create();
        projectile.transform.position = muzzle.position + Vector3.up * 3 + Vector3.right * 2;
        projectile.Fire(muzzle.transform.right);

        projectile = Projectile.Create();
        projectile.transform.position = muzzle.position + Vector3.up * 1 + Vector3.right * -2;
        projectile.Fire(muzzle.transform.right);

        timer = new Timer();
        timer.StartCountDown(1, null);
    }

    protected override bool _IsFinished()
    {
        return timer.Complete;
    }
}
