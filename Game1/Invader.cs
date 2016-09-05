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
    class Invader : Sprite
    {
        public bool IsShot = false;
        public int Cooldown { get; private set; }
        public int MaxCooldown { get; private set; }
        private Vector2 Velocity;

        public Invader(Texture2D texture, Vector2 position, Vector2 velocity, int cooldown) : base(texture, position)
        {
            Random random = new Random();
            Velocity = velocity;
            MaxCooldown = cooldown;
            Cooldown = random.Next(cooldown / 2);
        }
        
        public void Move()
        {
            Position += Velocity;
        }

        public void SwapVelocityValues()
        {
            float x = Velocity.X;
            float y = Velocity.Y;

            Velocity.Y = x;
            Velocity.X = y;
        }

        public void ChangeVelocityXSign()
        {
            Velocity.X *= -1;
        }

        public void ChangeVelocityYSign()
        {
            Velocity.Y *= -1;
        }

        public void ResetCooldown()
        {
            Cooldown = 0;
        }

        public void IncreaseCooldown()
        {
            Cooldown += 1;
        }
    }
}
