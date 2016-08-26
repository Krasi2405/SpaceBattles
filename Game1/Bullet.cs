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
    class Bullet : Sprite
    {
        public Object Parent;
        public bool isUsed;
        private Vector2 _velocity;

        public Bullet(Texture2D texture, Vector2 position, Object parent, Vector2 velocity) : base(texture, position)
        {
            Parent = parent;
            _velocity = velocity;
        }

        public void Move()
        {
            Position += _velocity;
        }



    }
}
