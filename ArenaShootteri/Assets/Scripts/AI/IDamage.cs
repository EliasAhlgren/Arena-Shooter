namespace AI
{
    public interface IDamage
    {
        float IHealth { get; set;}
        void TakeDamage(float damage);
    }
}
