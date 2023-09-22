namespace ActionGame
{
    public abstract class Item : GameObject
    {

        public Item(PlayScene playScene) : base(playScene)
        {
        }

        public override void Update()
        {

        }

        public override void Draw()
        {
        }

        public override void OnCollision(GameObject other)
        {
        }
    }
}
