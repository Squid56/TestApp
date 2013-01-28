using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace FirstTest
{
    public class Sound1
    {
        SoundEffect soundFlowerPick;
        SoundEffect soundBeesBuzzing;
        SoundEffect soundGong;
        SoundEffect soundAngryBuzz;
        SoundEffect soundGameOver;

        SoundEffect soundUmbrella_Spawn;
        SoundEffect soundUmbrella_PickUp;
        SoundEffect soundUmbrella_Open;

        SoundEffectInstance soundBeesBuzzingInstance;
        public bool playonce = true;
        public bool playsoundatstart = true;

        public void LoadContent(Game1 game)
        {
            soundFlowerPick = game.Content.Load<SoundEffect>("Flowerpick");
            soundBeesBuzzing = game.Content.Load<SoundEffect>("groupbuzz");
            soundGong = game.Content.Load<SoundEffect>("gong");
            soundAngryBuzz = game.Content.Load<SoundEffect>("angrybuzz");
            soundGameOver = game.Content.Load<SoundEffect>("gameover");
            // swapped the next 2 on purpose
            soundUmbrella_PickUp = game.Content.Load<SoundEffect>("umbrella_spawn"); ;
            soundUmbrella_Spawn = game.Content.Load<SoundEffect>("umbrella_pickup"); ;
            soundUmbrella_Open = game.Content.Load<SoundEffect>("umbrella_open"); ;
        }

        public void PlayStartSound(Game1 game)
        {
            if (playsoundatstart && !game.isGameOver())
            {
                soundGong.Play(1.0f, 0.0f, 0.0f, false);
                playsoundatstart = false;
            }
        }

        public void PlayGong(Game1 game)
        {
            if(!game.isGameOver())
                soundGong.Play(1.0f, 0.0f, 0.0f, false);
        }

        public void PlayFlowerPick(Game1 game)
        {
            if (!game.isGameOver())
                soundFlowerPick.Play(0.4f, 0.0f, 0.0f, false);
        }

        public void PlayAngryBuzz(Game1 game)
        {
            if (!game.isGameOver())
                soundAngryBuzz.Play(1.0f, 0.0f, 0.0f, false);
        }

        public void PlayUmbrellaSpawn(Game1 game)
        {
            if (!game.isGameOver())
                soundUmbrella_Spawn.Play(1.0f, 0.0f, 0.0f, false);
        }

        public void PlayUmbrellaPickUp(Game1 game)
        {
            if (!game.isGameOver())
                soundUmbrella_PickUp.Play(1.0f, 0.0f, 0.0f, false);
        }

        public void PlayUmbrellaOpen(Game1 game)
        {
            if (!game.isGameOver())
                soundUmbrella_Open.Play(1.0f, 0.0f, 0.0f, false);
        }

        public void PlayGameOver(Game1 game)
        {
            StopBuzzing(game);
            
            if (playonce)
            {
                soundGameOver.Play(1.0f, 0.0f, 0.0f, false);
                playonce = false;
            }
        }

        public void StopBuzzing(Game1 game)
        {
            if (soundBeesBuzzingInstance != null)
                soundBeesBuzzingInstance.Stop();
        }

        public void StartBuzzing(Game1 game)
        {
            if (!game.isGameOver())
            {
                if (soundBeesBuzzingInstance == null)
                    soundBeesBuzzingInstance = soundBeesBuzzing.Play(0.2f, 0.0f, 0.0f, true);
                else
                    soundBeesBuzzingInstance.Resume();
            }
        }
    }
}
