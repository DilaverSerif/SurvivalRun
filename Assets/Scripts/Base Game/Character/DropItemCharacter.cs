using System.Collections.Generic;
public class DropItemCharacter : CharacterSetup
{
    public List<DropItem> DropList = new List<DropItem>();
    
    protected override void OnStart()
    {
        
    }

    protected override void OnUpdate()
    {
        
    }
    private void DropRandomItem()
    {
        var pos = transform.position;
        pos.y = -0.895f;
        DropList.DropRandomItem(pos);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _character.OnDeath += DropRandomItem;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _character.OnDeath -= DropRandomItem;
    }
}
