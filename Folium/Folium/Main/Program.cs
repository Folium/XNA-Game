using System;

namespace Folium.Main
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (GameManager game = new GameManager())
            {
                game.Run();
            }
        }
    }
#endif
}

