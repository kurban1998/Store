using Database.Models;
using Database.Models.Helpers;

namespace TshirtManagementService.Builder
{
    public interface ITshirtBuilder
    {
        public ITshirtBuilder Create();

        public ITshirtBuilder SetBrand(string brand);

        public ITshirtBuilder SetColor(string color);

        public ITshirtBuilder SetSize(Size size);

        public ITshirtBuilder SetGender(Gender gender);

        public ITshirtBuilder SetPrice(decimal price);

        public ITshirtBuilder SetImageName(string name);

        public Tshirt Build();
    }
}
