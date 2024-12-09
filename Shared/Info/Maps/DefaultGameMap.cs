namespace BolyukGame.Shared.Info.Maps
{
    public class DefaultGameMap : GameMap
    {
        public DefaultGameMap()
        {
            Name = "Default";
            MapData = new int[32*16];

            long pointer = 0;

            for(int y=0; y < 15; y++)
                for(int x=0; x< 31; x++)
                {
                    if(x == 0 || x == 15 || y == 0 || y == 15)
                        MapData[pointer] = 1;

                    pointer++;
                }

                    
        }
    }
}
