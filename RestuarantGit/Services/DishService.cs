﻿using AutoMapper;
using Delivery.Resutruant.API.Models.Domain;
using Delivery.Resutruant.API.Models.DTO;
using Delivery.Resutruant.API.Models.Enums;
using Delivery.Resutruant.API.Models.Pagination;
using Delivery.Resutruant.API.Repositories.Interfaces;
using Delivery.Resutruant.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.Resutruant.API.Services
{
    // Service for handling operations related to dishes, including retrieval and filtering.
    public class DishService : IDishService
    {
        private readonly IDishRepository _dishRepository;
        private readonly IMapper _mapper;

        // Constructor to initialize dependencies via dependency injection.
        public DishService(IDishRepository dishRepository, IMapper mapper)
        {
            _dishRepository = dishRepository;
            _mapper = mapper;
        }


        // Retrieves a paginated list of dishes, with optional filtering by category, vegetarian status, and sorting options.
        public async Task<PagedResult<DishDto>> GetAllDishesAsync(
            [FromQuery] Category? category = null,
            [FromQuery] bool? isVegetarian = null,
            [FromQuery] SortOption sortOption = SortOption.NameAsc,
            [FromQuery] int currentPage = 1)
        {
            // Retrieve a paginated result of dishes from the repository based on the provided filters.
            var pagedResult = await _dishRepository.GetAllAsync(category, isVegetarian, sortOption, currentPage);

            var mappedDishes = _mapper.Map<List<DishDto>>(pagedResult.Dishes);

            

            return new PagedResult<DishDto>
            {
                Dishes = mappedDishes,
                Pagination = pagedResult.Pagination
            };
        }

        // Retrieves a specific dish by its unique ID, including its average rating.
        public async Task<DishDto> GetDishByIdAsync(Guid id)
        {
            var dish = await _dishRepository.GetByIdAsync(id);
            if (dish == null)
                return null; // Return null if the dish is not found.

            // Fetch the average rating for the dish.

            var dishDto = _mapper.Map<DishDto>(dish);

            return dishDto;
        }
    }
}
