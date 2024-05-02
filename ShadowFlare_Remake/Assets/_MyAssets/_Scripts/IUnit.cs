namespace ShadowFlareRemake {
    public interface IUnit {

        IUnitStats Stats { get; }

        public int CurrentHP { get;}
        public int CurrentMP { get;}

        public void TakeDamage(int damage);
    }
}
