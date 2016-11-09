using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    class Player : Sprite
    {
        private List<Powerup> _powerups;

        public Player(Texture2D texture, Vector2 position) : base(texture, position)
        {

        }

        public void ActivatePowerup(string name)
        {
            foreach(Powerup powerup in _powerups)
            {
                if(name == powerup.Name)
                {
                    powerup.Activate();
                }
            }
        }

        public void DecreasePowerupCounter()
        {
            foreach(Powerup powerup in _powerups)
            {
                if(powerup.IsActive)
                {
                    powerup.DecreaseCounter();
                }
            }
        }

        public void RemoveUsedPowerups()
        {
            for(int i = 0; i < _powerups.Count; i++) {
                Powerup powerup = _powerups[i];
                if(powerup.IsOver())
                {
                    _powerups.Remove(powerup);
                }
            }
        }

        public void AddPowerup(Powerup powerup)
        {
            _powerups.Add(powerup);
        }
    }
}
