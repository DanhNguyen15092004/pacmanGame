using System.Drawing;


public class Entity
{
    public Point Position { get; set; }
    public Size Size { get; set; }
    public Image Image { get; set; }

    public Entity(Point position, Size size, Image image)
    {
        Position = position;
        Size = size;
        Image = image;
    }

    public virtual void Draw(Graphics g)
    {
        g.DrawImage(Image, new Rectangle(Position, Size));
    }
}


