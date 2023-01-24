using System;
using System.Text;

namespace Hollow
{
    public class Utils
    {
        public static void Banner()
        {
            string[] arts =
            {
                "IOKWhOKWhCAgIOKWhOKWhCDiloTiloTiloTiloTiloTiloTiloQg4paE4paE4paEICAgICDiloTiloTiloQgICAgIOKWhOKWhOKWhOKWhOKWhOKWhOKWhCDiloQgICAgIOKWhCANCuKWiCAg4paIIOKWiCAg4paIICAgICAgIOKWiCAgIOKWiCAgIOKWiCAgIOKWiCAgIOKWiCAgICAgICDilogg4paIIOKWhCDilogg4paIDQriloggIOKWiOKWhOKWiCAg4paIICAg4paEICAg4paIICAg4paIICAg4paIICAg4paIICAg4paIICAg4paEICAg4paIIOKWiOKWiCDilojilogg4paIDQriloggICAgICAg4paIICDilogg4paIICDiloggICDiloggICDiloggICDiloggICDiloggIOKWiCDiloggIOKWiCAgICAgICDilogNCuKWiCAgIOKWhCAgIOKWiCAg4paI4paE4paIICDiloggICDilojiloTiloTiloTiloggICDilojiloTiloTiloTiloggIOKWiOKWhOKWiCAg4paIICAgICAgIOKWiA0K4paIICDilogg4paIICDiloggICAgICAg4paIICAgICAgIOKWiCAgICAgICDiloggICAgICAg4paIICAg4paEICAg4paIDQrilojiloTiloTilogg4paI4paE4paE4paI4paE4paE4paE4paE4paE4paE4paE4paI4paE4paE4paE4paE4paE4paE4paE4paI4paE4paE4paE4paE4paE4paE4paE4paI4paE4paE4paE4paE4paE4paE4paE4paI4paE4paE4paIIOKWiOKWhOKWhOKWiA0K",
                "IF9fICAgIF9fICAgICAgICAgIF9fIF9fICAgICAgICAgICAgICAgICAgICAgICANCnwgIFwgIHwgIFwgICAgICAgIHwgIFwgIFwgICAgICAgICAgICAgICAgICAgICAgDQp8IOKWk+KWkyAgfCDilpPilpMgX19fX19fIHwg4paT4paTIOKWk+KWkyBfX19fX18gIF9fICAgX18gICBfXyANCnwg4paT4paTX198IOKWk+KWky8gICAgICBcfCDilpPilpMg4paT4paTLyAgICAgIFx8ICBcIHwgIFwgfCAgXA0KfCDilpPilpMgICAg4paT4paTICDilpPilpPilpPilpPilpPilpNcIOKWk+KWkyDilpPilpMgIOKWk+KWk+KWk+KWk+KWk+KWk1wg4paT4paTIHwg4paT4paTIHwg4paT4paTDQp8IOKWk+KWk+KWk+KWk+KWk+KWk+KWk+KWkyDilpPilpMgIHwg4paT4paTIOKWk+KWkyDilpPilpMg4paT4paTICB8IOKWk+KWkyDilpPilpMgfCDilpPilpMgfCDilpPilpMNCnwg4paT4paTICB8IOKWk+KWkyDilpPilpNfXy8g4paT4paTIOKWk+KWkyDilpPilpMg4paT4paTX18vIOKWk+KWkyDilpPilpNfLyDilpPilpNfLyDilpPilpMNCnwg4paT4paTICB8IOKWk+KWk1zilpPilpMgICAg4paT4paTIOKWk+KWkyDilpPilpNc4paT4paTICAgIOKWk+KWk1zilpPilpMgICDilpPilpMgICDilpPilpMNCiBc4paT4paTICAgXOKWk+KWkyBc4paT4paT4paT4paT4paT4paTIFzilpPilpNc4paT4paTIFzilpPilpPilpPilpPilpPilpMgIFzilpPilpPilpPilpPilpNc4paT4paT4paT4paTIA0KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICANCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgDQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIA0K",
                "IF8gIF8gICAgICAgXyAgXyAgICAgICAgICAgICAgIA0KfCB8fCB8IF9fXyB8IHx8IHwgX19fICBfIF9fIF9fIA0KfCBfXyB8LyBfIFx8IHx8IHwvIF8gXCBcIFYgIFYgLw0KfF98fF98XF9fXy98X3x8X3xcX19fLyAgXF8vXF8vIA0K",
            };
            
            Random random = new Random();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(Encoding.UTF8.GetString(Convert.FromBase64String(arts[random.Next(arts.Length)])));
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Hollow: 1.0.0");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("repository: github.com/xiosec/Hollow");
            Console.WriteLine("twitter: twitter.com/xiosec\n");
            Console.ResetColor();
        }
    }
}
