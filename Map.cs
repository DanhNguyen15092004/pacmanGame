using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConvertImgToBitMap
{
    public class Map
    {
        private char[,] map_grid;

        // Load the map from a file
        public void LoadMapFromFile()
        {
            // Read all lines from the CSV file
            string[] lines = File.ReadAllLines(@"D:\ConvertImgToBitMap\ConvertImgToBitMap\static\level.csv")
                .Select(line => line.Trim()) // Trim leading and trailing spaces
                .Where(line => !string.IsNullOrWhiteSpace(line)) // Skip empty lines
                .ToArray();


            int rows = 55;
            int cols = 111;

            // Initialize the 2D array
            map_grid = new char[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    map_grid[i, j] = lines[i][j];
                }
            }
        }

        // Render the map onto a Graphics object
        public async Task RenderMap(Graphics g)
        {
            LoadMapFromFile();
            int cellSize = 3; // You can adjust this for visibility

            for (int x = 0; x < map_grid.GetLength(0); x++)
            {
                for (int y = 0; y < map_grid.GetLength(1); y++)
                {
                    char currentChar = map_grid[x, y];

                    switch (currentChar)
                    {
                        case 'x': 
                            g.FillRectangle(Brushes.Blue, y * cellSize /2 , x * cellSize/2, cellSize, cellSize);
                            break;

                        case '-': // Ghost gate
                            g.FillRectangle(Brushes.Pink, y * cellSize * 2, x * cellSize, cellSize, cellSize);
                            break;
                        case '.': 
                            Brush foodBrush = Brushes.Brown;
                            int foodDiameter = cellSize /2; 
                            g.FillEllipse(foodBrush, y * cellSize + (cellSize - foodDiameter) / 2  ,
                                          x * cellSize + (cellSize - foodDiameter) ,
                                          foodDiameter, foodDiameter);
                            break;

                        case ';':
                                g.FillRectangle(Brushes.Black, y * cellSize * 2, x * cellSize, cellSize, cellSize);
                            break;

                        case 'o': // Special food (for ghost fear)
                            Brush specialFoodBrush = Brushes.Brown;
                            int specialFoodDiameter = cellSize / 2; // Larger food size
                            g.FillEllipse(specialFoodBrush, y * cellSize + (cellSize - specialFoodDiameter) / 2,
                                          x * cellSize + (cellSize - specialFoodDiameter) / 1,
                                          specialFoodDiameter, specialFoodDiameter);
                            break;

                        default:
                          
                            break;
                    }
                }
            }
        }

    }
}
