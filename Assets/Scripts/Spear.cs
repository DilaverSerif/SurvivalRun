using UnityEngine;

public class Spear : Bullet
{
    protected override void OnContact()
    {
        base.OnContact();
        rb.isKinematic = true;
        transform.eulerAngles = new Vector3(115, 0, 0);
    }

    public override void OnEnable()
    {
        rb.isKinematic = false;
        base.OnEnable();
    }
}
