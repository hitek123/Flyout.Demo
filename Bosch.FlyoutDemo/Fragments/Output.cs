using System.Globalization;

namespace Bosch.FlyoutDemo.Fragments
{
    public class Output
    {
        public Output(int id, string name, bool isOn)
        {
            Id = id;
            Name = name;
            IsOn = isOn;

            switch (id)
            {
                case 253:
                    IdText = "A";
                    break;
                case 254:
                    IdText = "B";
                    break;
                case 255:
                    IdText = "C";
                    break;
                default:
                    IdText = (id).ToString(CultureInfo.InvariantCulture);
                    break;
            }

        }
        public int Id { get; private set; }
        public string Name { get; private set; }
        public bool IsOn { get; private set; }

        public string IdText { get; private set; }
    }
}