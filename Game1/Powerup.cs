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
    class Powerup : Sprite
    {
        public string Name { get; private set;}
        public bool IsActive { get; private set; }
        private int _counter;


        public Powerup(Texture2D texture, Vector2 position, string name, int duration) : base(texture, position)
        {
            IsActive = false;
            Name = name;
            _counter = duration;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public bool IsOver()
        {
            return _counter <= 0;
        }

        public void DecreaseCounter()
        {
            _counter -= 1;
        }

    }
}
