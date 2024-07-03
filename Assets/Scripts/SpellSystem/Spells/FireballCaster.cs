using UnityEngine;

public class FireballCaster : MonoBehaviour, IAimable
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject fireballProjectile;
    

    public void Cast()
    {
        Instantiate(fireballProjectile, player.transform);
    }
}