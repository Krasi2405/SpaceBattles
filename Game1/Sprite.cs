using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

class Sprite
{
        
    public Rectangle Box;
    public Vector2 Position;
    public int Width { get; private set; }
    public int Height { get; private set; }
    private float _scale = 1f;
    private Texture2D _texture;

    public Sprite(Texture2D texture, Vector2 position)
    {
        _texture = texture;
        Width = _texture.Width;
        Height = _texture.Height;
        Position = position;
    }

    public Sprite(Texture2D texture, Vector2 position, float scale)
    {
        _texture = texture;
        Width = _texture.Width;
        Height = _texture.Height;
        Position = position;
        _scale = scale;
    }

    public int GetWidth()
    {
        return (int)(Width * _scale);
    }

    public int GetHeight()
    {
        return (int)(Height * _scale);
    }

    public void SetScale(float scale)
    {
        _scale = scale;
    }

    public void LoadContent(ContentManager contentManager, string assetName)
    {
        _texture = contentManager.Load<Texture2D>(assetName);
    }

    public void Draw(SpriteBatch batch)
    {
        batch.Begin();
        batch.Draw(_texture, Position, null, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        batch.End();
    }

    public void Draw(SpriteBatch batch, float scale)
    {
        batch.Begin();
        batch.Draw(_texture, Position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        batch.End();
    }

    public void UpdateLogic()
    {
        Box = new Rectangle((int)Position.X, (int)Position.Y, (int)(Width * _scale), (int)(Height * _scale));
    }

    public bool Intersects(Rectangle box1)
    {
        return Box.Intersects(box1);
    }
}

