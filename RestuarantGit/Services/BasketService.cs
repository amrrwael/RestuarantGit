using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Delivery.Resutruant.API.Models.DTO;
using Delivery.Resutruant.API.Models.Domain;
using Delivery.Resutruant.API.Services.Interfaces;
using Delivery.Resutruant.API.Repositories.Interfaces;

namespace Delivery.Resutruant.API.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IDishRepository _dishRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository, IDishRepository dishRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _dishRepository = dishRepository;
            _mapper = mapper;
        }

        public async Task<BasketDto> GetUserBasketAsync(string userEmail)
        {
            Console.WriteLine($"Retrieving basket for user: {userEmail}");
            var basket = await _basketRepository.GetBasketByUserEmailAsync(userEmail);
            if (basket == null)
            {
                Console.WriteLine("Basket not found.");
                return null;
            }

            Console.WriteLine($"Basket found with {basket.Items.Count} items.");
            return _mapper.Map<BasketDto>(basket);
        }

        public async Task<bool> AddDishToBasketAsync(string userEmail, Guid dishId)
        {
            Console.WriteLine($"Adding dish with ID {dishId} to basket for user: {userEmail}");
            var basket = await _basketRepository.GetBasketByUserEmailAsync(userEmail)
                         ?? throw new KeyNotFoundException("Basket not found.");

            var dish = await _dishRepository.GetByIdAsync(dishId)
                        ?? throw new KeyNotFoundException($"Dish with ID {dishId} not found.");

            var basketItem = basket.Items.FirstOrDefault(item => item.DishId == dishId);
            if (basketItem == null)
            {
                basketItem = new BasketItem
                {
                    DishId = dish.Id,
                    Name = dish.Name,
                    Price = dish.Price,
                    Amount = 1,
                    Image = dish.Photo,
                    TotalPrice = dish.Price,
                    BasketId = basket.Id
                };
                basket.Items.Add(basketItem);
            }
            else
            {
                basketItem.Amount++;
                basketItem.TotalPrice = basketItem.Amount * basketItem.Price;
            }

            await _basketRepository.UpdateBasketAsync(basket);
            Console.WriteLine("Dish added successfully.");
            return true;
        }

        public async Task<bool> RemoveDishFromBasketAsync(string userEmail, Guid dishId, bool increase)
        {
            Console.WriteLine($"Removing dish with ID {dishId} from basket for user: {userEmail}");
            var basket = await _basketRepository.GetBasketByUserEmailAsync(userEmail);
            if (basket == null)
            {
                Console.WriteLine("Basket not found.");
                return false;
            }

            var basketItem = basket.Items.FirstOrDefault(item => item.DishId == dishId);
            if (basketItem == null)
            {
                Console.WriteLine("Dish not found in the basket.");
                return false;
            }

            if (increase && basketItem.Amount > 1)
            {
                basketItem.Amount--;
                basketItem.TotalPrice = basketItem.Amount * basketItem.Price;
            }
            else
            {
                basket.Items.Remove(basketItem);
            }

            await _basketRepository.UpdateBasketAsync(basket);
            Console.WriteLine("Dish removed successfully.");
            return true;
        }
    }
}
