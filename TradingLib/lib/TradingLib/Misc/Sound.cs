using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Misc
{
    public class Sound
    {

        public static void SoundNotice()
        {
            PlaySound.Sound.Play(@"music\notice.wav", 1);
        }
    }
}
