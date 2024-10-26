using System;
using System.Drawing;

namespace ConvertImgToBitMap
{
    public class Pacman : IDisposable
    {
        private Point position; // Vị trí của Pacman
        private Size size; // Kích thước của Pacman
        private Image[] sprites; // Mảng chứa các ảnh của Pacman
        private int currentImageIndex; // Chỉ số ảnh hiện tại
        public int Speed { get; set; }
        private Point direction; // Hướng di chuyển

        // Kích thước của mỗi ảnh
        private const int ImageWidth = 32;
        private const int ImageHeight = 32;
        // Tổng số ảnh
        private const int TotalImages = 16;

        public Pacman(Point startPosition, Size size, string spriteSheetPath, int speed)
        {
            this.position = startPosition;
            this.size = size;
            this.sprites = LoadImagesFromSpriteSheet(spriteSheetPath); // Gọi hàm để đọc ảnh
            this.Speed = speed;
            this.currentImageIndex = 0; // Mặc định chỉ số ảnh đầu tiên
            this.direction = Point.Empty; // Không có hướng ban đầu
        }

        private Image[] LoadImagesFromSpriteSheet(string path)
        {
            Image[] images = new Image[TotalImages]; 
            Bitmap spriteSheet = new Bitmap(path); 

            for (int i = 0; i < TotalImages; i++)
            {
                // Cắt từng ảnh từ sprite sheet
                Rectangle cropRect = new Rectangle(i * ImageWidth, 0, ImageWidth, ImageHeight);
                Bitmap croppedImage = new Bitmap(ImageWidth, ImageHeight);

                using (Graphics g = Graphics.FromImage(croppedImage))
                {
                    g.DrawImage(spriteSheet, new Rectangle(0, 0, ImageWidth, ImageHeight), cropRect, GraphicsUnit.Pixel);
                }

                images[i] = croppedImage; // Thêm ảnh đã cắt vào mảng
            }

            spriteSheet.Dispose(); // Giải phóng bộ nhớ
            return images;
        }

        public void SetDirection(Point direction)
        {
            this.direction = direction; 
        }

        public void Move()
        {
            position.X += direction.X * Speed; // Cập nhật vị trí theo hướng X
            position.Y += direction.Y * Speed; // Cập nhật vị trí theo hướng Y
            UpdateImageIndex(); // Cập nhật chỉ số ảnh
        }

        private void UpdateImageIndex()
        {
            // Cập nhật chỉ số ảnh dựa trên hướng
            if (direction.X > 0) // Di chuyển sang phải
                currentImageIndex = (currentImageIndex + 1) % 4; 
            else if (direction.X < 0) // Di chuyển sang trái
                currentImageIndex = 4 + (currentImageIndex + 1) % 4; 
            else if (direction.Y < 0) // Di chuyển lên
                currentImageIndex = 8 + (currentImageIndex + 1) % 4; 
            else if (direction.Y > 0) // Di chuyển xuống
                currentImageIndex = 12 + (currentImageIndex + 1) % 4; 
            else 
                currentImageIndex = 0; 
        }

        public void Update()
        {
            Move(); // Cập nhật vị trí của Pacman
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(sprites[currentImageIndex], new Rectangle(position, size)); // Vẽ ảnh hiện tại
        }

        public void Dispose()
        {
            foreach (var img in sprites)
            {
                img.Dispose(); 
            }
        }
    }
}
