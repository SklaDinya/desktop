using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Exceptions;
using SklaDinya_desktop_BL_component.Forms;
using SklaDinya_desktop_BL_component.Interfaces.Repositories;
using SklaDinya_desktop_BL_component.Models;
using SklaDinya_desktop_BL_component.Queries;

namespace SklaDinya_desktop_BackendMock;

// ═══════════════════════════════════════════════════════════════════════════
//  AuthRepository — мок
// ═══════════════════════════════════════════════════════════════════════════

public class MockAuthRepository : IAuthRepository
{
    /// <summary>Роль, с которой войдёт пользователь. Задаётся извне.</summary>
    public UserRole LoginRole { get; set; } = UserRole.Client;

    public Task<string> LoginAsync(LoginForm form)
    {
        // Определяем роль по логину
        var role = form.Username?.ToLower() switch
        {
            "admin" => UserRole.Admin,
            "operator" or "operator1" => UserRole.StorageOperator,
            _ => UserRole.Client,
        };
        return Task.FromResult(MockData.MakeJwt(role));
    }

    public Task<string> RegisterAsync(RegistrationForm form)
        => Task.FromResult(MockData.MakeJwt(UserRole.Client));
}

// ═══════════════════════════════════════════════════════════════════════════
//  BookingRepository — мок
// ═══════════════════════════════════════════════════════════════════════════

public class MockBookingRepository : IBookingRepository
{
    private readonly List<BookingModel> _bookings = new(MockData.Bookings);

    public Task<List<BookingModel>> GetMyBookingsAsync(BookingSearchQuery query, string token)
        => Task.FromResult(_bookings.ToList());

    public Task<BookingReceiptModel> CreateBookingAsync(BookingCreateForm form, string token)
    {
        var booking = new BookingModel
        {
            Id = Guid.NewGuid(),
            UserId = MockData.UserId1,
            StorageId = form.StorageId,
            Storage = MockData.Storages.FirstOrDefault(s => s.Id == form.StorageId) ?? MockData.Storages[0],
            Cells = MockData.Cells.Where(c => form.CellIds.Contains(c.Id)).ToList(),
            StartTime = form.StartTime,
            BookingTime = form.BookingTime,
            CreatedAt = DateTime.Now,
            Status = BookingStatus.Created,
        };
        _bookings.Add(booking);

        var receipt = new BookingReceiptModel
        {
            Booking = booking,
            Receipt = "mock-receipt-jwt-token",
        };
        return Task.FromResult(receipt);
    }

    public Task<BookingModel> GetMyBookingByIdAsync(Guid bookingId, string token)
        => Task.FromResult(_bookings.First(b => b.Id == bookingId));

    public Task<BookingModel> CancelMyBookingAsync(Guid bookingId, string token)
    {
        var booking = _bookings.First(b => b.Id == bookingId);
        booking.Status = BookingStatus.Canceled;
        return Task.FromResult(booking);
    }

    public Task<List<BookingOperatorModel>> GetStorageBookingsAsync(OperatorBookingSearchQuery query, string token)
        => Task.FromResult(MockData.OperatorBookings);
}

// ═══════════════════════════════════════════════════════════════════════════
//  CellRepository — мок
// ═══════════════════════════════════════════════════════════════════════════

public class MockCellRepository : ICellRepository
{
    private readonly List<CellModel> _cells = new(MockData.Cells);

    public Task<List<CellModel>> GetCellsAsync(Guid storageId, CellSearchQuery query)
        => Task.FromResult(_cells.Where(c => c.StorageId == storageId).ToList());

    public Task<List<string>> GetCellClassesAsync(Guid storageId)
        => Task.FromResult(_cells.Where(c => c.StorageId == storageId).Select(c => c.CellClass).Distinct().ToList());

    public Task<List<CellModel>> GetMyCellsAsync(MyCellSearchQuery query, string token)
        => Task.FromResult(_cells.Where(c => c.StorageId == MockData.StorageId1).ToList());

    public Task<List<string>> GetMyCellClassesAsync(string token)
        => Task.FromResult(_cells.Where(c => c.StorageId == MockData.StorageId1).Select(c => c.CellClass).Distinct().ToList());

    public Task<CellModel> CreateCellAsync(CellCreateForm form, string token)
    {
        var cell = new CellModel
        {
            Id = Guid.NewGuid(),
            StorageId = MockData.StorageId1,
            Name = form.Name,
            CellClass = form.CellClass,
            CreatedAt = DateTime.Now,
        };
        _cells.Add(cell);
        return Task.FromResult(cell);
    }
}

// ═══════════════════════════════════════════════════════════════════════════
//  OperatorRepository — мок
// ═══════════════════════════════════════════════════════════════════════════

public class MockOperatorRepository : IOperatorRepository
{
    private readonly List<OperatorModel> _operators = new(MockData.Operators);

    public Task<List<OperatorModel>> GetOperatorsAsync(OperatorSearchQuery query, string token)
        => Task.FromResult(_operators.ToList());

    public Task<OperatorModel> CreateOperatorAsync(OperatorCreateForm form, string token)
    {
        var op = new OperatorModel
        {
            Id = Guid.NewGuid(),
            Username = form.Username,
            Name = form.Name,
            Email = form.Email,
            Role = form.Role,
            Banned = false,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
        };
        _operators.Add(op);
        return Task.FromResult(op);
    }

    public Task<OperatorModel> GetOperatorByIdAsync(Guid operatorId, string token)
        => Task.FromResult(_operators.First(o => o.Id == operatorId));

    public Task<OperatorModel> UpdateOperatorAsync(Guid operatorId, OperatorUpdateForm form, string token)
    {
        var op = _operators.First(o => o.Id == operatorId);
        if (form.Username is not null) op.Username = form.Username;
        if (form.Name is not null) op.Name = form.Name;
        if (form.Email is not null) op.Email = form.Email;
        if (form.Role.HasValue) op.Role = form.Role.Value;
        if (form.Banned.HasValue) op.Banned = form.Banned.Value;
        op.UpdatedAt = DateTime.Now;
        return Task.FromResult(op);
    }
}

// ═══════════════════════════════════════════════════════════════════════════
//  PaymentRepository — мок
// ═══════════════════════════════════════════════════════════════════════════

public class MockPaymentRepository : IPaymentRepository
{
    private readonly MockBookingRepository _bookingRepo;

    public MockPaymentRepository(MockBookingRepository bookingRepo)
    {
        _bookingRepo = bookingRepo;
    }

    public Task<BookingModel> PayNoopAsync(PaymentForm form, string token)
    {
        // Помечаем последнее Created-бронирование как Paid и возвращаем его —
        // как делает реальный бэкенд, см. IPaymentRepository.
        var booking = MockData.Bookings.LastOrDefault(b => b.Status == BookingStatus.Created)
                      ?? MockData.Bookings.Last();
        booking.Status = BookingStatus.Paid;
        return Task.FromResult(booking);
    }

    public Task<BookingModel> PayRandomAsync(PaymentForm form, string token)
    {
        // 50% шанс
        if (Random.Shared.Next(2) == 0)
            throw new PaymentFailedException("Оплата не прошла. Попробуйте ещё раз.");

        return PayNoopAsync(form, token);
    }
}

// ═══════════════════════════════════════════════════════════════════════════
//  PriceRepository — мок
// ═══════════════════════════════════════════════════════════════════════════

public class MockPriceRepository : IPriceRepository
{
    private readonly List<PriceModel> _prices = new(MockData.Prices);

    public Task<List<PriceModel>> GetPricesAsync(Guid storageId)
        => Task.FromResult(_prices.Where(p => p.StorageId == storageId).ToList());

    public Task<List<PriceModel>> GetMyPricesAsync(string token)
        => Task.FromResult(_prices.Where(p => p.StorageId == MockData.StorageId1).ToList());

    public Task CreatePriceAsync(PriceCreateForm form, string token)
    {
        _prices.Add(new PriceModel
        {
            StorageId = MockData.StorageId1,
            CellClass = form.CellClass,
            Price = form.Price,
            CreatedAt = DateTime.Now,
        });
        return Task.CompletedTask;
    }
}

// ═══════════════════════════════════════════════════════════════════════════
//  StorageRepository — мок
// ═══════════════════════════════════════════════════════════════════════════

public class MockStorageRepository : IStorageRepository
{
    private readonly List<StorageModel> _storages = new(MockData.Storages);

    public Task<List<StorageModel>> GetStoragesAsync(StorageSearchQuery query)
    {
        var result = _storages.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(query.Name))
            result = result.Where(s => s.Name.Contains(query.Name, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(query.Address))
            result = result.Where(s => s.Address.Contains(query.Address, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(result.ToList());
    }

    public Task<List<StorageModel>> GetStorageRequestsAsync(StorageSearchQuery query, string token)
    {
        // В реальном API заявки — это пункты в статусе Created, ожидающие
        // одобрения. Мок возвращает их соответственно.
        var result = _storages.Where(s => s.Status == StorageStatus.Created);
        if (!string.IsNullOrWhiteSpace(query.Name))
            result = result.Where(s => s.Name.Contains(query.Name, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(query.Address))
            result = result.Where(s => s.Address.Contains(query.Address, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(result.ToList());
    }

    public Task CreateStorageAsync(StorageCreateForm form)
    {
        _storages.Add(new StorageModel
        {
            Id = Guid.NewGuid(),
            Name = form.StorageName,
            Address = form.Address,
            Description = form.Description,
            Status = StorageStatus.Created,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
        });
        return Task.CompletedTask;
    }

    public Task<StorageModel> GetStorageByIdAsync(Guid storageId, string token)
        => Task.FromResult(_storages.First(s => s.Id == storageId));

    public Task<StorageModel> UpdateStorageByIdAsync(Guid storageId, StorageUpdateForm form, string token)
    {
        var s = _storages.First(s => s.Id == storageId);
        if (form.Name is not null) s.Name = form.Name;
        if (form.Address is not null) s.Address = form.Address;
        if (form.Description is not null) s.Description = form.Description;
        s.UpdatedAt = DateTime.Now;
        return Task.FromResult(s);
    }

    public Task<StorageModel> ApproveStorageAsync(Guid storageId, string token)
    {
        var s = _storages.First(s => s.Id == storageId);
        s.Status = StorageStatus.Active;
        s.UpdatedAt = DateTime.Now;
        return Task.FromResult(s);
    }

    public Task RejectStorageAsync(Guid storageId, string token)
    {
        _storages.RemoveAll(s => s.Id == storageId);
        return Task.CompletedTask;
    }

    public Task<StorageModel> GetMyStorageAsync(string token)
        => Task.FromResult(_storages.First(s => s.Id == MockData.StorageId1));

    public Task<StorageModel> UpdateMyStorageAsync(StorageUpdateForm form, string token)
        => UpdateStorageByIdAsync(MockData.StorageId1, form, token);
}

// ═══════════════════════════════════════════════════════════════════════════
//  UserRepository — мок
// ═══════════════════════════════════════════════════════════════════════════

public class MockUserRepository : IUserRepository
{
    private readonly List<UserModel> _users = new(MockData.Users);

    public Task<List<UserModel>> GetUsersAsync(UserSearchQuery query, string token)
    {
        var result = _users.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(query.Name))
            result = result.Where(u => u.Name.Contains(query.Name, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(query.Username))
            result = result.Where(u => u.Username.Contains(query.Username, StringComparison.OrdinalIgnoreCase));
        if (query.Role.HasValue)
            result = result.Where(u => u.Role == query.Role.Value);
        return Task.FromResult(result.ToList());
    }

    public Task<UserModel> CreateUserAsync(UserCreateForm form, string token)
    {
        var user = new UserModel
        {
            Id = Guid.NewGuid(),
            Username = form.Username,
            Name = form.Name,
            Email = form.Email,
            Role = form.Role,
            Banned = false,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
        };
        _users.Add(user);
        return Task.FromResult(user);
    }

    public Task<UserModel> GetUserByIdAsync(Guid userId, string token)
        => Task.FromResult(_users.First(u => u.Id == userId));

    public Task<UserModel> UpdateUserAsync(Guid userId, UserUpdateForm form, string token)
    {
        var u = _users.First(u => u.Id == userId);
        if (form.Username is not null) u.Username = form.Username;
        if (form.Name is not null) u.Name = form.Name;
        if (form.Email is not null) u.Email = form.Email;
        if (form.Banned.HasValue) u.Banned = form.Banned.Value;
        u.UpdatedAt = DateTime.Now;
        return Task.FromResult(u);
    }

    public Task<MeModel> GetMeAsync(string token)
    {
        // Определяем роль из токена через JwtHelper
        var payload = SklaDinya_desktop_BL_component.Helpers.JwtHelper.ParsePayload(token);
        return Task.FromResult(MockData.Me(payload.UserRole));
    }

    public Task<string> UpdateMeAsync(MeUpdateForm form, string token)
    {
        // Возвращаем тот же токен — роль не меняется
        return Task.FromResult(token);
    }
}
