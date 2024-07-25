namespace ShadowFlareRemake.CombatRestrictedData
{
    public struct ReceivedAttackData
    {
        public int InflictedDamage {  get; private set; }
        public bool IsCritialHit { get; private set; }

        public ReceivedAttackData(int inflictedDamage, bool isCriticalHit)
        {
            InflictedDamage = inflictedDamage;
            IsCritialHit = isCriticalHit;
        }
    }
}
