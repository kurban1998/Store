using Database.Models;
using System.Text;

namespace ThisrtApiService.Models
{
    public static class TshitrMapper
    {
        public static IQueryable<TshirtDTO> MapTshirtsToDto(this IQueryable<Tshirt> tshirts)
        {
            return tshirts.Select(tshirt => new TshirtDTO
            {
                TshirtId = tshirt.Id,
                Brand = tshirt.Brand,
                Color = tshirt.Color,
                Size = tshirt.Size,
                Price = tshirt.Price,
                NewPrice = tshirt.PriceOffer != null
                    ? tshirt.PriceOffer.NewPrice
                    : 0,
                ImageName = ParseImagePath(tshirt.ImageName),
                Gender = tshirt.Gender,
                VoterNames = tshirt.Reviews == null
                    ? string.Empty
                    : string.Join(", ", tshirt.Reviews.Select(review => review.VoterName).Take(3)),
                NumStats = tshirt.Reviews.Count() == 0
                    ? 0
                    : Math.Round(tshirt.Reviews.Select(review => (double)review.NumStats).Average(), 1),
                PromotionalText = tshirt.PriceOffer == null
                    ? null
                    : tshirt.PriceOffer.PromotionalText
            });
        }

        public static TshirtDTO MapTshirtToDto(this Tshirt tshirt)
        {
            return new TshirtDTO
            {
                TshirtId = tshirt.Id,
                Brand = tshirt.Brand,
                Color = tshirt.Color,
                Size = tshirt.Size,
                Price = tshirt.Price,
                NewPrice = tshirt.PriceOffer != null
                    ? tshirt.PriceOffer.NewPrice
                    : 0,
                ImageName = ParseImagePath(tshirt.ImageName),
                Gender = tshirt.Gender,
                VoterNames = tshirt.Reviews == null
                    ? string.Empty
                    : string.Join(", ", tshirt.Reviews.Select(review => review.VoterName).Take(3)),
                NumStats = tshirt.Reviews.Count() == 0
                    ? 0
                    : Math.Round(tshirt.Reviews.Select(review => (double)review.NumStats).Average(), 1),
                PromotionalText = tshirt.PriceOffer == null
                    ? null
                    : tshirt.PriceOffer.PromotionalText
            };
        }

        // Отбрасывает ненужную часть пути, оставляя только для вывода на html
        private static string ParseImagePath(string imageName)
        {
            return @$"/img/{imageName}.jpg";

            //var i = path.IndexOf(subString);

            //var newPath = new StringBuilder(25);

            //for (int j = i + 7; j < path.Length; j++)
            //{
            //    newPath.Append(path[j]);
            //}

            //return newPath.ToString();
        }
    }
}
