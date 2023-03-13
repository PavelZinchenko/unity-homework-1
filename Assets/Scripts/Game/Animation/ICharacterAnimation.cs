public interface ICharacterAnimation
{
    void SetVelocity(float runSpeedNormalized, float jumpSpeed, bool grounded);
    void Hit();
    void Die();
}
