using UnityEngine;

class SwordLight : Bullet
{
    public override void InteractWith(GameObject gameObject)
    {
        LivingBaseAgent agent = gameObject.GetComponent<LivingBaseAgent>();
        if (agent != null)
        {
            if (!gameObject.CompareTag(UserTag))
            {
                agent.ChangeHealth(-Damage);
                ExtraEffect?.Invoke(agent);
            }
        }
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
    }
}