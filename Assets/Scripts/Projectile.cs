using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public int damage;

    public int speed = 100;
    public Rigidbody2D rigidbody2;
    public System.Action<Projectile, GameObject> onFinish;

    IEnumerator Start () {
        yield return new WaitForSeconds(1);

        if (onFinish != null) onFinish(this, null);
        Destroy(gameObject);
	}

    public void Fire(Vector3 dir)
    {
        rigidbody2.AddForce(dir * speed, ForceMode2D.Force);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (onFinish != null) onFinish(this, collision.gameObject);
        Destroy(gameObject);
    }

    public static Projectile Create() {
        Object @object = Resources.Load("Prefabs/cap");
        GameObject go = Instantiate(@object) as GameObject;
        return go.GetComponent<Projectile>();
    }
}
