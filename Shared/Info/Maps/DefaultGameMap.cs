namespace BolyukGame.Shared.Info.Maps
{
    public class DefaultGameMap : GameMap
    {

        public DefaultGameMap()
        {
            Name = "Default";

            Height = 16;
            Width = 32;

            CollisionLayer = new int[Height*Width];

            long pointer = 0;

            for(int y=0; y < Height; y++)
                for(int x=0; x< Width; x++)
                {
                    if(x == 0 || x == Width-1 || y == 0 || y == Height-1)
                        CollisionLayer[pointer] = 1;

                    pointer++;
                }                 
        }
    }
}
