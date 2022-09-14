using UnityEngine;

public class Obstacle : Contactable
{
    protected override void Contant(GameObject _gObject)
    {
        if(_gObject.GetComponent<Health>())
            _gObject.GetComponent<Health>().HealthSystem(Value);
        base.Contant(_gObject);
    }
}
