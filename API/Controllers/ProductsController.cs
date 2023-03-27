using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;
using API.Errors;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductType> _productTypeRepository;
        private readonly IGenericRepository<ProductBrand> _productBrandRepository;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepository, IGenericRepository<ProductType> productTypeRepository, IGenericRepository<ProductBrand> productBrandRepository, IMapper mapper )
        {
            _productRepository = productRepository;
            _productTypeRepository = productTypeRepository;
            _productBrandRepository = productBrandRepository;
            _mapper = mapper;
        }
 
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _productRepository.ListAsync(spec);
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _productRepository.GetEntityWithSpec(spec);
            if (product == null)
            {
                return NotFound(new ApiResponse(404)); // return 404 error response
            }
            return _mapper.Map<Product, ProductToReturnDto>(product);
        }
        [HttpGet("types")]
        public async Task<ActionResult<List<ProductType>>> GetProductTypes()
        {
            var types = await _productTypeRepository.ListAllAsync();
            return Ok(types);
        }
        [HttpGet("brands")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands()
        {
            var brands = await _productBrandRepository.ListAllAsync();
            return Ok(brands);
        }

        
    }
}