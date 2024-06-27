namespace Interfaces
{
    public interface IDamageable
    {
        public float Health { get; }

        public void TakeDamage(float damage);
    }
}