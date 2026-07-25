using AutoMapper;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs.Customer;
using SmartInventory.Application.Exceptions;
using SmartInventory.Application.Interfaces;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CustomerService> _logger;
    private readonly ICurrentUserService _currentUser;

    public CustomerService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CustomerService> logger,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        var customers = await _unitOfWork.CustomerRepository.GetAllAsync();

        _logger.LogInformation("Fetching all customers.");

        return _mapper.Map<IEnumerable<CustomerDto>>(customers);
    }

    public async Task<CustomerDto?> GetByIdAsync(int id)
    {
        var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(id);

        if (customer == null)
            return null;

        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
    {
        var existingCustomer =
            await _unitOfWork.CustomerRepository.GetByEmailAsync(dto.Email);

        if (existingCustomer != null)
            throw new BadRequestException("Customer email already exists.");

        var customer = _mapper.Map<Customer>(dto);

        customer.IsActive = true;
        customer.CreatedOn = DateTime.UtcNow;
        customer.CreatedBy = _currentUser.UserName ?? "System";

        _logger.LogInformation(
            "Creating customer {Email}",
            dto.Email);

        await _unitOfWork.CustomerRepository.AddAsync(customer);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateCustomerDto dto)
    {
        var customer =
            await _unitOfWork.CustomerRepository.GetByIdAsync(id);

        if (customer == null)
            throw new NotFoundException("Customer not found.");

        _mapper.Map(dto, customer);

        customer.ModifiedOn = DateTime.UtcNow;
        customer.ModifiedBy = _currentUser.UserName ?? "System";

        _unitOfWork.CustomerRepository.Update(customer);

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation(
            "Customer {Id} updated.",
            id);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var customer =
            await _unitOfWork.CustomerRepository.GetByIdAsync(id);

        if (customer == null)
            throw new NotFoundException("Customer not found.");

        _unitOfWork.CustomerRepository.Delete(customer);

        await _unitOfWork.SaveChangesAsync();

        _logger.LogWarning(
            "Customer {Id} deleted.",
            id);

        return true;
    }
}