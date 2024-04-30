namespace ShadowFlareRemake {
    public interface IUnitHandler {

        public int CurrentHP { get;}
        public int CurrentMP { get;}

        public void TakeDamage(int damage);
    }
}
