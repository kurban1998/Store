using Database.Models;
using Database.Models.Helpers;

namespace TshirtManagementService.Builder
{
    public sealed class TshirtBuilder : ITshirtBuilder
    {
        public ITshirtBuilder Create()
        {
            _tshirt = new Tshirt();
            return this;
        }

        public ITshirtBuilder SetBrand(string brand)
        {
            _tshirt.Brand = brand;
            return this;
        }

        public ITshirtBuilder SetColor(string color)
        {
            _tshirt.Color = color;
            return this;
        }

        public ITshirtBuilder SetGender(Gender gender)
        {
            _tshirt.Gender = gender;
            return this;
        }

        public ITshirtBuilder SetImageName(string name)
        {
            _tshirt.ImageName = name;
            return this;
        }

        public ITshirtBuilder SetPrice(decimal price)
        {
            _tshirt.Price = price;
            return this;
        }

        public ITshirtBuilder SetSize(Size size)
        {
            _tshirt.Size = size;
            return this;
        }

        public Tshirt Build()
        {
            if(!string.IsNullOrWhiteSpace(_tshirt.Color) &&
               !string.IsNullOrWhiteSpace(_tshirt.Brand) &&
               !string.IsNullOrWhiteSpace(_tshirt.ImageName) &&
               _tshirt.Price > 0 &&
               Enum.IsDefined(typeof(Size), _tshirt.Size) &&
               Enum.IsDefined(typeof(Gender), _tshirt.Gender))
            {
                return _tshirt;
            }
            else
            {
                throw new Exception("Ошибка при создании");
            }
        }

        private Tshirt _tshirt;
    }
}
