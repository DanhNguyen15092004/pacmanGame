using System;
using System.Drawing;

namespace ConvertImgToBitMap
{
    public class Ghost 
    {
        private Point position;
        private Size size;
        private Image[] images; // Mảng chứa các ảnh ghost
        private int currentImageIndex; // Chỉ số ảnh hiện tại
        public int Speed { get; set; }
        private Point direction; // Hướng di chuyển

        // Kích thước của mỗi ảnh
        private const int ImageWidth = 32;
        private const int ImageHeight = 32;
        // Tổng số ảnh
        private const int TotalImages = 8;

        public Ghost(Point startPosition, Size size, string spriteSheetPath, int speed)
        {
            this.position = startPosition;
            this.size = size;
            this.images = LoadImagesFromSpriteSheet(spriteSheetPath); // Gọi hàm để đọc ảnh
            this.Speed = speed;
            this.currentImageIndex = 0; // Mặc định chỉ số ảnh đầu tiên
            this.direction = new Point(1, 0); // Hướng di chuyển ban đầu
        }

        private Image[] LoadImagesFromSpriteSheet(string path)
        {
            Image[] images = new Image[TotalImages]; // Mảng chứa các ảnh
            Bitmap spriteSheet = new Bitmap(path); // Đọc file ảnh

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

        public void Update()
        {
            Move();
        }

        private void Move()
        {
            position.X += direction.X * Speed;
            position.Y += direction.Y * Speed;

            UpdateImageIndex(); // Cập nhật chỉ số ảnh
        }

        private void UpdateImageIndex()
        {
            // Cập nhật chỉ số ảnh dựa trên hướng
            if (direction.X > 0) currentImageIndex = 1; // Di chuyển sang phải
            else if (direction.X < 0) currentImageIndex = 2; // Di chuyển sang trái
            else if (direction.Y > 0) currentImageIndex = 3; // Di chuyển xuống
            else if (direction.Y < 0) currentImageIndex = 0; // Di chuyển lên
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(images[currentImageIndex], new Rectangle(position, size)); // Vẽ ảnh hiện tại
        }

    }
}
