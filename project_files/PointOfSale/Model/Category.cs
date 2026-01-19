using System.Windows.Media;

namespace PointOfSale.Model
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Brush Color { get; set; }

        public Category(int id, string name, string colorString)
        {
            Id = id;

            Name = name;

            Color = (Brush) new BrushConverter().ConvertFromString(colorString);
        }
    }
}
